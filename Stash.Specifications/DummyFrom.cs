namespace Stash.Specifications
{
    using Engine;
    using Selectors;

    public class DummyFrom : From<DummyFrom,object,DummyPersistentObject>
    {
        public DummyFrom(ProjectedIndex<object> projector) : base(projector)
        {
        }
    }
}