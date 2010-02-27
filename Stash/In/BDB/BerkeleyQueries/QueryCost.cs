namespace Stash.In.BDB.BerkeleyQueries
{
    public enum QueryCost
    {
        SingleGet = 1,
        MultiGet = 10,
        RangeScan = 50,
        FullScan = 100,
    }
}