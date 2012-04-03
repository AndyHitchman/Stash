namespace Stash.Azure.AzureQueries
{
    using Configuration;

    public interface IAzureIndexQuery : IAzureQuery
    {
        IRegisteredIndexer Indexer { get; }
    }
}