namespace Stash
{
    using System.Collections.Generic;
    using Queries;

    public interface IStashedSet<TGraph> : IEnumerable<TGraph> where TGraph : class 
    {
        StashedSet<TGraph> Where(IQuery query);
        void Add(TGraph item);
        bool Contains(TGraph item);
        bool Remove(TGraph item);
        int Count { get; }
    }
}