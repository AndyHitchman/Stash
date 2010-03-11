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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BackingStore;
    using Configuration;
    using PersistenceEvents;

    public class InternalSession : IInternalSession
    {
        protected readonly List<IPersistenceEvent> enrolledPersistenceEvents;
        private readonly ReaderWriterLockSlim enrolledPersistenceEventsLocker = new ReaderWriterLockSlim();

        public InternalSession(Registry registry, IPersistenceEventFactory persistenceEventFactory)
        {
            Registry = registry;
            PersistenceEventFactory = persistenceEventFactory;
            InternalRepository = new DefaultUnenlistedRepository();
            enrolledPersistenceEvents = new List<IPersistenceEvent>();
        }

        public IUnenlistedRepository InternalRepository { get; private set; }

        public Registry Registry { get; private set; }
        public IPersistenceEventFactory PersistenceEventFactory { get; set; }

        public IBackingStore BackingStore
        {
            get { return Registry.BackingStore; }
        }

        public virtual IEnumerable<IPersistenceEvent> EnrolledPersistenceEvents
        {
            get
            {
                enrolledPersistenceEventsLocker.EnterReadLock();
                try
                {
                    return enrolledPersistenceEvents.ToList();
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
                    return enrolledPersistenceEvents.Select(_ => _.UntypedGraph).ToList();
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
                enrolledPersistenceEvents.Clear();
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }
        }

        public virtual void Complete()
        {
            while(enrolledPersistenceEvents.Any())
            {
                phaseComplete();
            }
        }

        public virtual void Dispose()
        {
            Complete();
        }

        public virtual IEnlistedRepository EnlistRepository(IUnenlistedRepository unenlistedRepository)
        {
            return new DefaultEnlistedRepository(this, unenlistedRepository);
        }

        public void Enroll(IPersistenceEvent persistenceEvent)
        {
            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                foreach(
                    var @event in
                        enrolledPersistenceEvents.Where(_ => ReferenceEquals(persistenceEvent.UntypedGraph, _.UntypedGraph)))
                {
                    //TODO: Act on answer (determine whether answer is ever useful.)
                    persistenceEvent.SayWhatToDoWithPreviouslyEnrolledEvent(@event);
                }
                enrolledPersistenceEvents.Add(persistenceEvent);
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

        private void phaseComplete()
        {
            List<IPersistenceEvent> drain;

            enrolledPersistenceEventsLocker.EnterWriteLock();
            try
            {
                drain = enrolledPersistenceEvents.ToList();
                enrolledPersistenceEvents.Clear();
            }
            finally
            {
                enrolledPersistenceEventsLocker.ExitWriteLock();
            }

            foreach(var @event in drain)
            {
                @event.Complete();
            }
        }
    }
}