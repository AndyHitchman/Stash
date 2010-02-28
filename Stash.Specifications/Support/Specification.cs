namespace Stash.Specifications.Support
{
    using NUnit.Framework;

    public abstract class Specification
    {
        protected virtual void BaseContext() {}

        protected virtual void BaseTidyUp() {}
        protected abstract void Given();

        [TestFixtureSetUp]
        public void Setup()
        {
            BaseContext();
            WithContext();
            Given();
            When();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TidyUp();
            BaseTidyUp();
        }

        protected virtual void TidyUp() {}
        protected abstract void When();
        protected virtual void WithContext() {}
    }
}