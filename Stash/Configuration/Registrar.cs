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
    using BackingStore;
    using Engine;

    /// <summary>
    /// The starting point for configuring Stash.
    /// </summary>
    public class Registrar<TBackingStore> where TBackingStore : IBackingStore
    {
        private readonly TBackingStore backingStore;
        private PersistenceContext<TBackingStore> persistenceContext;

        public Registrar(TBackingStore backingStore)
        {
            this.backingStore = backingStore;
        }

        public virtual IRegistry Registry
        {
            get { return persistenceContext.Registry; }
        }

        public virtual void ApplyRegistration()
        {
            persistenceContext.Registry.EngageBackingStore();
        }

        /// <summary>
        /// Configure Stash in the required <paramref name="persistenceConfigurationActions"/>.
        /// </summary>
        /// <param name="persistenceConfigurationActions"></param>
        public virtual void PerformRegistration(Action<PersistenceContext<TBackingStore>> persistenceConfigurationActions)
        {
            persistenceContext = new PersistenceContext<TBackingStore>(new Registry(backingStore));
            registerTypeHierarchyIndex();
            persistenceConfigurationActions(persistenceContext);
        }

        private void registerTypeHierarchyIndex()
        {
            persistenceContext.Index(new StashTypeHierarchy());
        }
    }
}