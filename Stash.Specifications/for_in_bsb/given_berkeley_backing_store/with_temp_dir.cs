namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.IO;
    using NUnit.Framework;

    public class with_temp_dir
    {
        protected string TempDir;

        [TearDown]
        public void each_down()
        {
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }

        [SetUp]
        public void each_up()
        {
            TempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}