namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System.IO;
    using In.BDB.Configuration;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_ensuring_an_index_that_does_not_exist : with_temp_dir
    {
        private string indexName;

        protected override void Given()
        {
            indexName = typeof(DummyIndex).FullName;
        }

        protected override void When()
        {
            Subject.EnsureIndex(indexName, typeof(int));
        }

        [Then]
        public void it_should_create_the_index_database()
        {
            File.Exists(TempDir + "\\data\\index-" + indexName + ".db").ShouldBeTrue();
        }

        [Then]
        public void it_configure_the_database_with_the_correct_comparer()
        {
            Subject.IndexDatabases[indexName].IndexDatabase.Compare.Method.DeclaringType.ShouldEqual(typeof(IntIndexDatabaseConfig));
        }
    }
}