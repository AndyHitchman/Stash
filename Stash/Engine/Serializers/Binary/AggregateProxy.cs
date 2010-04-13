namespace Stash.Engine.Serializers.Binary
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AggregateProxy : ISerializable, IObjectReference
    {
        public AggregateProxy(SerializationInfo info, StreamingContext context)
        {
            internalId = new Guid(info.GetString("InternalId"));
        }

        private readonly Guid internalId;

        public object GetRealObject(StreamingContext context)
        {
            var session = context.Context as IInternalSession;
            if (session == null)
                throw new ArgumentException("context does not contain an instance of IInternalSession");

            return session.TrackedGraphForInternalId(internalId);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}