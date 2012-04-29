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
    using Support;
    using System.Linq;

    public class when_serializing_aggregates_to_json : Specification
    {
        private JsonSerializer<ComposingClass> subject;
        private IsolatedClass isolatedGraph;
        private ISerializationSession session;
        private Stream actual;
        private ComposingClass composingGraph;
        private InternalId expectedInternalId;

        protected override void Given()
        {
            session = Substitute.For<ISerializationSession>();
            var backingStore = Substitute.For<IBackingStore>();
            var registry = new Registry(backingStore);
            registry.RegisterGraph<IsolatedClass>();
            registry.RegisterGraph<ComposingClass>();
            subject = new JsonSerializer<ComposingClass>((IRegisteredGraph<ComposingClass>)registry.RegisteredGraphs[typeof(ComposingClass)]);

            isolatedGraph = new IsolatedClass {AProperty = "test property"};
            composingGraph = new ComposingClass {AnotherProperty = "another property", Composed = isolatedGraph};
            expectedInternalId = new InternalId(Guid.NewGuid());

            session.InternalIdOfTrackedGraph(isolatedGraph).Returns(expectedInternalId);
        }

        protected override void When()
        {
            actual = subject.Serialize(composingGraph, session);
        }

        [Then]
        public void it_should_create_a_json_document_referencing_the_registered_graph_by_internal_id()
        {
            var doc = new StreamReader(actual).ReadToEnd();
            doc.ShouldEqual(@"{""Composed"":{""__StashInternalId"":""" + expectedInternalId + @"""},""AnotherComposed"":null,""AnotherProperty"":""another property""}");
        }
    }
}