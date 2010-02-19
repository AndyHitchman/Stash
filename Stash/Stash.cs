namespace Stash
{
    using System;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;

    public static class Stash
    {
        public static void Kickstart<TBackingStore>(
            TBackingStore backingStore, Action<PersistenceContext<TBackingStore>> configurationAction)
            where TBackingStore : IBackingStore
        {
            var registrar = new Registrar<TBackingStore>(backingStore);
            registrar.PerformRegistration(configurationAction);
            registrar.ApplyRegistration();
            Registry = registrar.Registry;
            SessionFactory = new DefaultSessionFactory(registrar.Registry, new DefaultPersistenceEventFactory());
        }

        public static SessionFactory SessionFactory { get; private set; }

        public static Registry Registry { get; set; }
    }
}