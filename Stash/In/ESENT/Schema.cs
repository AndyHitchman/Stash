namespace Stash.In.ESENT
{
    using Microsoft.Isam.Esent.Interop;

    public class Schema
    {
        public static string StashTableName { get { return "mainStash"; } }
        public JET_TABLEID StashTableId { get; set; }
        
        public static string InternalIdColumnName { get { return "internalId"; } }
        public JET_COLUMNID InternalIdColumnId { get; set; }

        public static string VersionColumnName { get { return "version"; } }
        public JET_COLUMNID VersionColumnId { get; set; }
        
        public static string TypeColumnName { get { return "type"; } }
        public JET_COLUMNID TypeColumnId { get; set; }
        
        public static string GraphColumnName { get { return "graph"; } }
        public JET_COLUMNID GraphColumnId { get; set; }
    }
}