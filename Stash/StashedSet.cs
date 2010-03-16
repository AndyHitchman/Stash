namespace Stash
{
    using Configuration;

    public static class StashedSet
    {
        public static StashedSet<TGraph> Get<TGraph>()
        {
            return new StashedSet<TGraph>(Kernel.Registry, Kernel.SessionFactory.GetSession());
        }


        public static StashedSet<object> Get()
        {
            return new StashedSet<object>(Kernel.Registry, Kernel.SessionFactory.GetSession());
        }
    }
}