namespace Stash.Specifications.for_configuration.given_configuration_engine
{
    using System;
    using Configuration;

    public class DummyBackingStore : BackingStore
    {
        public void OpenDatabase()
        {
            throw new NotImplementedException();
        }

        public void CloseDatabase()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}