namespace Stash.Engine
{
    public interface ITrackedGraph<TGraph> : ITrackedGraph
    {
        TGraph Graph { get; }
    }
}