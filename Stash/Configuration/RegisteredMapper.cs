#region License

// Copyright 2009 Andrew Hitchman
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

namespace Stash.Configuration
{
    using System.Collections.Generic;
    using Engine;

    /// <summary>
    /// A configured Map.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public abstract class RegisteredMapper<TGraph>
    {
        public abstract void EngageBackingStore(IBackingStore backingStore);

        /// <summary>
        /// Calls the index function and returns a <see cref="Projection"/>
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        public abstract IEnumerable<IProjectedIndex> GetKeyFreeProjections(TGraph graph);
    }
}