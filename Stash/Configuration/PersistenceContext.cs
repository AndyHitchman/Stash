namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;

    public class PersistenceContext
    {
        private readonly Dictionary<Type, RegisteredGraph> registeredGraphs;

        public PersistenceContext()
        {
            registeredGraphs = new Dictionary<Type, RegisteredGraph>();
        }

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        public IEnumerable<RegisteredGraph> RegisteredGraphs
        {
            get { return registeredGraphs.Values; }
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> and provide an action that performs additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        public void Register<TGraph>(Action<GraphContext<TGraph>> configurePersistentGraph)
        {
            configurePersistentGraph(new GraphContext<TGraph>(registerGraph<TGraph>()));
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/> with no additional configuration.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public void Register<TGraph>()
        {
            registerGraph<TGraph>();
        }

        private RegisteredGraph<TGraph> registerGraph<TGraph>()
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