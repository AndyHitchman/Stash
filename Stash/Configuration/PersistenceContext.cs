namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using Engine;

    /// <summary>
    /// The root context for configuring persistence.
    /// </summary>
    /// <typeparam name="TBackingStore"></typeparam>
    public class PersistenceContext<TBackingStore> where TBackingStore : BackingStore
    {
        private readonly Dictionary<Type, RegisteredGraph> registeredGraphs;

        public PersistenceContext()
        {
            registeredGraphs = new Dictionary<Type, RegisteredGraph>();
        }

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        public virtual IEnumerable<RegisteredGraph> AllRegisteredGraphs
        {
            get { return registeredGraphs.Values; }
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        public virtual RegisteredGraph<TGraph> GetGraphFor<TGraph>()
        {
            return (RegisteredGraph<TGraph>)GetGraphFor(typeof(TGraph));
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <paramref name="graphType"/>.
        /// </summary>
        /// <returns></returns>
        public virtual RegisteredGraph GetGraphFor(Type graphType)
        {
            if(graphType == null) throw new ArgumentNullException("graphType");
            if(!registeredGraphs.ContainsKey(graphType)) throw new ArgumentOutOfRangeException("graphType");

            return registeredGraphs[graphType];
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> and provide an action that performs additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        public virtual void Register<TGraph>(Action<GraphContext<TBackingStore,TGraph>> configurePersistentGraph)
        {
            configurePersistentGraph(new GraphContext<TBackingStore,TGraph>(RegisterGraph<TGraph>()));
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> with no additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public virtual void Register<TGraph>()
        {
            RegisterGraph<TGraph>();
        }

        protected virtual RegisteredGraph<TGraph> RegisterGraph<TGraph>()
        {
            var graph = typeof(TGraph);
            if(registeredGraphs.ContainsKey(graph))
                throw new ArgumentException(string.Format("Graph {0} is already registered", graph));

            var registeredGraph = new RegisteredGraph<TGraph>();
            registeredGraphs.Add(graph, registeredGraph);
            return registeredGraph;
        }
    }
}