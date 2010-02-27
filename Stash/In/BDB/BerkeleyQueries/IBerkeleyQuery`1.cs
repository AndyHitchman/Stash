namespace Stash.In.BDB.BerkeleyQueries
{
    using System;
    using Queries;

    public interface IBerkeleyQuery<TKey> : IBerkeleyQuery, IQuery<TKey> where TKey : IComparable<TKey>, IEquatable<TKey> {}
}