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
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using BackingStore;
    using Configuration;

    public class Track<TGraph> : PersistenceEvent<TGraph>
    {
        private readonly IStoredGraph storedGraph;
        private readonly IRegisteredGraph<TGraph> registeredGraph;
        private readonly SHA1CryptoServiceProvider hashCodeGenerator;

        public Track(IStoredGraph storedGraph, IRegisteredGraph<TGraph> registeredGraph)
        {
            this.storedGraph = storedGraph;
            this.registeredGraph = registeredGraph;
            hashCodeGenerator = new SHA1CryptoServiceProvider();
            OriginalHash = hashCodeGenerator.ComputeHash(storedGraph.SerialisedGraph.ToArray());
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
        /// The typed graph.
        /// </summary>
        public TGraph Graph { get; private set; }

        /// <summary>
        /// Get the untypes graph.
        /// </summary>
        public object UntypedGraph
        {
            get { return Graph; }
        }

        protected virtual IEnumerable<IProjectedIndex> CalculateIndexes()
        {
            return 
                registeredGraph.IndexersOnGraph
                .Select(registeredIndexer => registeredIndexer.GetUntypedProjections(Graph))
                .SelectMany(indices => indices);
        }

        public virtual void Complete()
        {
            var serializedGraph = registeredGraph.Serialize(Graph);
            CompletionHash = hashCodeGenerator.ComputeHash(serializedGraph.ToArray());

            if(CompletionHash.SequenceEqual(OriginalHash))
                //No change to object. No work to do.
                return;

            var trackedGraph = new TrackedGraph(storedGraph.InternalId, serializedGraph, CalculateIndexes(), registeredGraph);
        }

        public virtual void PrepareEnrollment() {}

        public virtual PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(IPersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }
    }
}