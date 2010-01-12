namespace Stash.Configuration
{
    using System;
    using Engine;

    /// <summary>
    /// The root context for configuring persistence.
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    public class PersistenceContext<TBackingStore> where TBackingStore : BackingStore
    {
        public PersistenceContext(Registry registry)
        {
            Registry = registry;
        }

        public Registry Registry { get; private set; }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> and provide an action that performs additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        public virtual void Register<TGraph>(Action<GraphContext<TBackingStore, TGraph>> configurePersistentGraph) where TGraph : class
        {
            configurePersistentGraph(new GraphContext<TBackingStore, TGraph>(Registry.RegisterGraph<TGraph>()));
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> with no additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public virtual void Register<TGraph>() where TGraph : class
        {
            Registry.RegisterGraph<TGraph>();
        }
    }
}