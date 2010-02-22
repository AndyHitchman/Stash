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

namespace Stash.Specifications.Support
{
    using NUnit.Framework;
    using StructureMap.AutoMocking;

    /// <summary>
    /// Thanks to GShutler and the OpenRasta.Wiki sample code for this BDD-style specification base class. No license specified.
    /// </summary>
    [TestFixture]
    public abstract class Specification<TSut> where TSut : class
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            BaseContext();
            WithContext();
            Given();
            When();
        }

        protected virtual void BaseContext()
        {
            AutoMocker = new RhinoAutoMocker<TSut>(MockMode.AAA);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TidyUp();
            BaseTidyUp();
        }

        protected virtual void BaseTidyUp() {}

        protected virtual void WithContext() {}
        protected abstract void Given();
        protected abstract void When();
        protected virtual void TidyUp() {}

        protected T Dependency<T>() where T : class
        {
            return AutoMocker.Get<T>();
        }

        protected TSut Subject
        {
            get { return AutoMocker.ClassUnderTest; }
        }

        protected RhinoAutoMocker<TSut> AutoMocker { get; set; }
    }
}