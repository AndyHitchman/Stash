namespace Stash
{
    using System;
    using Configuration;

    public static class Stash
    {
        public static void ConfigurePersistence<TBackingStore>(TBackingStore backingStore, Action<PersistenceContext> configurationAction) where TBackingStore : BackingStore
        {
            new ConfigurationEngine<TBackingStore>(backingStore).ConfigurePersistence(configurationAction);
        }
    }
}