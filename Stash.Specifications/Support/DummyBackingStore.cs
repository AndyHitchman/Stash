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

namespace Stash.Specifications.Support
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Engine;
    using Queries;
    using Serializers;
    using Serializers.Binary;

    public class DummyBackingStore : IBackingStore
    {
        public IQueryFactory QueryFactory
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Count(IQuery query)
        {
            throw new NotImplementedException();
        }

        public void EnsureIndex(IRegisteredIndexer registeredIndexer)
        {
            throw new NotImplementedException();
        }

        public IProjectedIndex ProjectIndex<TKey>(string indexName, TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InternalId> Matching(IQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStoredGraph> Get(IQuery query)
        {
            throw new NotImplementedException();
        }

        public IStoredGraph Get(InternalId internalId)
        {
            throw new NotImplementedException();
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            throw new NotImplementedException();
        }

        public bool IsTypeSupportedInIndexes(Type proposedIndexType)
        {
            throw new NotImplementedException();
        }

        public ISerializer<TGraph> GetDefaultSerialiser<TGraph>(IRegisteredGraph<TGraph> registeredGraph)
        {
            return new BinarySerializer<TGraph>();
        }

        public IStoredGraph CreateStoredGraph(Type graphType)
        {
            throw new NotImplementedException();
        }

        public TReturn InTransactionDo<TReturn>(Func<IStorageWork, TReturn> storageWorkFunction)
        {
            throw new NotImplementedException();
        }
    }
}