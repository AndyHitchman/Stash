#region License
// Copyright 2009, 2010 Andrew Hitchman
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

namespace Stash.Azure
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Configuration;
    using Engine;
    using Microsoft.WindowsAzure;
    using Queries;

    public class AzureBackingStore : IBackingStore, IDisposable
    {
        private readonly CloudStorageAccount cloudStorageAccount;
        private bool isDisposed;

        public AzureBackingStore(CloudStorageAccount cloudStorageAccount)
        {
            this.cloudStorageAccount = cloudStorageAccount;
        }

        public IQueryFactory QueryFactory
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
        }

        public int Count(IQuery query)
        {
            return InTransactionDo(work => work.Count(query));
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

        public TReturn InTransactionDo<TReturn>(Func<IStorageWork,TReturn> storageWorkActions)
        {
            throw new NotImplementedException();
        }

        public void InTransactionDo(Action<IStorageWork> storageWorkActions)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (isDisposed) return;
            isDisposed = true;

            Close();
        }
    }
}