namespace Stash.JsonSerializer
{
    using System;
    using Configuration;
    using Engine;
    using Newtonsoft.Json;

    public class AggregateConverter<TGraph> : JsonConverter 
    {
        private readonly Func<Type, bool> isAggregate;
        private readonly ISerializationSession session;
        private bool handlingRoot;
        private readonly object rootLock = new object();

        public AggregateConverter(Func<Type,bool> isAggregate, ISerializationSession session)
        {
            this.isAggregate = isAggregate;
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
            writer.WritePropertyName("__StashInternalId");
            writer.WriteValue(internalId.ToString());
            writer.WriteEndObject();
        }

        private static void readAndAssertProperty(JsonReader reader, string propertyName)
        {
            readAndAssert(reader);

            if (reader.TokenType != JsonToken.PropertyName || reader.Value.ToString() != propertyName)
                throw new JsonSerializationException(string.Format("Expected JSON property '{0}'.", propertyName));
        }

        private static void readAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw new JsonSerializationException("Unexpected end.");
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            readAndAssertProperty(reader, "Key");
            readAndAssert(reader);
            var internalId = new InternalId(new Guid(reader.Value.ToString()));
            
            return session.TrackedGraphForInternalId(internalId);
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

            return isAggregate(objectType);
        }
    }
}