namespace Stash.Specifications.for_engine.given_default_enlisted_repository
{
    using Engine;
    using Selectors;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_doing_work
    {
        [Test]
        public void it_should_delegate_all_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.All<DummyPersistentObject>();

            mockUnenlistedRepository.AssertWasCalled(repository => repository.All<DummyPersistentObject>(mockSession));
        }

        [Test]
        public void it_should_delegate_delete_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Delete(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Delete(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_get_tracker_for_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.GetTrackerFor(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.GetTrackerFor(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_fetch_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockSelector = MockRepository.GenerateMock<From<DummyFrom, DummyPersistentObject>>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.Fetch(mockSelector);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Fetch(mockSession, mockSelector));
        }

        [Test]
        public void it_should_delegate_fetch_many_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockSelector = MockRepository.GenerateMock<From<DummyFrom, object, DummyPersistentObject>>((Projector<object, DummyPersistentObject>)null);
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var expected = new[] {mockSelector};
            
            sut.Fetch(expected);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Fetch(mockSession, expected));
        }

        [Test]
        public void it_should_delegate_persist_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);
            var graph = new DummyPersistentObject();

            sut.Persist(graph);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.Persist(mockSession, graph));
        }

        [Test]
        public void it_should_delegate_reconnect_tracker_to_the_underlying_repository()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var mockUnenlistedRepository = MockRepository.GenerateMock<UnenlistedRepository>();
            var mockTracker = MockRepository.GenerateMock<Tracker>();
            var sut = new DefaultEnlistedRepository(mockSession, mockUnenlistedRepository);

            sut.ReconnectTracker(mockTracker);

            mockUnenlistedRepository.AssertWasCalled(repository => repository.ReconnectTracker(mockSession, mockTracker));
        }
    }
}