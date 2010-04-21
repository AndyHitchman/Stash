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

namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using BackingStore;
    using Configuration;

    public class Track : ITrack
    {
        private readonly object graph;
        private readonly SHA1CryptoServiceProvider hashCodeGenerator;
        private readonly IRegisteredGraph registeredGraph;

        /// <summary>
        /// <paramref name="session"/> is used transiently in the constructor.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="storedGraph"></param>
        /// <param name="registeredGraph"></param>
        public Track(ISerializationSession session, IStoredGraph storedGraph, IRegisteredGraph registeredGraph)
            : this(session, storedGraph.InternalId, storedGraph.SerialisedGraph, registeredGraph) {}

        public Track(ISerializationSession session, Guid internalId, IEnumerable<byte> storedSerializedGraph, IRegisteredGraph registeredGraph)
            : this()
        {
            InternalId = internalId;
            this.registeredGraph = registeredGraph;
            OriginalHash = hashCodeGenerator.ComputeHash(storedSerializedGraph.ToArray());
            graph = registeredGraph.Deserialize(storedSerializedGraph, session);
        }

        protected Track(Guid internalId, object graph, IRegisteredGraph registeredGraph)
            : this()
        {
            InternalId = internalId;
            this.graph = graph;
            this.registeredGraph = registeredGraph;
            OriginalHash = new byte[0];
        }

        private Track()
        {
            hashCodeGenerator = new SHA1CryptoServiceProvider();
        }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is created.
        /// </summary>
        public byte[] OriginalHash { get; private set; }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is completed.
        /// </summary>
        public byte[] CompletionHash { get; private set; }

        public Guid InternalId { get; private set; }

        /// <summary>
        /// Get the untyped graph.
        /// </summary>
        public object UntypedGraph
        {
            get { return graph; }
        }

        protected virtual IEnumerable<IProjectedIndex> CalculateIndexes()
        {
            return
                registeredGraph.IndexersOnGraph
                    .Select(registeredIndexer => registeredIndexer.GetUntypedProjections(UntypedGraph))
                    .SelectMany(indices => indices);
        }

        public virtual void Complete(IStorageWork work, ISerializationSession session)
        {
            CompleteInBackingStore(trackedGraph => work.UpdateGraph(trackedGraph), session);
        }

        protected void CompleteInBackingStore(Action<ITrackedGraph> completionAction, ISerializationSession session)
        {
            var transientSerializedGraph = registeredGraph.Serialize(UntypedGraph, session);
            CompletionHash = hashCodeGenerator.ComputeHash(transientSerializedGraph.ToArray());

            if(CompletionHash.SequenceEqual(OriginalHash))
                //No change to object. No work to do.
                return;

            var trackedGraph = new TrackedGraph(InternalId, transientSerializedGraph, CalculateIndexes(), registeredGraph);
            completionAction(trackedGraph);
        }
    }
}