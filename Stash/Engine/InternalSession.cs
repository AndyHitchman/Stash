#region License
// Copyright 2009 Andrew Hitchman
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

        public InternalSession(IBackingStore backingStore)
        {
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
                                @event.Complete(work);
                            }
                        });
            }
        }

        public virtual void Dispose()
        {
            Complete();
        }

        public virtual void Enroll(IPersistenceEvent persistenceEvent)
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                foreach(
                    var @event in
                        PersistenceEvents.Where(_ => ReferenceEquals(persistenceEvent.UntypedGraph, _.UntypedGraph)))
                {
                    //TODO: Act on answer (determine whether answer is ever useful.)
                    persistenceEvent.SayWhatToDoWithPreviouslyEnrolledEvent(@event);
                }
                PersistenceEvents.Add(persistenceEvent);
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public bool GraphIsTracked(object graph)
        {
            return TrackedGraphs.Any(o => ReferenceEquals(o, graph));
        }

        public virtual IInternalSession Internalize()
        {
            return this;
        }

        public ITrack Track(IStoredGraph storedGraph, IRegisteredGraph registeredGraph)
        {
            var track = new Track(storedGraph, registeredGraph);
            Enroll(track);
            return track;
        }

        public IEndure Endure(object graph, IRegisteredGraph registeredGraph)
        {
            var endure = new Endure(graph, registeredGraph);
            Enroll(endure);
            return endure;
        }
    }
}