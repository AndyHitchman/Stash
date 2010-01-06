namespace Stash
{
    using System;
    using Configuration;
    using Engine;

    public static class Stash
    {
        public static void ConfigurePersistence<TBackingStore>(
            TBackingStore backingStore, Action<PersistenceContext<TBackingStore>> configurationAction)
            where TBackingStore : BackingStore
        {
            new Registrar<TBackingStore>(backingStore).ConfigurePersistence(configurationAction);
        }
    }
}