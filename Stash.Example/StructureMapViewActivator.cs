namespace Stash.Example
{
    using System;
    using Spark;
    using StructureMap;

    public class StructureMapViewActivator : IViewActivatorFactory
    {
        public IViewActivator Register(Type type)
        {
            ObjectFactory.Configure(c => c.For(type).Use(type));
            return new Activator();
        }

        public void Unregister(Type type, IViewActivator activator)
        {
        }


        private class Activator : IViewActivator
        {
            public ISparkView Activate(Type type)
            {
                return (ISparkView)ObjectFactory.GetInstance(type);
            }

            public void Release(Type type, ISparkView view)
            {
            }
        }
    }
}