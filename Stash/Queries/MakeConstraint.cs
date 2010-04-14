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

namespace Stash.Queries
{
    using System.Collections.Generic;
    using Configuration;

    public class MakeConstraint
    {
        private readonly IQueryFactory queryFactory;
        private readonly IRegistry registry;

        public MakeConstraint(IRegistry registry, IQueryFactory queryFactory)
        {
            this.registry = registry;
            this.queryFactory = queryFactory;
        }

        /// <summary>
        /// The logical intersection (boolean 'and') of the set of queries. 
        /// Only graphs that match all queries are returned.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries)
        {
            return queryFactory.IntersectionOf(queries);
        }

        /// <summary>
        /// The logical intersection (boolean 'and') of the left-hand side and right-hand side queries.
        /// Only graphs that match both queries are returned.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public IIntersectOperator IntersectionOf(IQuery lhs, IQuery rhs)
        {
            return IntersectionOf(new[] {lhs, rhs});
        }

        /// <summary>
        /// The logical union (boolean 'or') of the left-hand side and right-hand side queries.
        /// Graphs that match either query are returned.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public IUnionOperator UnionOf(IQuery lhs, IQuery rhs)
        {
            return UnionOf(new[] {lhs, rhs});
        }

        /// <summary>
        /// The logical union (boolean 'or') of the set of queries.
        /// Graphs that match any query are returned.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IUnionOperator UnionOf(IEnumerable<IQuery> queries)
        {
            return queryFactory.UnionOf(queries);
        }

        /// <summary>
        /// A match constraint against a specific <typeparamref name="TIndex"/> index implementing <see cref="IIndex"/>.
        /// </summary>
        /// <typeparam name="TIndex"></typeparam>
        /// <returns></returns>
        public SetConstraint<TIndex> Where<TIndex>() where TIndex : IIndex
        {
            return new SetConstraint<TIndex>(registry, queryFactory);
        }
    }
}