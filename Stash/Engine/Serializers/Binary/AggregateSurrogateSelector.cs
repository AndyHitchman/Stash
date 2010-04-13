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

        public void ChainSelector(ISurrogateSelector selector) {}

        public ISurrogateSelector GetNextSelector()
        {
            return null;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = this;

            if(!rootIsSerialised && type.Equals(registeredGraph.GraphType))
            {
                rootIsSerialised = true;
                return null;
            }

            if(registeredGraph.Registry.IsManagingGraphTypeOrAncestor(type))
                return new AggregateSurrogate();

            return null;
        }
    }
}