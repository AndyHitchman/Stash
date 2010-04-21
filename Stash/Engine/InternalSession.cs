#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BackingStore;
    using Configuration;
    using PersistenceEvents;

    public class InternalSession : IInternalSession
    {
        protected readonly List<IPersistenceEvent> PersistenceEvents;
        private readonly IBackingStore backingStore;
        private readonly ReaderWriterLockSlim enrolledPersistenceEventsLocker = new ReaderWriterLockSlim();
        private readonly IRegistry registry;

        public InternalSession(IRegistry registry) : this(registry, registry.BackingStore) {}

        public InternalSession(IRegistry registry, IBackingStore backingStore)
        {
            this.registry = registry;
            this.backingStore = backingStore;
            PersistenceEvents = new List<IPersistenceEvent>();
        }

        public virtual IEnumerable<IPersistenceEvent> EnrolledPersistenceEvents
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    //A stable clone.
                    return PersistenceEvents.Materialize();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitReadLock();
                }
            }
        }

        public virtual IEnumerable<object> TrackedGraphs
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    return PersistenceEvents.Select(_ => _.UntypedGraph).Materialize();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitReadLock();
                }
            }
        }

        public void Abandon()
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                PersistenceEvents.Clear();
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public virtual void Complete()
        {
            while(PersistenceEvents.Any())
            {
                IEnumerable<IPersistenceEvent> drain;

                enrolledPersistenceEventsLocker.EnterWriteLock();
                try
                {
                    //A stable clone.
                    drain = PersistenceEvents.Materialize();
                    PersistenceEvents.Clear();
                }
                finally
                {
                    enrolledPersistenceEventsLocker.ExitWriteLock();
                }

                backingStore.InTransactionDo(
                    work =>
                        {
                            foreach(var @event in drain)
                            {
                                @event.Complete(work, new SerializationSession(() => drain, this));
                            }
                        });
            }
        }

        public bool Destroy(object graph, IRegisteredGraph registeredGraph)
        {
            var trackedEventForGraph = EnrolledPersistenceEvents.Where(_ => ReferenceEquals(graph, _.UntypedGraph));
            if(!trackedEventForGraph.Any())
                return false;

            var internalId = trackedEventForGraph.Select(_ => _.InternalId).First();
            Enroll(new Destroy(internalId, graph, registeredGraph));
            return true;
        }

        public virtual void Dispose()
        {
            Complete();
        }

        public void Endure(object graph, IRegisteredGraph registeredGraph)
        {
            Enroll(new Endure(this, graph, registeredGraph));
        }

        public void Endure<TGraph>(TGraph graph)
        {
            Endure(graph, registry.GetRegistrationFor<TGraph>());
        }

        public virtual void Enroll(IPersistenceEvent persistenceEvent)
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                PersistenceEvents.Add(persistenceEvent);
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public StashedSet<object> GetEntireStash()
        {
            return
                new StashedSet<object>(
                    this,
                    registry,
                    backingStore,
                    backingStore.QueryFactory);
        }

        public StashedSet<TGraph> GetStashOf<TGraph>() where TGraph : class
        {
            return
                new StashedSet<TGraph>(
                    this,
                    registry,
                    backingStore,
                    backingStore.QueryFactory,
                    new[]
                        {
                            backingStore.QueryFactory.EqualTo(
                                registry.GetIndexerFor<StashTypeHierarchy>(),
                                StashTypeHierarchy.GetConcreteTypeValue(typeof(TGraph)))
                        });
        }


        public virtual IInternalSession Internalize()
        {
            return this;
        }

        public ITrack Track(IStoredGraph storedGraph, IRegisteredGraph registeredGraph, ISerializationSession serializationSession)
        {
            var track = new Track(serializationSession, storedGraph, registeredGraph);
            Enroll(track);
            return track;
        }

        /// <summary>
        /// Get the graph by internal id. If the graph is not tracked, it is fetched from the 
        /// backing store and tracked.
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="serializationSession"></param>
        /// <returns></returns>
        /// <exception cref="GraphForKeyNotFoundException">If the graph is not persisted in the backing store.</exception>
        public object LoadTrackedGraphForInternalId(Guid internalId, ISerializationSession serializationSession)
        {
            var storedGraph = backingStore.Get(internalId);
            var tracked = Track(storedGraph, registry.GetRegistrationFor(storedGraph.GraphType), serializationSession);

            return tracked.UntypedGraph;
        }
    }
}