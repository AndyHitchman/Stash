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
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using BackingStore;
    using Configuration;

    public class Track : ITrack
    {
        private static readonly SHA1CryptoServiceProvider hashCodeGenerator = new SHA1CryptoServiceProvider();
        private readonly IRegisteredGraph registeredGraph;
        private readonly IStoredGraph storedGraph;

        /// <summary>
        /// <paramref name="session"/> is used transiently in the constructor.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="storedGraph"></param>
        /// <param name="registeredGraph"></param>
        public Track(ISerializationSession session, IStoredGraph storedGraph, IRegisteredGraph registeredGraph)
        {
            this.storedGraph = storedGraph;
            this.registeredGraph = registeredGraph;
            OriginalHash = hashCodeGenerator.ComputeHash(storedGraph.SerialisedGraph);

            //Reset stream after calculating hash.
            storedGraph.SerialisedGraph.Position = 0;
            
            UntypedGraph = registeredGraph.Deserialize(storedGraph.SerialisedGraph, session);
        }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is created.
        /// </summary>
        public byte[] OriginalHash { get; private set; }

        /// <summary>
        /// The hash code calculated from the serialised graph at the time this track is completed.
        /// </summary>
        public byte[] CompletionHash { get; private set; }

        public InternalId InternalId { get { return storedGraph.InternalId; } }

        public object UntypedGraph { get; private set; }

        protected virtual IEnumerable<IProjectedIndex> CalculateIndexes()
        {
            return
                registeredGraph.IndexersOnGraph
                    .Select(registeredIndexer => registeredIndexer.GetUntypedProjections(UntypedGraph))
                    .SelectMany(indices => indices);
        }

        public virtual void Complete(IStorageWork work, ISerializationSession session)
        {
            storedGraph.SerialisedGraph = registeredGraph.Serialize(UntypedGraph, session);

            CompletionHash = hashCodeGenerator.ComputeHash(storedGraph.SerialisedGraph);
            
            //Reset stream after calculating hash.
            storedGraph.SerialisedGraph.Position = 0;

            if(CompletionHash.SequenceEqual(OriginalHash))
                //No change to object. No work to do.
                return;

            var trackedGraph = new TrackedGraph(storedGraph, CalculateIndexes(), registeredGraph);
            work.UpdateGraph(trackedGraph);
        }
    }
}