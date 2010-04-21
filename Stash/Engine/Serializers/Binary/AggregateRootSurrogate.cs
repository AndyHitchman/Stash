namespace Stash.Engine.Serializers.Binary
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class AggregateRootSurrogate : ISerializationSurrogate 
    {
        private readonly AggregateReferenceSurrogate referenceSurrogate;
        private object rootObject;
        private MemberInfo[] cachedMemberInfos;

        public AggregateRootSurrogate(AggregateReferenceSurrogate referenceSurrogate)
        {
            this.referenceSurrogate = referenceSurrogate;
        }

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            //We are making the assumption that the first call to this surrogate is for the top object (the root of the graph).
            //Seems sensible.
            if(rootObject == null)
            {
                rootObject = obj;
            }

            if(ReferenceEquals(obj, rootObject))
            {
                var data = FormatterServices.GetObjectData(obj, getSerializableMembers(obj, context));

                for(var i = 0; i < getSerializableMembers(obj, context).Length; i++)
                {
                    info.AddValue(getSerializableMembers(obj, context)[i].Name, data[i]);
                }

                //Put the internal id into the serialised data to allow us to provide a reference to an already tracked
                //object when we deserialise.
                var internalIdOfTrackedGraph = getSession(context).InternalIdOfTrackedGraph(obj);
                if(internalIdOfTrackedGraph != null)
                    info.AddValue(AggregateReferenceSurrogate.ReferenceInfoKey, internalIdOfTrackedGraph.Value);
            }
            else
            {
                referenceSurrogate.GetObjectData(obj, info, context);
            }
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var session = getSession(context);

            //Return a reference to a tracked object if available.
            var rawInternalId = info.GetValue(AggregateReferenceSurrogate.ReferenceInfoKey, typeof(Guid));

            if (rawInternalId == null)
                throw new InvalidOperationException("Expected SerializationInfo to contain an internal id for an aggregate");

            var internalId = (Guid)rawInternalId;

            if(session.GraphIsTracked(internalId)) 
                return session.TrackedGraphForInternalId(internalId);

            //To avoid circular references in separate deserialisation sessions, we track active deserialisations.
            session.RecordActiveDeserialization(internalId, obj);

            //If we are not tracking already, then deserialise.
            var data =
                getSerializableMembers(obj, context)
                    .Select(_ => info.GetValue(_.Name, ((FieldInfo)_).FieldType))
                    .ToArray();

            //Returns the passed in obj ref.
            return FormatterServices.PopulateObjectMembers(obj, getSerializableMembers(obj, context), data);
        }

        private static ISerializationSession getSession(StreamingContext context) 
        {
            var session = context.Context as ISerializationSession;
            if (session == null)
                throw new ArgumentException("context does not contain an instance of ISerializationSession");
            return session;
        }

        private MemberInfo[] getSerializableMembers(object obj, StreamingContext context)
        {
            return cachedMemberInfos ?? (cachedMemberInfos = FormatterServices.GetSerializableMembers(obj.GetType(), context));
        }
    }
}