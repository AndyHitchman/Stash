namespace Stash.In.BDB.BerkeleyQueries
{
    public enum QueryCost
    {
        SingleGet = 1,
        MultiGet = 2,
        ClosedRangeScan = 5,
        OpenRangeScan = 10,
        FullScan = 20,
    }
}