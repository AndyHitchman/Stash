namespace Stash.Specifications.for_engine.given_default_internal_session
{
    using Configuration;
    using Engine;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_told_to_dispose
    {
        [Test]
        public void it_should_tell_itself_to_end_the_session()
        {
            var sut = new StandInDefaultInternalSession();

            sut.Dispose();

            sut.EndWasCalled.ShouldBeTrue();
        }


        private class StandInDefaultInternalSession: DefaultInternalSession
        {
            public bool EndWasCalled;
            public StandInDefaultInternalSession() : base(null) {}

            public override void End()
            {
                EndWasCalled = true;
            }
        }
    }
}