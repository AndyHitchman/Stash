namespace Stash.In.ESENT
{
    using System;
    using System.Globalization;
    using System.IO;
    using Microsoft.Isam.Esent.Interop;

    public static class SchemaManager
    {
        public static void CreateDatabase(Database database)
        {
            using(var session = new Session(database.Instance))
            {
                try
                {
                    JET_DBID dbid;
                    Api.JetCreateDatabase(session, database.Path, "", out dbid, CreateDatabaseGrbit.None);

                    try
                    {
                        createMainStashTable(session, dbid);
                        using(var transaction = new Transaction(session))
                        {
                            transaction.Commit(CommitTransactionGrbit.None);
                            Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);
                        }
                    }
                    catch(EsentErrorException)
                    {
                        Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);
                        throw;
                    }
                }
                catch(EsentErrorException)
                {
                    try
                    {
                        new FileInfo(database.Path).Directory.Delete(true);
                    }
// ReSharper disable EmptyGeneralCatchClause
                    catch
// ReSharper restore EmptyGeneralCatchClause
                    {
                    }
                    throw;
                }
                finally
                {
                    Api.JetDetachDatabase(session, database.Path);
                }
            }
        }

        private static void createMainStashTable(Session session, JET_DBID dbid)
        {
            JET_TABLEID table;
            JET_COLUMNID internalIdColumn;
            JET_COLUMNID versionColumn;
            JET_COLUMNID typeColumn;
            JET_COLUMNID graphColumn;
            const string internalId = "internalId";
            const string version = "version";
            const string type = "type";

            Api.JetCreateTable(session, dbid, "mainStash", 1, 100, out table);

            Api.JetAddColumn(
                session,
                table,
                internalId,
                new JET_COLUMNDEF
                    {coltyp = JET_coltyp.Binary, cp = JET_CP.None, cbMax = 16, grbit = ColumndefGrbit.ColumnNotNULL},
                null,
                0,
                out internalIdColumn);

            Api.JetAddColumn(
                session,
                table,
                version,
                new JET_COLUMNDEF {coltyp = JET_coltyp.Long, cp = JET_CP.None, grbit = ColumndefGrbit.ColumnVersion},
                null,
                0,
                out versionColumn);

            Api.JetAddColumn(
                session,
                table,
                type,
                new JET_COLUMNDEF
                    {
                        coltyp = JET_coltyp.LongText,
                        cp = JET_CP.Unicode,
                        grbit = ColumndefGrbit.ColumnTagged | ColumndefGrbit.ColumnMultiValued
                    },
                null,
                0,
                out typeColumn);

            Api.JetAddColumn(
                session,
                table,
                "graph",
                new JET_COLUMNDEF {coltyp = JET_coltyp.LongBinary, cp = JET_CP.None},
                null,
                0,
                out graphColumn);

            var internalIdIndex = String.Format(CultureInfo.InvariantCulture, "+{0}\0\0", internalId);
            Api.JetCreateIndex(
                session,
                table,
                "onInternalId",
                CreateIndexGrbit.IndexPrimary,
                internalIdIndex,
                internalIdIndex.Length,
                100);

            var typeIndex = String.Format(CultureInfo.InvariantCulture, "+{0}\0\0", type);
            Api.JetCreateIndex(
                session, table, "onType", CreateIndexGrbit.IndexDisallowNull, typeIndex, typeIndex.Length, 100);

            Api.JetCloseTable(session, table);
        }
    }
}