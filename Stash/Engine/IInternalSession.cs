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
    using BackingStore;
    using Configuration;
    using PersistenceEvents;

    public interface IInternalSession : ISession
    {
        /// <summary>
        /// Persistence events enrolled in the session.
        /// </summary>
        IEnumerable<IPersistenceEvent> EnrolledPersistenceEvents { get; }

        /// <summary>
        /// Graphs tracked by the session.
        /// </summary>
        IEnumerable<object> TrackedGraphs { get; }

        IPersistenceEventFactory PersistenceEventFactory { get; set; }

        /// <summary>
        /// True if the graph is being tracked by this session.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        bool GraphIsTracked(object graph);

        /// <summary>
        /// Track a stored graph such that changes made in the session are persisted to the backing store.
        /// </summary>
        /// <param name="storedGraph"></param>
        /// <param name="registeredGraph"></param>
        /// <returns></returns>
        ITrack Track(IStoredGraph storedGraph, IRegisteredGraph registeredGraph);

        IEndure Endure(object graph, IRegisteredGraph registeredGraph);
    }
}