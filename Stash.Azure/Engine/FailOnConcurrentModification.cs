namespace Stash.Azure.Engine
{
    using Microsoft.WindowsAzure.StorageClient;

    public class FailOnConcurrentModification : IConcurrencyPolicy
    {
        public AccessCondition GetAccessConditionForCreation()
        {
            return AccessCondition.IfNoneMatch("*");
        }

        public AccessCondition GetAccessConditionForModification(string etag)
        {
            return AccessCondition.IfMatch(etag);
        }
    }
}