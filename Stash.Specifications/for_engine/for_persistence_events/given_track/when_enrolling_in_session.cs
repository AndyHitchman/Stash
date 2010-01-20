namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System;
    using System.IO;
    using Configuration;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_enrolling_in_session
    {
        [Test]
        public void it_should_do_nothing_if_the_graph_is_already_tracked()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, new MemoryStream(), mockSession);

            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(true);

            sut.EnrollInSession();

            mockSession.AssertWasNotCalled(s => s.Enroll(null), o => o.IgnoreArguments());
        }

        [Test]
        public void it_should_enroll_if_the_graph_is_not_tracked()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockRegistry = MockRepository.GenerateMock<Registry>();
            var graph = new DummyPersistentObject();
            var sut = new StandInTrack<DummyPersistentObject>(Guid.Empty, graph, mockSession);

            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(false);

            sut.EnrollInSession();

            mockSession.AssertWasCalled(s => s.Enroll(sut));
        }

        [Test]
        public void it_should_find_out_if_the_graph_is_already_tracked()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var graph = new DummyPersistentObject();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, new MemoryStream(), mockSession);

            mockSession.Expect(s => s.GraphIsTracked(graph)).Return(true);

            sut.EnrollInSession();

            mockSession.VerifyAllExpectations();
        }


        private class StandInTrack<TGraph> : Track<TGraph>
        {
            public StandInTrack(Guid internalId, TGraph graph, InternalSession session)
                : base(internalId, graph, new MemoryStream(), session)
            {
            }

            public override void PrepareEnrollment()
            {
                //Do nothing
            }
        }
    }
}