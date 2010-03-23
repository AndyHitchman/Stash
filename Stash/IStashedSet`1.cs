namespace Stash
{
    using System.Collections.Generic;
    using Queries;

    public interface IStashedSet<TGraph> : IEnumerable<TGraph> where TGraph : class 
    {
        StashedSet<TGraph> Where(IQuery query);
        void Endure(TGraph item);
        bool Contains(TGraph item);
        void Destroy(TGraph item);
        int Count { get; }
    }
}