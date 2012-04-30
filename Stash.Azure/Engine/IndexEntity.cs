namespace Stash.Azure.Engine
{
    using Microsoft.WindowsAzure.StorageClient;

    public class IndexEntity : TableServiceEntity
    {
        public string OriginalValue { get; set; }
    }
}