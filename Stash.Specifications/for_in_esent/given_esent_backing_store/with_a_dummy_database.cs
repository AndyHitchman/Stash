namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using NUnit.Framework;

    public class with_a_dummy_database
    {
        protected ESENTBackingStore sut;

        [SetUp]
        public void each_up()
        {
            var tempDir = Path.Combine(Environment.CurrentDirectory, "with_a_dummy_database");
            var tempPath = Path.Combine(tempDir, "test.db");
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);

            sut = new ESENTBackingStore(tempPath);
        }

        [TearDown]
        public void each_down()
        {
            sut.CloseDatabase();
        }
    }
}