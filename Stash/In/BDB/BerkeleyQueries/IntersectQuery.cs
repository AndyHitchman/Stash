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
    using System.Linq;
    using BerkeleyDB;
    using Configuration;
    using Queries;

    public class IntersectQuery : IBerkeleyQuery, IIntersectQuery
    {
        private readonly IBerkeleyQuery lhs;
        private readonly IBerkeleyQuery rhs;
        private JoinExecutionOrder joinExecutionOrder;

        public IntersectQuery(IQuery lhs, IQuery rhs)
        {
            this.lhs = (IBerkeleyQuery)lhs;
            this.rhs = (IBerkeleyQuery)rhs;
        }

        public QueryCostScale QueryCostScale
        {
            get { return QueryCostScale.MultiGet; }
        }

        public double EstimatedQueryCost(ManagedIndex managedIndex, Transaction transaction)
        {
            var lhsCost = lhs.EstimatedQueryCost(managedIndex, transaction);
            var rhsCost = rhs.EstimatedQueryCost(managedIndex, transaction);
            joinExecutionOrder = lhsCost < rhsCost ? JoinExecutionOrder.LeftFirst : JoinExecutionOrder.RightFirst;
            return lhsCost + rhsCost;
        }

        public IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction)
        {
            //TODO: After we execute the cheaper side, pass the results to the other side to act as a filter.
            //Will probably mean all queries need to be reworked.
            //Ignore execution order for now.

            return lhs.Execute(managedIndex, transaction).Intersect(rhs.Execute(managedIndex, transaction));
        }
    }
}