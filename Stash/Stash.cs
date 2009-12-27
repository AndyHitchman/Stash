namespace Stash
{
    using System;
    using Configuration;

    public static class Stash
    {
        public static void Configure(Action<ConfigurationContext> configurationAction)
        {
            new ConfigurationEngine().Configure(configurationAction);
        }
    }
}