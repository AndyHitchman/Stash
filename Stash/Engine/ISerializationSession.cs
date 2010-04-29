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
        InternalId InternalIdOfTrackedGraph(object graph);


        /// <summary>
        /// Get the graph by internal id. If the graph is not tracked, it is fetched from the 
        /// backing store and tracked.
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        object TrackedGraphForInternalId(InternalId internalId);

        bool GraphIsTracked(InternalId internalId);
        void RecordActiveDeserialization(InternalId internalId, object graph);
    }
}