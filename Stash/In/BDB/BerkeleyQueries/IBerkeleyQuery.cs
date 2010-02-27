namespace Stash.In.BDB.BerkeleyQueries
{
    using System;
    using System.Collections.Generic;
    using BerkeleyDB;
    using Engine;

    public interface IBerkeleyQuery
    {
        QueryCost QueryCost { get; }
        IEnumerable<Guid> Execute(ManagedIndex managedIndex, Transaction transaction);
    }
}