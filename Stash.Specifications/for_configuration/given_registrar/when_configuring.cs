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

namespace Stash.Specifications.for_configuration.given_registrar
{
    using Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_configuring
    {
        [Test]
        public void it_should_provide_a_configuration_context_to_scope_configuration()
        {
            PersistenceContext<DummyBackingStore> actual = null;
            var sut = new Registrar<DummyBackingStore>(new DummyBackingStore());
            sut.PerformRegistration(context => { actual = context; });
            actual.ShouldNotBeNull();
        }
    }
}