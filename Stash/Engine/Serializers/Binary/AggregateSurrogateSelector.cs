#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Engine.Serializers.Binary
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Configuration;

    /// <summary>
    /// A custom surrogate selector to enable serialisation and deserialisation of referenced
    /// to aggregate roots by internal id.
    /// </summary>
    public class AggregateSurrogateSelector : ISurrogateSelector
    {
        private readonly IRegisteredGraph registeredGraph;
        private ISurrogateSelector nextSelector;
        private readonly AggregateReferenceSurrogate aggregateReferenceSurrogate;
        private readonly AggregateRootSurrogate aggregateRootSurrogate;

        public AggregateSurrogateSelector(IRegisteredGraph registeredGraph)
        {
            this.registeredGraph = registeredGraph;
            aggregateReferenceSurrogate = new AggregateReferenceSurrogate();
            aggregateRootSurrogate = new AggregateRootSurrogate(aggregateReferenceSurrogate);
        }

        public void ChainSelector(ISurrogateSelector selector)
        {
            if(nextSelector == null)
                nextSelector = selector;
            else
                nextSelector.ChainSelector(selector);
        }

        public ISurrogateSelector GetNextSelector()
        {
            return nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = this;

            if(type.Equals(registeredGraph.GraphType))
                return aggregateRootSurrogate;

            if(registeredGraph.Registry.IsManagingGraphTypeOrAncestor(type))
                return aggregateReferenceSurrogate;

            return null;
        }
    }
}