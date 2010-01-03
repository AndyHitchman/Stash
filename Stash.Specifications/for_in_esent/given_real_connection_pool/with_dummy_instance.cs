namespace Stash.Specifications.for_in_esent.given_real_connection_pool
{
    using System;
    using System.IO;
    using Microsoft.Isam.Esent.Interop;
    using NUnit.Framework;

    public class with_dummy_instance
    {
        protected Instance Instance;

        [SetUp]
        public void each_up()
        {
            var tempDir = Path.Combine(Environment.CurrentDirectory, "with_a_dummy_instance");
            var tempPath = Path.Combine(tempDir, "test.db");
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);

            Instance = new Instance(Guid.NewGuid().ToString());
            Instance.Init();
        }

        [TearDown]
        public void each_down()
        {
            Instance.Term();
        }
    }
}