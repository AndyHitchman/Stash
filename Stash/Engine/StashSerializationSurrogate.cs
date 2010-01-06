namespace Stash.Engine
{
    using System;
    using System.Runtime.Serialization;

    public class StashSerializationSurrogate : ISerializationSurrogate
    {
        public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public virtual object SetObjectData(
            object obj,
            SerializationInfo info,
            StreamingContext context,
            ISurrogateSelector selector)
        {
            throw new NotImplementedException();
        }
    }
}