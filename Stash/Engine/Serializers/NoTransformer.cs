namespace Stash.Engine.Serializers
{
    public class NoTransformer<TGraph> : ITransformer<TGraph,TGraph>
    {
        public TGraph TransformDown(TGraph graph)
        {
            return graph;
        }

        public TGraph TransformUp(TGraph transform)
        {
            return transform;
        }
    }
}