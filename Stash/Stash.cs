namespace Stash
{
    using System;
    using Configuration;

    public static class Stash
    {
        public static void Configure<TBackingStore>(Action<PersistenceContext> configurationAction) where TBackingStore : BackingStore
        {
            new ConfigurationEngine<TBackingStore>().Persist(configurationAction);
        }
    }
}