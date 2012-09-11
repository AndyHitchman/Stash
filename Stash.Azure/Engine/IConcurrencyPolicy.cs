namespace Stash.Azure.Engine
{
    using Microsoft.WindowsAzure.StorageClient;

    public interface IConcurrencyPolicy
    {
        AccessCondition GetAccessConditionForCreation();
        AccessCondition GetAccessConditionForModification(string etag);
    }
}