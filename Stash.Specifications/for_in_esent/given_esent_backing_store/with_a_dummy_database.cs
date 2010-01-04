namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using NUnit.Framework;

    public class with_a_dummy_database
    {
        protected ESENTBackingStore Sut;
        private string tempDir;

        [SetUp]
        public void each_up()
        {
            this.tempDir = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            var tempPath = Path.Combine(tempDir, "test.db");

            Sut = new ESENTBackingStore(tempPath, new DummyConnectionPool());
        }

        [TearDown]
        public void each_down()
        {
            Sut.Dispose();
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }
}