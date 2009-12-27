namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;

    public abstract class ConfigureStash
    {
        protected ConfigureStash()
        {
            registeredGraphs = new Dictionary<Type, RegisteredGraph>();
        }

        private readonly IDictionary<Type, RegisteredGraph> registeredGraphs;

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        public IEnumerable<RegisteredGraph> RegisteredGraphs { get { return registeredGraphs.Values; } }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="configurePersistentGraph"></param>
        protected void For<TGraph>(Action<GraphContext<TGraph>> configurePersistentGraph)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configure Stash for the <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        protected void For<TGraph>()
        {
            var graph = typeof(TGraph);
            if(registeredGraphs.ContainsKey(graph))
                throw new ArgumentException(string.Format("Graph {0} is already registered", typeof(TGraph)));

            registeredGraphs.Add(graph, new RegisteredGraph<TGraph>());
        }
    }
}