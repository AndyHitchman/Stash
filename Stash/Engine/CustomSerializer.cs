namespace Stash.Engine
{
    using System;
    using System.IO;

    /// <summary>
    /// An interface that conveniently groups serialization and deserialization functions.
    /// Implement to provide customised behaviour or alternative serialization strategies.
    /// </summary>
    public interface CustomSerializer
    {
        Func<Stream, TGraph> Deserialize<TGraph>();
        Func<TGraph, Stream> Serialize<TGraph>();
    }
}