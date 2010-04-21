namespace Stash.Engine
{
    using System;

    public interface ISerializationSession
    {
        /// <summary>
        /// Get the internal id of a graph, if it is tracked.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        Guid? InternalIdOfTrackedGraph(object graph);


        /// <summary>
        /// Get the graph by internal id. If the graph is not tracked, it is fetched from the 
        /// backing store and tracked.
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        object TrackedGraphForInternalId(Guid internalId);

        bool GraphIsTracked(Guid internalId);
        void RecordActiveDeserialization(Guid internalId, object graph);
    }
}