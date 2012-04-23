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

namespace Stash.Azure.AzureQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.Queries;
    using Stash.Engine;

    public class UnionOperator : IAzureQuery, IUnionOperator
    {
        private readonly IEnumerable<IAzureQuery> queries;

        public UnionOperator(IEnumerable<IQuery> queries)
        {
            this.queries = queries.OfType<IAzureQuery>();
            if(!this.queries.Any())
                throw new ArgumentException("No executable queries passed", "queries");
        }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(TableServiceContext serviceContext)
        {
            return queries.Aggregate(0D, (cost, query) => cost + query.EstimatedQueryCost(serviceContext));
        }

        public IEnumerable<InternalId> Execute(TableServiceContext serviceContext)
        {
            return
                queries
                    .OrderBy(_ => _.EstimatedQueryCost(serviceContext))
                    .Aggregate(
                        Enumerable.Empty<InternalId>(),
                        (matching, query) => matching.Union(query.Execute(serviceContext)).Materialize()
                    );
        }

        public IEnumerable<InternalId> ExecuteInsideIntersect(TableServiceContext serviceContext, IEnumerable<InternalId> joinConstraint)
        {
            return
                queries
                    .OrderBy(_ => _.EstimatedQueryCost(serviceContext))
                    .Aggregate(
                        Enumerable.Empty<InternalId>(),
                        (matching, query) => matching.Union(query.ExecuteInsideIntersect(serviceContext, joinConstraint)).Materialize()
                    );
        }
    }
}