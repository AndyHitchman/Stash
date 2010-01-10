namespace Stash
{
    using System.Collections.Generic;
    using Selectors;

    public interface Repository
    {
        /// <summary>
        /// Instruct the repository to delete the graph from the persistent store.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Delete<TGraph>(TGraph graph);

        /// <summary>
        /// Instruct the repository to durably persist the <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TGraph"></typeparam>
        /// <param name="graph"></param>
        void Persist<TGraph>(TGraph graph);

        /// <summary>
        /// Fetch from the projection.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        IEnumerable<Projection<TKey, TProjection>> Fetch<TKey, TProjection>(From<TKey, TProjection> @from);

        /// <summary>
        /// Fetch from the set of projections.
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        IEnumerable<TProjection> Fetch<TProjection>(params From<TProjection>[] @from);
    }
}