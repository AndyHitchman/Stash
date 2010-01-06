namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using Engine;

    public class RegisteredStash
    {
        public RegisteredStash()
        {
            RegisteredGraphs = new Dictionary<Type, RegisteredGraph>();
        }

        public Dictionary<Type, RegisteredGraph> RegisteredGraphs { get; private set; }

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        public virtual IEnumerable<RegisteredGraph> AllRegisteredGraphs
        {
            get { return RegisteredGraphs.Values; }
        }

        /// <summary>
        /// Engage the backing store in managing the stash.
        /// </summary>
        public void EngageBackingStore(BackingStore backingStore)
        {
            foreach(var registeredGraph in AllRegisteredGraphs)
            {
                registeredGraph.EngageBackingStore(backingStore);
            }
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
            if(!RegisteredGraphs.ContainsKey(graphType)) throw new ArgumentOutOfRangeException("graphType");

            return RegisteredGraphs[graphType];
        }

        public virtual RegisteredGraph<TGraph> RegisterGraph<TGraph>()
        {
            var graph = typeof(TGraph);
            if(RegisteredGraphs.ContainsKey(graph))
                throw new ArgumentException(string.Format("Graph {0} is already registered", graph));

            var registeredGraph = new RegisteredGraph<TGraph>();
            RegisteredGraphs.Add(graph, registeredGraph);
            return registeredGraph;
        }
    }
}