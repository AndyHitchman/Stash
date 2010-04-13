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

namespace Stash.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Engine;

    /// <summary>
    /// A configured Index.
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RegisteredIndexer<TGraph, TKey> : RegisteredIndexer<TGraph> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public RegisteredIndexer(IIndex<TGraph, TKey> index)
        {
            Index = index;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        public virtual IIndex<TGraph, TKey> Index { get; private set; }

        public override Type IndexType
        {
            get { return Index.GetType(); }
        }

        public override string IndexName
        {
            get { return IndexType.FullName; }
        }

        public override Type YieldType
        {
            get { return IndexType.GetMethod("Yield").ReturnType.GetGenericArguments()[0]; }
        }

        public override void EngageBackingStore(IBackingStore backingStore)
        {
            backingStore.EnsureIndex(this);
        }

        public override IEnumerable<IProjectedIndex> GetUntypedProjections(object graph)
        {
            return Index.Yield((TGraph)graph).Select(key => new ProjectedIndex<TKey>(this, key)).Cast<IProjectedIndex>();
        }
    }
}