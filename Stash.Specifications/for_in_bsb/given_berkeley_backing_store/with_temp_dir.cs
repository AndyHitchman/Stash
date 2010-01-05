namespace Stash.Specifications.for_in_bsb.given_bdb_backing_store
{
    using System;
    using System.IO;
    using NUnit.Framework;

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
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}