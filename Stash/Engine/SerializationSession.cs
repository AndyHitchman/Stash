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
    using BackingStore;
    using PersistenceEvents;

    public class SerializationSession : ISerializationSession
    {
        private bool untracked;
        public Func<IEnumerable<IPersistenceEvent>> GetCurrentPersistenceEvents { get; private set; }
        public IInternalSession InternalSession { get; private set; }
        public Dictionary<InternalId, object> ActivelyDeserialising { get; set; }

        public SerializationSession(Func<IEnumerable<IPersistenceEvent>> getCurrentPersistenceEvents, IInternalSession internalSession, bool untracked)
        {
            GetCurrentPersistenceEvents = getCurrentPersistenceEvents;
            InternalSession = internalSession;
            ActivelyDeserialising = new Dictionary<InternalId, object>();
            this.untracked = untracked;
        }


        public bool GraphIsTracked(InternalId internalId)
        {
            return
                GetCurrentPersistenceEvents()
                    .Where(_ => _.InternalId == internalId)
                    .Any();

        }

        public void RecordActiveDeserialization(InternalId internalId, object graph)
        {
            ActivelyDeserialising.Add(internalId, graph);
        }

        /// <summary>
        ///   Get the graph by internal id. If the graph is not tracked, it is fetched from the 
        ///   backing store and tracked.
        /// </summary>
        /// <param name = "internalId"></param>
        /// <returns></returns>
        /// <exception cref = "GraphForKeyNotFoundException">If the graph is not persisted in the backing store.</exception>
        public object TrackedGraphForInternalId(InternalId internalId)
        {
            if(ActivelyDeserialising.ContainsKey(internalId))
                return ActivelyDeserialising[internalId];

            var tracked =
                GetCurrentPersistenceEvents()
                    .Where(_ => _.InternalId == internalId)
                    .Select(_ => _.UntypedGraph).FirstOrDefault();

            if(tracked != null)
                return tracked;

            var loading = InternalSession.LoadTrackedGraphForInternalId(internalId, this, untracked);

            return loading;
        }

        /// <summary>
        ///   Get the internal id of a graph if it is tracked.
        /// </summary>
        /// <param name = "graph"></param>
        /// <returns></returns>
        public InternalId InternalIdOfTrackedGraph(object graph)
        {
            return GetCurrentPersistenceEvents().Where(_ => ReferenceEquals(_.UntypedGraph, graph)).Select(_ => _.InternalId).FirstOrDefault();
        }
    }
}