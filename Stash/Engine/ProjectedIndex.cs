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

namespace Stash.Engine
{
    using System;
    using BackingStore;
    using Configuration;

    public class ProjectedIndex<TKey> : IProjectedIndex<TKey>
    {
        public ProjectedIndex(IRegisteredIndexer indexer, TKey key)
        {
            Indexer = indexer;
            Key = key;
        }

        public IRegisteredIndexer Indexer { get; set; }
        public TKey Key { get; private set; }

        public string IndexName
        {
            get { return Indexer.IndexName; }
        }

        public Type TypeOfKey
        {
            get { return typeof(TKey); }
        }

        public object UntypedKey
        {
            get { return Key; }
        }
    }
}