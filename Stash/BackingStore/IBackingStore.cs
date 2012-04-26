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

namespace Stash.BackingStore
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Queries;
    using Serializers;

    public interface IBackingStore
    {
        /// <summary>
        /// A factory to build queries over indexes.
        /// </summary>
        IQueryFactory QueryFactory { get; }

        void Close();

        /// <summary>
        /// Count the number of stored graphs matching the given <paramref name="query"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int Count(IQuery query);

        /// <summary>
        /// Ensures that the backing store is ready to persist the given <paramref name="registeredIndexer"/>.
        /// </summary>
        /// <param name="registeredIndexer"></param>
        void EnsureIndex(IRegisteredIndexer registeredIndexer);

        /// <summary>
        /// Provide a projection for a specific index key in a platform dependant implementation.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IProjectedIndex ProjectIndex<TKey>(string indexName, TKey key);

        /// <summary>
        /// Find and return the <see cref="InternalId"/> of graphs that match the given <paramref name="query"/>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<InternalId> Matching(IQuery query);

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
        IStoredGraph Get(InternalId internalId);

        /// <summary>
        /// Perform some <paramref name="storageWorkActions"/> inside a transaction.
        /// </summary>
        /// <param name="storageWorkActions"></param>
        void InTransactionDo(Action<IStorageWork> storageWorkActions);

        /// <summary>
        /// Indicates whether a type yielded by an index is supported by the backing store
        /// </summary>
        /// <param name="proposedIndexType"></param>
        /// <returns></returns>
        bool IsTypeASupportedInIndexes(Type proposedIndexType);

        /// <summary>
        /// Get the default serializer for the backing store.
        /// </summary>
        /// <param name="registeredGraph"></param>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        ISerializer<TGraph> GetDefaultSerialiser<TGraph>(IRegisteredGraph<TGraph> registeredGraph);
    }
}