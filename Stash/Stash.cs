namespace Stash
{
    using Configuration;

    public class StashedSet
    {
        public StashedSet() : this(Kernel.Registry, Kernel.SessionFactory.GetSession()) { }

        public StashedSet(Registry registry, ISession session)
        {
            Registry = registry;
            Session = session;
        }

        public Registry Registry { get; private set; }
        public ISession Session { get; set; }

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