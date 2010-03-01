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

    public class IntersectOperator : IBerkeleyQuery, IIntersectOperator
    {
        private readonly IBerkeleyQuery lhs;
        private readonly IBerkeleyQuery rhs;
        private JoinExecutionOrder joinExecutionOrder;

        public IntersectOperator(IQuery lhs, IQuery rhs)
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
            EstimatedQueryCost(managedIndex, transaction);

            var cheapQuery = joinExecutionOrder == JoinExecutionOrder.LeftFirst ? lhs : rhs;
            var expensiveQuery = joinExecutionOrder == JoinExecutionOrder.LeftFirst ? rhs : lhs;

            var cheapResults = cheapQuery.Execute(managedIndex, transaction).ToList();
            var expensiveResults = expensiveQuery.ExecuteInsideIntersect(managedIndex, transaction, cheapResults);
            return cheapResults.Intersect(expensiveResults);
        }

        public IEnumerable<Guid> ExecuteInsideIntersect(ManagedIndex managedIndex, Transaction transaction, IEnumerable<Guid> joinConstraint)
        {
            EstimatedQueryCost(managedIndex, transaction);

            var cheapQuery = joinExecutionOrder == JoinExecutionOrder.LeftFirst ? lhs : rhs;
            var expensiveQuery = joinExecutionOrder == JoinExecutionOrder.LeftFirst ? rhs : lhs;

            var cheapResults = cheapQuery.ExecuteInsideIntersect(managedIndex, transaction, joinConstraint).ToList();
            var expensiveResults = expensiveQuery.ExecuteInsideIntersect(managedIndex, transaction, cheapResults);
            return cheapResults.Intersect(expensiveResults);
        }
    }
}