namespace Stash.Serializers
{
    using System.IO;

    public class PreservedMemoryStream : MemoryStream
    {
        public PreservedMemoryStream() {}

        public PreservedMemoryStream(int capacity) : base(capacity) {}

        public PreservedMemoryStream(byte[] buffer) : base(buffer) {}

        /// <summary>
        /// Instead of closing and disposing of resources
        /// keep the stream open and reset the position to the start.
        /// </summary>
        public override void Close()
        {
            // Do NOT close.
            // Reset stream.
            Position = 0;
        }
    }
}