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
    using Configuration;
    using PersistenceEvents;

    public interface InternalSession : Session
    {
        /// <summary>
        /// The registered configuration.
        /// </summary>
        Registry Registry { get; }

        /// <summary>
        /// The engaged backing store.
        /// </summary>
        IBackingStore BackingStore { get; }

        /// <summary>
        /// An internal repository for use by the session.
        /// </summary>
        UnenlistedRepository InternalRepository { get; }

        /// <summary>
        /// Persistence events enrolled in the session.
        /// </summary>
        IEnumerable<PersistenceEvent> EnrolledPersistenceEvents { get; }

        /// <summary>
        /// Graphs tracked by the session.
        /// </summary>
        IEnumerable<object> TrackedGraphs { get; }

        PersistenceEventFactory PersistenceEventFactory { get; set; }

        /// <summary>
        /// Manage the persistence event.
        /// </summary>
        /// <param name="persistenceEvent"></param>
        void Enroll(PersistenceEvent persistenceEvent);

        /// <summary>
        /// True if the graph is being tracked by this session.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        bool GraphIsTracked(object graph);
    }
}