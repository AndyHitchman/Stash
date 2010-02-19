namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    [Ignore]
    public class when_instantiating
    {
        [Test]
        public void it_should_create_an_esent_database()
        {
            var tempDir = Path.Combine(Environment.CurrentDirectory, "it_should_create_an_esent_database");
            var tempPath = Path.Combine(tempDir, "test.db");
            if(Directory.Exists(tempDir)) Directory.Delete(tempDir, true);

            var sut = new ESENTBackingStore(tempPath, new DummyConnectionPool());
            sut.Dispose();

            File.Exists(tempPath).ShouldBeTrue();
        }

        [Test]
        public void it_should_create_the_directory_if_it_does_not_exist()
        {
            var tempDir = Path.Combine(Environment.CurrentDirectory, "it_should_create_the_directory_if_it_does_not_exist");
            var tempPath = Path.Combine(tempDir, "test.db");
            if(Directory.Exists(tempDir)) Directory.Delete(tempDir, true);

            var sut = new ESENTBackingStore(tempPath, new DummyConnectionPool());
            sut.Dispose();

            Directory.Exists(tempDir).ShouldBeTrue();
        }
    }
}