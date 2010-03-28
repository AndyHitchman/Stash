namespace Stash.Queries
{
    using System.Collections.Generic;
    using Configuration;

    public class MakeConstraint
    {
        private readonly IRegistry registry;
        private readonly IQueryFactory queryFactory;

        public MakeConstraint(IRegistry registry, IQueryFactory queryFactory)
        {
            this.registry = registry;
            this.queryFactory = queryFactory;
        }

        public SetConstraint<TIndex> Where<TIndex>() where TIndex : IIndex
        {
            return new SetConstraint<TIndex>(registry, queryFactory);
        }

        public IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries)
        {
            return queryFactory.IntersectionOf(queries);
        }

        public IIntersectOperator IntersectionOf(IQuery lhs, IQuery rhs)
        {
            return IntersectionOf(new[] { lhs, rhs });
        }

        public IUnionOperator UnionOf(IQuery lhs, IQuery rhs)
        {
            return UnionOf(new[] { lhs, rhs });
        }

        public IUnionOperator UnionOf(IEnumerable<IQuery> queries)
        {
            return queryFactory.UnionOf(queries);
        }

    }
}