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

namespace Stash.BackingStore
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Queries;

    public interface IStorageWork
    {
        /// <summary>
        /// Count the number of stored graphs matching the given <paramref name="query"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int Count(IQuery query);

        void DeleteGraph(Guid internalId, IRegisteredGraph registeredGraph);

        /// <summary>
        /// Find and return instances of graphs that match the given <paramref name="query"/>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<IStoredGraph> Get(IQuery query);

        /// <summary>
        /// Get an instance of a graph by its <paramref name="internalId"/>.
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IStoredGraph Get(Guid internalId);
        
        /// <summary>
        /// Insert a tracked graph.
        /// </summary>
        /// <param name="trackedGraph"></param>
        void InsertGraph(ITrackedGraph trackedGraph);

        /// <summary>
        /// Update a tracked graph.
        /// </summary>
        /// <param name="trackedGraph"></param>
        void UpdateGraph(ITrackedGraph trackedGraph);
    }
}