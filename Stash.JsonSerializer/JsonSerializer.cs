namespace Stash.JsonSerializer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configuration;
    using Engine;
    using Newtonsoft.Json;
    using Serializers;
    using System.Linq;

    public class JsonSerializer<TGraph> : ISerializer<TGraph>
    {
        const int initialBufferSize = 1024 * 32;
        private readonly IRegisteredGraph<TGraph> registeredGraph;
        private readonly Func<Type, bool> isAggregate;

        public JsonSerializer(IRegisteredGraph<TGraph> registeredGraph)
        {
            this.registeredGraph = registeredGraph;
            isAggregate = objectType => registeredGraph.Registry.IsManagingGraphTypeOrAncestor(objectType);
        }

        public TGraph Deserialize(Stream serial, ISerializationSession session)
        {
            var jsonSerializer = createFreshSerializer(session);
            serial.Position = 0;

            using (var reader = new JsonTextReader(new StreamReader(serial)))
                return (TGraph)jsonSerializer.Deserialize(reader, registeredGraph.GraphType);
        }

        public Stream Serialize(TGraph graph, ISerializationSession session)
        {
            var jsonSerializer = createFreshSerializer(session);

            var ms = new PreservedMemoryStream();
            using (var streamWriter = new StreamWriter(ms))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(writer, graph);
                writer.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        private JsonSerializer createFreshSerializer(ISerializationSession session) 
        {
            var jsonSerializer = JsonSerializer.Create(
                new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Auto
                    });
            jsonSerializer.Converters.Insert(0, new AggregateConverter<TGraph>(isAggregate, session));
            return jsonSerializer;
        }
    }
}