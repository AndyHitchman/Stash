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

namespace Stash.Engine.Partitioning
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Queries;

    public class PartitionedStorageWork : IStorageWork
    {
        private readonly IPartition partition;
        private readonly IStorageWork underlyingStorageWork;

        public PartitionedStorageWork(IPartition partition, IStorageWork underlyingStorageWork)
        {
            this.partition = partition;
            this.underlyingStorageWork = underlyingStorageWork;
        }

        public int Count(IQuery query)
        {
            return underlyingStorageWork.Count(query);
        }

        public void DeleteGraph(InternalId internalId, IRegisteredGraph registeredGraph)
        {
            if(!partition.IsResponsibleForGraph(internalId))
                return;

            underlyingStorageWork.DeleteGraph(internalId, registeredGraph);
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            return underlyingStorageWork.Get(query);
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            return underlyingStorageWork.Matching(query);
        }

        public IStoredGraph Get(InternalId internalId)
        {
            if(!partition.IsResponsibleForGraph(internalId))
                return null;

            try
            {
                return underlyingStorageWork.Get(internalId);
            }
            catch(GraphForKeyNotFoundException)
            {
                //This is normal for partitioned stores.
            }

            return null;
        }

        public void InsertGraph(ITrackedGraph trackedGraph)
        {
            if(!partition.IsResponsibleForGraph(trackedGraph.StoredGraph.InternalId))
                return;

            underlyingStorageWork.InsertGraph(trackedGraph);
        }

        public void UpdateGraph(ITrackedGraph trackedGraph)
        {
            if(!partition.IsResponsibleForGraph(trackedGraph.StoredGraph.InternalId))
                return;

            underlyingStorageWork.UpdateGraph(trackedGraph);
        }
    }
}