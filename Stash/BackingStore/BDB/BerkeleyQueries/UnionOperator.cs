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

namespace Stash.BackingStore.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BerkeleyDB;
    using Queries;
    using Engine;

    public class UnionOperator : IBerkeleyQuery, IUnionOperator
    {
        private readonly IEnumerable<IBerkeleyQuery> queries;

        public UnionOperator(IEnumerable<IQuery> queries)
        {
            this.queries = queries.OfType<IBerkeleyQuery>();
            if(!this.queries.Any())
                throw new ArgumentException("No executable queries passed", "queries");
        }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(Transaction transaction)
        {
            return queries.Aggregate(0D, (cost, query) => cost + query.EstimatedQueryCost(transaction));
        }

        public IEnumerable<Guid> Execute(Transaction transaction)
        {
            return
                queries
                    .OrderBy(_ => _.EstimatedQueryCost(transaction))
                    .Aggregate(
                        Enumerable.Empty<Guid>(),
                        (guids, query) => guids.Union(query.Execute(transaction)).Materialize()
                    );
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            return
                queries
                    .OrderBy(_ => _.EstimatedQueryCost(transaction))
                    .Aggregate(
                        Enumerable.Empty<Guid>(),
                        (matching, query) => matching.Union(query.ExecuteInsideIntersect(transaction, joinConstraint)).Materialize()
                    );
        }
    }
}