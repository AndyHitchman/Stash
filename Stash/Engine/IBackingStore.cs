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

namespace Stash.Engine
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Queries;

    public interface IBackingStore
    {
        IQueryFactory QueryFactory { get; }
        int Count(IRegisteredGraph registeredGraph, IQuery query);
        void EnsureIndex(IRegisteredIndexer registeredIndexer);
        IEnumerable<IStoredGraph> Find(IRegisteredGraph registeredGraph, IQuery query);
        IStoredGraph Get(Guid internalId, IRegisteredGraph registeredGraph);
        void InTransactionDo(Action<IStorageWork> storageWorkActions);
        TReturn InTransactionDo<TReturn>(Func<IStorageWork, TReturn> storageWorkFunction);
    }
}