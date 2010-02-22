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

namespace Stash.Specifications.for_configuration.given_persistence_context
{
    using System;
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_told_to_register_a_graph : with_persistence_context
    {
        [Test]
        public void it_should_insist_that_the_graph_is_not_already_registered()
        {
            Sut.Register<DummyPersistentObject>();
            typeof(ArgumentException)
                .ShouldBeThrownBy(
                    () =>
                    Sut.Register<DummyPersistentObject>());
        }

        [Test]
        public void it_should_provide_a_graph_context_for_additional_configuration()
        {
            GraphContext<DummyBackingStore, DummyPersistentObject> actual = null;
            Sut.Register<DummyPersistentObject>(context => { actual = context; });
            actual.ShouldNotBeNull();
        }

        [Test]
        public void it_should_provide_a_graph_context_with_a_registered_graph()
        {
            GraphContext<DummyBackingStore, DummyPersistentObject> actual = null;
            Sut.Register<DummyPersistentObject>(context => { actual = context; });
            actual.RegisteredGraph.ShouldBeOfType(typeof(RegisteredGraph<DummyPersistentObject>));
        }

        [Test]
        public void it_should_provide_a_graph_context_with_a_registered_graph_having_an_assigned_aggregate_type()
        {
            GraphContext<DummyBackingStore, DummyPersistentObject> actual = null;
            Sut.Register<DummyPersistentObject>(context => { actual = context; });
            actual.RegisteredGraph.GraphType.ShouldEqual(typeof(DummyPersistentObject));
        }
    }
}