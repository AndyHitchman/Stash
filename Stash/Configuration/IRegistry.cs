﻿#region License
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
    using BackingStore;

    public interface IRegistry
    {
        Dictionary<Type, RegisteredGraph> RegisteredGraphs { get; }
        List<IRegisteredIndexer> RegisteredIndexers { get; }

        /// <summary>
        /// The aggregate object graphs currently configured.
        /// </summary>
        IEnumerable<IRegisteredGraph> AllRegisteredGraphs { get; }

        IBackingStore BackingStore { get; }

        /// <summary>
        /// Engage the backing store in managing the stash.
        /// </summary>
        void EngageBackingStore();

        IRegisteredIndexer GetIndexerFor(Type indexType);
        IRegisteredIndexer GetIndexerFor(IIndex index);
        IRegisteredIndexer GetIndexerFor<TIndex>() where TIndex : IIndex;

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <typeparamref name="TGraph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <returns></returns>
        IRegisteredGraph<TGraph> GetRegistrationFor<TGraph>();

        /// <summary>
        /// Get the <see cref="RegisteredGraph{TGraph}"/> for a given type <paramref name="graphType"/>.
        /// </summary>
        /// <returns></returns>
        IRegisteredGraph GetRegistrationFor(Type graphType);

        bool IsManagingGraphTypeOrAncestor(Type graphType);
        RegisteredGraph<TGraph> RegisterGraph<TGraph>() where TGraph : class;
        void RegisterIndexer<TGraph, TKey>(RegisteredIndexer<TGraph, TKey> registeredIndexer) where TKey : IComparable<TKey>, IEquatable<TKey>;
    }
}