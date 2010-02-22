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

namespace Stash
{
    using System;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;

    public static class Stash
    {
        public static SessionFactory SessionFactory { get; private set; }

        public static Registry Registry { get; set; }

        public static void Kickstart<TBackingStore>(
            TBackingStore backingStore, Action<PersistenceContext<TBackingStore>> configurationAction)
            where TBackingStore : IBackingStore
        {
            var registrar = new Registrar<TBackingStore>(backingStore);
            registrar.PerformRegistration(configurationAction);
            registrar.ApplyRegistration();
            Registry = registrar.Registry;
            SessionFactory = new DefaultSessionFactory(registrar.Registry, new DefaultPersistenceEventFactory());
        }
    }
}