namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.IO;

    public interface PersistenceEventFactory
    {
        Track<TGraph> MakeTrack<TGraph>(Guid internalId, TGraph graph, Stream serializedGraph, InternalSession session);
        Endure<TGraph> MakeEndure<TGraph>(TGraph graph, InternalSession session);
        Destroy<TGraph> MakeDestroy<TGraph>(Guid internalId, TGraph graph, InternalSession session);
        Insert<TGraph> MakeInsert<TGraph>(Endure<TGraph> endure);
        Update<TGraph> MakeUpdate<TGraph>(Track<TGraph> track);
    }
}