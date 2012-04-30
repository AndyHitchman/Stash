namespace Stash.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;
    using Queries;

    public static class StashedSetExtensions
    {
        public static IEnumerable<string> SharedSignatureUrls<TGraph>(this IStashedSet<TGraph> stashedSet, SharedAccessPolicy accessPolicy) where TGraph : class
        {
            var internalSet = (StashedSet<TGraph>)stashedSet;
            return
                internalSet
                    .GetMatchingStoredGraphs()
                    .Select(storedGraph => ((AzureBackingStore)Kernel.Registry.BackingStore).GetBlobReferenceForInternalId(storedGraph.InternalId))
                    .Select(blob => blob.GetSharedAccessSignature(accessPolicy));
        }
    }
}