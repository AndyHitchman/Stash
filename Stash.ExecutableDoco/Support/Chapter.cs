namespace Stash.ExecutableDoco.Support
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public abstract class Chapter
    {
        protected string TempDir;

        protected virtual void TidyUp()
        {
            if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }

        protected virtual void WithContext()
        {
            TempDir = Path.Combine(Path.GetTempPath(), "Stash-" + Guid.NewGuid());
            Console.WriteLine("TempDir: " + TempDir);
            if (!Directory.Exists(TempDir)) Directory.CreateDirectory(TempDir);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            WithContext();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TidyUp();
        }

    }
}