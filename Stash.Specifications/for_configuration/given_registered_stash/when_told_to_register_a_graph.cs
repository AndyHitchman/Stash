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

namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using System;
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_told_to_register_a_graph : with_registered_stash
    {
        [Test]
        public void it_should_insist_that_the_graph_is_not_already_registered()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            typeof(ArgumentException)
                .ShouldBeThrownBy(
                    () =>
                    Sut.RegisterGraph<DummyPersistentObject>());
        }

        [Test]
        public void it_should_register_a_graph()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldHaveCount(1);
        }

        [Test]
        public void it_should_register_the_provided_distinct_graphs()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.RegisterGraph<OtherDummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<OtherDummyPersistentObject>));
        }

        [Test]
        public void it_should_register_the_provided_graph()
        {
            Sut.RegisterGraph<DummyPersistentObject>();
            Sut.AllRegisteredGraphs.ShouldContain(
                graph => graph.GetType() == typeof(RegisteredGraph<DummyPersistentObject>));
        }
    }
}