namespace Stash.Specifications.for_in_esent.given_esent_backing_store
{
    using System;
    using System.IO;
    using In.ESENT;
    using Microsoft.Isam.Esent.Interop;
    using NUnit.Framework;

    [TestFixture]
    public class when_opening : with_a_dummy_database
    {
        [Test]
        public void it_should_work_without_errors()
        {
            try
            {
                sut.OpenDatabase();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}