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

namespace Stash.BackingStore.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using Queries;

    public class IntersectOperator : IBerkeleyQuery, IIntersectOperator
    {
        private readonly IEnumerable<IBerkeleyQuery> queries;

        public IntersectOperator(IEnumerable<IQuery> queries)
        {
            this.queries = queries.OfType<IBerkeleyQuery>();
            if(!this.queries.Any())
                throw new ArgumentException("No executables queries passed", "queries");
        }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(ManagedIndex managedIndex, Transaction transaction)
        {
            return queries.Aggregate(0D, (cost, query) => cost + query.EstimatedQueryCost(managedIndex, transaction));
        }

        public IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            return
                queries
                    .Skip(1)
                    .OrderBy(_ => _.EstimatedQueryCost(managedIndex, transaction))
                    .Aggregate(
                        queries.First().Execute(managedIndex, transaction),
                        (guids, query) => guids.Intersect(query.ExecuteInsideIntersect(managedIndex, transaction, guids))
                    );
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(ManagedIndex managedIndex, Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            return
                queries
                    .OrderBy(_ => _.EstimatedQueryCost(managedIndex, transaction))
                    .Aggregate(
                        joinConstraint,
                        (guids, query) => guids.Intersect(query.ExecuteInsideIntersect(managedIndex, transaction, guids))
                    );
        }
    }
}