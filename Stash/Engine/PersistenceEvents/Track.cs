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
        private readonly SHA1CryptoServiceProvider hashCodeGenerator;
        private readonly IRegisteredGraph registeredGraph;
        private object graph;
        private readonly IEnumerable<byte> storedSerializedGraph;
        private readonly Guid internalId;

        public Track(IStoredGraph storedGraph, IRegisteredGraph registeredGraph)
            : this(storedGraph.InternalId, storedGraph.SerialisedGraph, registeredGraph)
        {
        }

        public Track(Guid internalId, IEnumerable<byte> storedSerializedGraph, IRegisteredGraph registeredGraph)
            : this()
        {
            this.internalId = internalId;
            this.storedSerializedGraph = storedSerializedGraph;
            this.registeredGraph = registeredGraph;
            OriginalHash = hashCodeGenerator.ComputeHash(storedSerializedGraph.ToArray());
            graph = registeredGraph.Deserialize(storedSerializedGraph);
        }

        public Track(Guid internalId, object graph, IRegisteredGraph registeredGraph)
            : this()
        {
            this.internalId = internalId;
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

        public Guid InternalId { get; set; }

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

        public virtual void Complete(IStorageWork work)
        {
            var transientSerializedGraph = registeredGraph.Serialize(UntypedGraph);
            CompletionHash = hashCodeGenerator.ComputeHash(transientSerializedGraph.ToArray());

            if(CompletionHash.SequenceEqual(OriginalHash))
                //No change to object. No work to do.
                return;

            var trackedGraph = new TrackedGraph(internalId, transientSerializedGraph, CalculateIndexes(), registeredGraph);
            work.UpdateGraph(trackedGraph);
        }

        public virtual PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(IPersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }
    }
}