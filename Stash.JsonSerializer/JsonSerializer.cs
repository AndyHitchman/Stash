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

        public JsonSerializer(IRegisteredGraph<TGraph> registeredGraph)
        {
            this.registeredGraph = registeredGraph;
        }

        public TGraph Deserialize(IEnumerable<byte> bytes, ISerializationSession session)
        {
            var jsonSerializer = createFreshSerializer(registeredGraph, session);

            using (var sr = new StreamReader(new MemoryStream(bytes.ToArray())))
            using (var reader = new JsonTextReader(sr))
                return (TGraph)jsonSerializer.Deserialize(reader, registeredGraph.GraphType);
        }

        public IEnumerable<byte> Serialize(TGraph graph, ISerializationSession session)
        {
            var jsonSerializer = createFreshSerializer(registeredGraph, session);

            using (var memoryStream = new MemoryStream(initialBufferSize))
            using (var sr = new StreamWriter(memoryStream))
            using (var writer = new JsonTextWriter(sr))
            {
                jsonSerializer.Serialize(writer, graph);
                return memoryStream.ToArray();
            }
        }

        private static JsonSerializer createFreshSerializer(IRegisteredGraph<TGraph> registeredGraph, ISerializationSession session) 
        {
            var jsonSerializer = JsonSerializer.Create(
                new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Auto
                    });
            jsonSerializer.Converters.Insert(0, new AggregateConverter<TGraph>(registeredGraph, session));
            return jsonSerializer;
        }
    }

    public class AggregateConverter<TGraph> : JsonConverter 
    {
        private readonly IRegisteredGraph<TGraph> registeredGraph;
        private readonly ISerializationSession session;
        private bool handlingRoot;
        private readonly object rootLock = new object();

        public AggregateConverter(IRegisteredGraph<TGraph> registeredGraph, ISerializationSession session)
        {
            this.registeredGraph = registeredGraph;
            this.session = session;
            handlingRoot = true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var internalId = session.InternalIdOfTrackedGraph(value);

            if (internalId == null)
                throw new InvalidOperationException(
                    string.Format("Graph of type {0} ({1}) is not tracked and therefore cannot be serialised as a reference", value.GetType(), value));

            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            writer.WriteValue(entityKeyMember.Key);
            writer.WritePropertyName("Type");
            writer.WriteValue((keyType != null) ? keyType.FullName : null);

            writer.WritePropertyName("Value");

            if (keyType != null)
            {
                string valueJson;
                if (JsonSerializerInternalWriter.TryConvertToString(entityKeyMember.Value, keyType, out valueJson))
                    writer.WriteValue(valueJson);
                else
                    writer.WriteValue(entityKeyMember.Value);
            }
            else
            {
                writer.WriteNull();
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
        }

        public override bool CanConvert(Type objectType)
        {
            if(handlingRoot)
                lock(rootLock)
                    if(handlingRoot)
                    {
                        handlingRoot = false;
                        return false;
                    }
            return registeredGraph.Registry.IsManagingGraphTypeOrAncestor(objectType);
        }
    }
}