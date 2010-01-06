namespace Stash.Specifications.for_configuration.given_registered_stash
{
    using Configuration;
    using NUnit.Framework;

    public class with_registered_stash
    {
        protected RegisteredStash Sut { get; private set; }

        [SetUp]
        public void each_up()
        {
            Sut = new RegisteredStash();
        }
    }
}