namespace Stash.Azure.Engine
{
    using Microsoft.WindowsAzure.StorageClient;

    public class NoConcurrencyProtection : IConcurrencyPolicy
    {
        public AccessCondition GetAccessConditionForCreation()
        {
            return AccessCondition.None;
        }

        public AccessCondition GetAccessConditionForModification(string etag)
        {
            return AccessCondition.None;
        }
    }
}