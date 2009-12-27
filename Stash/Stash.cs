namespace Stash
{
    using System;
    using Configuration;

    public static class Stash
    {
        public static void Configure<TBackingStore>(TBackingStore backingStore, Action<PersistenceContext> configurationAction) where TBackingStore : BackingStore
        {
            new ConfigurationEngine<TBackingStore>(backingStore).Persist(configurationAction);
        }
    }
}