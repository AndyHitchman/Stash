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

    public class when_serializing_more_complex_aggregates_to_json : Specification
    {
        private JsonSerializer<ComposingClass> subject;
        private ISerializationSession session;
        private Stream actual;
        private ComposingClass composingGraph;
        private InternalId expectedIsolatedInternalId;
        private InternalId expectedInnerInternalId;

        protected override void Given()
        {
            session = Substitute.For<ISerializationSession>();
            var backingStore = Substitute.For<IBackingStore>();
            var registry = new Registry(backingStore);
            registry.RegisterGraph<IsolatedClass>();
            registry.RegisterGraph<ComposingClass>();
            subject = new JsonSerializer<ComposingClass>((IRegisteredGraph<ComposingClass>)registry.RegisteredGraphs[typeof(ComposingClass)]);

            var isolatedGraph = new IsolatedClass {AProperty = "test property"};
            var innerGraph = new ComposingClass {AnotherProperty = "test"};
            composingGraph = new ComposingClass
                {
                    AnotherProperty = "another property", 
                    Composed = isolatedGraph,
                    AnotherComposed = innerGraph
                };
            expectedIsolatedInternalId = new InternalId(Guid.NewGuid());
            expectedInnerInternalId = new InternalId(Guid.NewGuid());

            session.InternalIdOfTrackedGraph(isolatedGraph).Returns(expectedIsolatedInternalId);
            session.InternalIdOfTrackedGraph(innerGraph).Returns(expectedInnerInternalId);
        }

        protected override void When()
        {
            actual = subject.Serialize(composingGraph, session);
        }

        [Then]
        public void it_should_create_a_json_document_referencing_the_registered_graphs_by_internal_id()
        {
            var doc = new StreamReader(actual).ReadToEnd();
            doc.ShouldEqual(@"{""Composed"":{""__StashInternalId"":""" + expectedIsolatedInternalId + @"""},""AnotherComposed"":{""__StashInternalId"":""" + expectedInnerInternalId + @"""},""AnotherProperty"":""another property""}");
        }
    }
}