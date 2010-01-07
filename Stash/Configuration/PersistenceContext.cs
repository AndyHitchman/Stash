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
        public PersistenceContext(Registration registration)
        {
            Registration = registration;
        }

        public Registration Registration { get; private set; }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> and provide an action that performs additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        public virtual void Register<TGraph>(Action<GraphContext<TBackingStore, TGraph>> configurePersistentGraph)
        {
            configurePersistentGraph(new GraphContext<TBackingStore, TGraph>(Registration.RegisterGraph<TGraph>()));
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> with no additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public virtual void Register<TGraph>()
        {
            Registration.RegisterGraph<TGraph>();
        }
    }
}