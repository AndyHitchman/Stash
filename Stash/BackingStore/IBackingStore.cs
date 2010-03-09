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

    public interface IBackingStore
    {
        /// <summary>
        /// A factory to build queries over indexes.
        /// </summary>
        IQueryFactory Query { get; }

        /// <summary>
        /// Count the number of stored <paramref name="registeredGraph"/> matching the given <paramref name="query"/>.
        /// </summary>
        /// <param name="registeredGraph"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        int Count(IRegisteredGraph registeredGraph, IQuery query);

        /// <summary>
        /// Ensures that the backing store is ready to persist the given <paramref name="registeredIndexer"/>.
        /// </summary>
        /// <param name="registeredIndexer"></param>
        void EnsureIndex(IRegisteredIndexer registeredIndexer);

        /// <summary>
        /// Find and return instances of the <paramref name="registeredGraph"/> that match the given <paramref name="query"/>
        /// </summary>
        /// <param name="registeredGraph"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<IStoredGraph> Find(IRegisteredGraph registeredGraph, IQuery query);

        /// <summary>
        /// Get an instance of a <paramref name="registeredGraph"/> by its <paramref name="internalId"/>.
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="registeredGraph"></param>
        /// <returns></returns>
        IStoredGraph Get(Guid internalId, IRegisteredGraph registeredGraph);

        /// <summary>
        /// Perform some <paramref name="storageWorkActions"/> inside a transaction.
        /// </summary>
        /// <param name="storageWorkActions"></param>
        void InTransactionDo(Action<IStorageWork> storageWorkActions);

        /// <summary>
        /// Call a <paramref name="storageWorkFunction"/> inside a transaction.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="storageWorkFunction"></param>
        /// <returns></returns>
        TReturn InTransactionDo<TReturn>(Func<IStorageWork, TReturn> storageWorkFunction);
    }
}