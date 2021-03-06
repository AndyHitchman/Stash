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

        /// <summary>
        /// Destroy a persistent graph such that when the session completes the graph is removed from the backing store. can only operate on tracked
        /// graphs within the same session.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="registeredGraph"></param>
        /// <returns>true is a tracked graph was marked for deletion</returns>
        bool Destroy(object graph, IRegisteredGraph registeredGraph);

        /// <summary>
        /// Endure a transient graph so that when the session completes the graph is persisted to the backing store.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="registeredGraph"></param>
        void Endure(object graph, IRegisteredGraph registeredGraph);

        /// <summary>
        /// Track a stored graph so that changes made in the session are persisted to the backing store.
        /// </summary>
        /// <param name="storedGraph"></param>
        /// <param name="registeredGraph"></param>
        /// <param name="serializationSession"></param>
        /// <param name="untracked"></param>
        /// <returns></returns>
        ITrack Load(IStoredGraph storedGraph, IRegisteredGraph registeredGraph, ISerializationSession serializationSession, bool untracked);

        /// <summary>
        /// Get the graph by internal id. If the graph is not tracked, it is fetched from the 
        /// backing store and tracked.
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="serializationSession"></param>
        /// <param name="untracked"></param>
        /// <returns></returns>
        /// <exception cref="GraphForKeyNotFoundException">If the graph is not persisted in the backing store.</exception>
        object LoadTrackedGraphForInternalId(InternalId internalId, ISerializationSession serializationSession, bool untracked);
    }
}