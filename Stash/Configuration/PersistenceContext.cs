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
        public IEnumerable<RegisteredGraph> AllRegisteredGraphs
        {
            get { return registeredGraphs.Values; }
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        public RegisteredGraph<TGraph> GetGraphFor<TGraph>()
        {
            return (RegisteredGraph<TGraph>)GetGraphFor(typeof(TGraph));
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <paramref name="graphType"/>.
        /// </summary>
        /// <returns></returns>
        public RegisteredGraph GetGraphFor(Type graphType)
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