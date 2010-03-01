namespace Stash.Queries
{
    public interface IComplementaryQuery<TQuery> where TQuery : IQuery
    {
        /// <summary>
        /// Used by the not unary operator to get the complement (i.e. yielding the inverse set of 
        /// results) of its given query.
        /// The implementor has two basic strategies and may decide which to implement
        /// given their knowledge of the underlying storage architecture.
        /// 1) Actually return the complementary query and let the not operator execture this, or
        /// 2) Return a graph table scan wrapper around the original query that gets everything
        /// but the results of the original query.
        /// The first option will more generally be more efficient.
        /// </summary>
        /// <returns></returns>
        TQuery GetComplementaryQuery();
    }
}