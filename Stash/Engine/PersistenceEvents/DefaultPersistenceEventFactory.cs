namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.IO;

    public class DefaultPersistenceEventFactory : PersistenceEventFactory
    {
        public Track<TGraph> MakeTrack<TGraph>(Guid internalId, TGraph graph, Stream serializedGraph, InternalSession session)
        {
            return new Track<TGraph>(internalId, graph, serializedGraph, session);
        }

        public Endure<TGraph> MakeEndure<TGraph>(TGraph graph, InternalSession session)
        {
            return new Endure<TGraph>(graph, session);
        }

        public Destroy<TGraph> MakeDestroy<TGraph>(Guid internalId, TGraph graph, InternalSession session)
        {
            return new Destroy<TGraph>(internalId, graph, session);
        }

        public Insert<TGraph> MakeInsert<TGraph>(Endure<TGraph> endure)
        {
            return new Insert<TGraph>(endure);
        }

        public Update<TGraph> MakeUpdate<TGraph>(Track<TGraph> track)
        {
            return new Update<TGraph>(track);
        }
    }
}