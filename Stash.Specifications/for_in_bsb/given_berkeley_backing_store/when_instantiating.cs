namespace Stash.Specifications.for_in_bsb.given_bdb_backing_store
{
    using System.IO;
    using In.BDB;
    using NUnit.Framework;

    [TestFixture]
    public class when_instantiating : with_temp_dir
    {
        [Test]
        public void it_should_create_the_directory_if_it_does_not_exist()
        {
            var sut = new BerkeleyBackingStore(TempDir);
            sut.Dispose();

            Directory.Exists(TempDir).ShouldBeTrue();
        }

        [Test]
        public void it_should_create_the_primary_database()
        {
            var sut = new BerkeleyBackingStore(TempDir);
            sut.Dispose();

            File.Exists(Path.Combine(TempDir, "data\\stash.db")).ShouldBeTrue();
        }
    }
}