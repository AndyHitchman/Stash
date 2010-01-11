namespace Stash.Specifications
{
    using Selectors;

    public class DummyFrom : From<DummyFrom,object,DummyPersistentObject>
    {
        public DummyFrom(Projector<object, DummyPersistentObject> projector) : base(projector)
        {
        }
    }
}