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

        public TGraph Deserialize(IEnumerable<byte> bytes, ISerializationSession session)
        {
            var jsonSerializer = createFreshSerializer(session);

            using (var sr = new StreamReader(new MemoryStream(bytes.ToArray())))
            using (var reader = new JsonTextReader(sr))
                return (TGraph)jsonSerializer.Deserialize(reader, registeredGraph.GraphType);
        }

        public IEnumerable<byte> Serialize(TGraph graph, ISerializationSession session)
        {

            var tmp = JsonConvert.SerializeObject(graph, new AggregateConverter<TGraph>(isAggregate, session));
            return tmp.ToCharArray().Select(c => char.);
            //            var jsonSerializer = createFreshSerializer(session);
            //            
            //            using (var memoryStream = new MemoryStream(initialBufferSize))
            //            using (var streamWriter = new StreamWriter(memoryStream))
            //            using (var writer = new JsonTextWriter(streamWriter))
            //            {
            //                jsonSerializer.Serialize(writer, graph);
            //                return memoryStream.ToArray();
            //            }
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