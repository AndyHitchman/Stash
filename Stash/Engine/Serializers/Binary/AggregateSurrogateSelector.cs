namespace Stash.Engine.Serializers.Binary
{
    using System;
    using System.Runtime.Serialization;
    using Configuration;

    /// <summary>
    /// A custom surrogate selector to enable serialisation and deserialisation of referenced
    /// to aggregate roots by internal id.
    /// </summary>
    public class AggregateSurrogateSelector : ISurrogateSelector 
    {
        private readonly IRegisteredGraph registeredGraph;
        private bool rootIsSerialised;

        public AggregateSurrogateSelector(IRegisteredGraph registeredGraph)
        {
            this.registeredGraph = registeredGraph;
            rootIsSerialised = false;
        }

        public void ChainSelector(ISurrogateSelector selector)
        {
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = this;

            if (!rootIsSerialised && type.Equals(registeredGraph.GraphType))
            {
                rootIsSerialised = true;
                return null;
            }

            if (registeredGraph.Registry.IsManagingGraphTypeOrAncestor(type))
                return new AggregateSurrogate();

            return null;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return null;
        }
    }
}