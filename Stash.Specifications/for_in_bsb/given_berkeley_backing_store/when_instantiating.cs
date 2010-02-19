namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System.IO;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_instantiating : with_temp_dir
    {
        protected override void Given()
        {
        }

        protected override void When()
        {
            var create = Subject;
            Subject.Dispose();
        }

        [Then]
        public void it_should_create_the_directory_if_it_does_not_exist()
        {
            Directory.Exists(TempDir).ShouldBeTrue();
        }

        [Test]
        public void it_should_create_the_primary_database()
        {
            File.Exists(Path.Combine(TempDir, "data\\graphs.db")).ShouldBeTrue();
        }
    }
}