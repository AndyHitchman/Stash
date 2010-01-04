namespace Stash.Specifications.for_in_bsb.given_bdb_backing_store
{
    using System;
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
    }

    public class with_temp_dir
    {
        protected string TempDir;

        [SetUp]public void each_up()
        {
            TempDir = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }

        [TearDown]public void each_down()
        {
//            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}