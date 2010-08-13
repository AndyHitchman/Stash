namespace Stash.BackingStore
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IndexNotReadyException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public IndexNotReadyException() {}
        public IndexNotReadyException(string message) : base(message) {}
        public IndexNotReadyException(string message, Exception inner) : base(message, inner) {}

        protected IndexNotReadyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}