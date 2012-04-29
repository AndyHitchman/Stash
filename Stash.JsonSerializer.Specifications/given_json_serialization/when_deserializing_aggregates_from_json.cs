namespace Stash.JsonSerializer.Specifications.given_json_serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BackingStore;
    using Configuration;
    using Engine;
    using Newtonsoft.Json;
    using NSubstitute;
    using NSubstitute.Core;
    using Serializers;
    using Support;
    using System.Linq;

    public class when_deserializing_aggregates_from_json : Specification
    {
        private JsonSerializer<ComposingClass> subject;
        private ISerializationSession session;
        private ComposingClass actual;
        private InternalId expectedInternalId;
        private string jsonSource;
        private PreservedMemoryStream serialised;
        private IsolatedClass expected;

        protected override void Given()
        {
            session = Substitute.For<ISerializationSession>();
            var backingStore = Substitute.For<IBackingStore>();
            var registry = new Registry(backingStore);
            registry.RegisterGraph<IsolatedClass>();
            registry.RegisterGraph<ComposingClass>();
            subject = new JsonSerializer<ComposingClass>((IRegisteredGraph<ComposingClass>)registry.RegisteredGraphs[typeof(ComposingClass)]);

            expectedInternalId = new InternalId(Guid.NewGuid());

            jsonSource = @"{""Composed"":{""__StashInternalId"":""" + expectedInternalId + @"""},""AnotherProperty"":""another property""}";
            serialised = new PreservedMemoryStream();
            using(var sw = new StreamWriter(serialised))
                sw.Write(jsonSource);

            expected = new IsolatedClass();

            session.TrackedGraphForInternalId(expectedInternalId).Returns(expected);
        }

        protected override void When()
        {
            actual = subject.Deserialize(serialised, session);
        }

        [Then]
        public void it_should_deserialise_and_load_referenced_internal_id()
        {
            actual.Composed.ShouldEqual(expected);
        }

        [Then]
        public void the_session_was_asked_to_provide_the_object_referenced()
        {
            session.Received().TrackedGraphForInternalId(expectedInternalId);
        }
    }
}