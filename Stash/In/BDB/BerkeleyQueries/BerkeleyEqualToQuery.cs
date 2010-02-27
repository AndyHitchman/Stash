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

namespace Stash.In.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using BerkeleyDB;
    using Configuration;
    using Engine;
    using Queries;

    public class BerkeleyEqualToQuery<TKey> : IBerkeleyQuery<TKey>, IEqualToQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public BerkeleyEqualToQuery(IRegisteredIndexer indexer, TKey key)
        {
            Indexer = indexer;
            Key = key;
        }

        public IRegisteredIndexer Indexer { get; private set; }
        public TKey Key { get; private set; }

        public QueryCost QueryCost
        {
            get { return QueryCost.SingleGet; }
        }

        public IEnumerable<IStoredGraph> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBerkeleyQuery<TKey> : IBerkeleyQuery, IQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        
    }

    public enum QueryCost
    {
        SingleGet = 1,
        MultiGet = 10,
        RangeScan = 50,
        FullScan = 100,
    }

    public interface IBerkeleyQuery
    {
        QueryCost QueryCost { get; }
        IEnumerable<IStoredGraph> Execute(ManagedIndex managedIndex, Transaction transaction);
    }
}