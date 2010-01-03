namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using NUnit.Framework;

    public class with_a_dummy_database
    {
        protected ESENTBackingStore Sut;

        [SetUp]
        public void each_up()
        {
            var tempDir = Path.Combine(Environment.CurrentDirectory, "with_a_dummy_database");
            var tempPath = Path.Combine(tempDir, "test.db");
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);

            Sut = new ESENTBackingStore(tempPath, new DummyConnectionPool());
        }

        [TearDown]
        public void each_down()
        {
            Sut.Dispose();
        }
    }
}