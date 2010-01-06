namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using Microsoft.Isam.Esent.Interop;
    using NUnit.Framework;

    [TestFixture][Ignore]
    public class when_creating_a_new_database : with_a_dummy_database
    {
        [Test]
        public void it_should_set_the_table_id_in_the_database_schema()
        {
            Sut.Database.Schema.StashTableId.ShouldNotBeNull();
        }

        [Test]
        public void it_should_set_the_internal_id_column_id_in_the_database_schema()
        {
            Sut.Database.Schema.InternalIdColumnId.ShouldNotBeNull();
        }

        [Test]
        public void it_should_set_the_type_column_id_in_the_database_schema()
        {
            Sut.Database.Schema.TypeColumnId.ShouldNotBeNull();
        }

        [Test]
        public void it_should_set_the_version_column_id_in_the_database_schema()
        {
            Sut.Database.Schema.VersionColumnId.ShouldNotBeNull();
        }

        [Test]
        public void it_should_set_the_graph_column_id_in_the_database_schema()
        {
            Sut.Database.Schema.GraphColumnId.ShouldNotBeNull();
        }
    }
}