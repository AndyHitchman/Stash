namespace Stash.Engine.Serializers
{
    public interface ITransformer<TGraph,TTransform>
    {
        TTransform TransformDown(TGraph graph);
        TGraph TransformUp(TTransform transform);
    }
}