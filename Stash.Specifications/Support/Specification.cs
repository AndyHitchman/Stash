using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using StructureMap.AutoMocking;

namespace Stash.Specifications.Support
{
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

        protected virtual void BaseTidyUp()
        {
        }

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