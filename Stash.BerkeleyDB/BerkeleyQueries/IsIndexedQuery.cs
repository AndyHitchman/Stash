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

namespace Stash.BerkeleyDB.BerkeleyQueries
{
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using global::BerkeleyDB;
    using Stash.Configuration;
    using Stash.Engine;
    using Stash.Queries;

    public class IsIndexedQuery : IBerkeleyIndexQuery, IIsIndexedQuery
    {
        private const int pageSizeBufferMultipler = 4;
        private readonly ManagedIndex managedIndex;

        public IsIndexedQuery(ManagedIndex managedIndex, IRegisteredIndexer indexer)
        {
            this.managedIndex = managedIndex;
            Indexer = indexer;
        }

        public IRegisteredIndexer Indexer { get; private set; }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.FullScan; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return managedIndex.Index.FastStats().nPages / (double)pageSizeBufferMultipler * (double)QueryCostScale;
        }

        public IEnumerable<InternalId> Execute(Transaction transaction)
        {
            var cursor = managedIndex.ReverseIndex.Cursor();
            while(cursor.MoveNextUnique())
            {
                yield return cursor.Current.Key.Data.AsInternalId();
            }
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(Transaction transaction, IEnumerable<InternalId> joinConstraint)
        {
            if(joinConstraint.Count() > EstimatedQueryCost(transaction))
                return Execute(transaction);

            var cursor = managedIndex.ReverseIndex.Cursor();

            return
                joinConstraint
                    .Where(joinMatched => cursor.Move(new DatabaseEntry(joinMatched.AsByteArray()), true));
        }
    }
}