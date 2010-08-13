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

namespace Stash.Specifications.for_backingstore_bsb.given_managed_index
{
    using System;
    using System.IO;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB;
    using BackingStore.BDB.BerkeleyConfigs;
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_registering_a_new_index_on_existing_graph_data : with_temp_dir
    {
        private IRegistry registry;
        private SessionFactory sessionFactory;
        private BerkeleyBackingStore backingStore;

        protected override void Given()
        {
            var intBackingStore = new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir));
            var registrar = new Registrar<BerkeleyBackingStore>(intBackingStore);
            registrar.PerformRegistration(
                context =>
                    {
                        context.Graph<GraphAffectedByNewIndex>();
                        context.Index(new ExistingIndex());
                    });
            registrar.ApplyRegistration();
            var intRegistry = registrar.Registry;
            var intSessFactory = new SessionFactory(intRegistry);

            using(var session = intSessFactory.GetSession())
            {
                session.Endure(new GraphAffectedByNewIndex { Text = "TheGraph"});
            }

            intBackingStore.Close();
        }

        protected override void When()
        {
            backingStore = new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir));
            var registrar = new Registrar<BerkeleyBackingStore>(backingStore);
            registrar.PerformRegistration(
                context =>
                {
                    context.Graph<GraphAffectedByNewIndex>();
                    context.Index(new ExistingIndex());
                    context.Index(new NewIndex());
                });
            registrar.ApplyRegistration();
            registry = registrar.Registry;
            sessionFactory = new SessionFactory(registry);
        }

        [Then]
        public void it_should_populate_the_index()
        {
            backingStore.IndexDatabases[registry.RegisteredIndexers[1].IndexName].Index.Stats().nKeys.ShouldNotEqual(0);
        }

        [Then]
        public void it_should_populate_the_index_for_the_graph()
        {
            using(var session = sessionFactory.GetSession())
            {
                session
                    .GetEntireStash()
                    .Matching(_ => _.Where<NewIndex>().EqualTo("TheGraph"))
                    .Materialize()
                    .ShouldHaveCount(1);
            }
        }

        protected override void TidyUp()
        {
            backingStore.Close();
            base.TidyUp();
        }
    }
}