namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engine;
    using Engine.Serializers;

    public class Registry
    {
        public Registry()
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
        public virtual void EngageBackingStore(BackingStore backingStore)
        {
            BackingStore = backingStore;
            foreach(var registeredGraph in AllRegisteredGraphs)
            {
                registeredGraph.EngageBackingStore(backingStore);
            }
        }

        public virtual BackingStore BackingStore { get; private set; }

        public Func<Serializer> Serializer { get; set; }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        public virtual RegisteredGraph<TGraph> GetRegistrationFor<TGraph>()
        {
            return (RegisteredGraph<TGraph>)GetRegistrationFor(typeof(TGraph));
        }

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <paramref name="graphType"/>.
        /// </summary>
        /// <returns></returns>
        public virtual RegisteredGraph GetRegistrationFor(Type graphType)
        {
            if(graphType == null) throw new ArgumentNullException("graphType");
            if(!RegisteredGraphs.ContainsKey(graphType)) throw new ArgumentOutOfRangeException("graphType");

            return RegisteredGraphs[graphType];
        }

        public virtual RegisteredGraph<TGraph> RegisterGraph<TGraph>() where TGraph : class
        {
            var graph = typeof(TGraph);
            if(RegisteredGraphs.ContainsKey(graph))
                throw new ArgumentException(string.Format("Graph {0} is already registered", graph));

            var registeredGraph = new RegisteredGraph<TGraph>();
            RegisteredGraphs.Add(graph, registeredGraph);
            return registeredGraph;
        }

        public virtual bool IsManagingGraphTypeOrAncestor(Type graphType)
        {
            return AllRegisteredGraphs.Any(rg => rg.GraphType.IsAssignableFrom(graphType));
        }
    }
}