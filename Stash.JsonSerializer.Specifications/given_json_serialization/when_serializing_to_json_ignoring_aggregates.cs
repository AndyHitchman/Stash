namespace Stash.JsonSerializer.Specifications.given_json_serialization
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Engine;
    using Newtonsoft.Json;
    using NSubstitute;
    using Support;
    using System.Linq;

    public class when_serializing_to_json_ignoring_aggregates : Specification
    {
        private JsonSerializer<IsolatedClass> subject;
        private IsolatedClass graph;
        private ISerializationSession session;
        private IEnumerable<byte> actual;

        protected override void Given()
        {
            var registry = new Registry(null);
            var registeredGraph = new RegisteredGraph<IsolatedClass>(registry);
            subject = new JsonSerializer<IsolatedClass>(registeredGraph);

            graph = new IsolatedClass {AProperty = "test property"};
            session = Substitute.For<ISerializationSession>();
        }

        protected override void When()
        {
            actual = subject.Serialize(graph, session);
            var test = JsonConvert.SerializeObject(graph);
        }

        [Then]
        public void it_should_create_a_json_document()
        {
            var doc = new string(actual.Cast<char>().ToArray());
            doc.ShouldEqual(@"{""AProperty"":""test property""}");
        }
    }
}