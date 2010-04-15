#region License
// Copyright 2009, 2010 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

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