namespace Stash.Specifications.for_engine.for_persistence_events.given_track
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Engine;
    using Engine.PersistenceEvents;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_initialising_track
    {
        [Test]
        public void it_should_calculate_a_hash_code_based_on_the_original_stream()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var graph = new DummyPersistentObject();
            var serializedGraph = new MemoryStream("this is a pretend serialised object graph".Select(_ => Convert.ToByte(_)).ToArray());
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, serializedGraph, mockSession);

            sut.OriginalHash.ShouldNotBeEmpty();
            Console.WriteLine(Convert.ToBase64String(sut.OriginalHash));
        }

        [Test]
        public void it_should_calculate_a_hash_code_on_an_empty_stream()
        {
            var mockSession = MockRepository.GenerateMock<InternalSession>();
            var graph = new DummyPersistentObject();
            var serializedGraph = new MemoryStream();
            var sut = new Track<DummyPersistentObject>(Guid.Empty, graph, serializedGraph, mockSession);

            sut.OriginalHash.ShouldNotBeEmpty();
            Console.WriteLine(Convert.ToBase64String(sut.OriginalHash));
        }
    }
}