#region License
// Copyright 2009 Andrew Hitchman
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

namespace BerkeleyDBFailingTest
{
    using System;
    using System.IO;
    using System.Linq;
    using BerkeleyDB;
    using NUnit.Framework;

    [TestFixture]
    [Ignore]
    public class minimal_test_set
    {
        private static DatabaseEnvironment makeEnv()
        {
            var home = Path.Combine(Path.GetTempPath(), "DeleteMe" + Guid.NewGuid());
            Console.WriteLine(home);
            Directory.CreateDirectory(home);

            var environmentConfig = new DatabaseEnvironmentConfig
                {
                    AutoCommit = false,
                    Create = true,
                    UseLogging = true,
                    UseTxns = true,
                    FreeThreaded = true,
                    RunRecovery = true,
                    UseMPool = true,
                    MPoolSystemCfg = new MPoolConfig {CacheSize = new CacheInfo(0, 128 * 1024, 1)},
                    UseLocking = true,
                    LockSystemCfg = new LockingConfig {DeadlockResolution = DeadlockPolicy.MIN_WRITE},
                };

            return DatabaseEnvironment.Open(home, environmentConfig);
        }

        private static HashDatabaseConfig getConfig(DatabaseEnvironment env)
        {
            return new HashDatabaseConfig
                {
                    Creation = CreatePolicy.IF_NEEDED,
                    FreeThreaded = true,
                    ReadUncommitted = true,
                    AutoCommit = true,
                    Env = env,
                };
        }

        [Test]
        public void i_fail_when_inserting_four_duplicates_with_a_hash_function_when_the_data_is_longer_than_40_bytes()
        {
            const int minLengthOfDataThatCausesFailure = 41;

            //Page = 8192 - 26 = 8166
            //Key = 16 + 6 = 22
            //Data = 41 + 6 = 47
            //Record = 69
            //Four records = 276

            var env = makeEnv();
            var cfg = getConfig(env);

            cfg.HashComparison = (dbt1, dbt2) => new Guid(dbt1.Data).CompareTo(new Guid(dbt2.Data));
            cfg.Duplicates = DuplicatesPolicy.UNSORTED;

            var db = HashDatabase.Open("db", cfg);
            var transaction = env.BeginTransaction();

            var data = Enumerable.Repeat((byte)'A', minLengthOfDataThatCausesFailure).ToArray();

            for(var i = 0; i < 1000; i++)
            {
                var key = Guid.NewGuid().ToByteArray();

                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
            }

            transaction.Commit();
            db.Close();
            env.Close();
        }

        [Test]
        public void i_fail_when_inserting_two_duplicates_with_a_hash_function_when_the_data_is_longer_than_276_bytes()
        {
            const int minLengthOfDataThatCausesFailure = 277;

            //Page = 8192 - 26 = 8166
            //Key = 16 + 6 = 22
            //Data = 277 + 6 = 283
            //Record = 305

            var env = makeEnv();
            var cfg = getConfig(env);

            cfg.HashComparison = (dbt1, dbt2) => new Guid(dbt1.Data).CompareTo(new Guid(dbt2.Data));
            cfg.Duplicates = DuplicatesPolicy.UNSORTED;

            var db = HashDatabase.Open("db", cfg);
            var transaction = env.BeginTransaction();

            var data = Enumerable.Repeat((byte)'A', minLengthOfDataThatCausesFailure).ToArray();

            for(var i = 0; i < 1000; i++)
            {
                var key = Guid.NewGuid().ToByteArray();

                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
            }

            transaction.Commit();
            db.Close();
            env.Close();
        }

        [Test]
        public void i_fail_when_inserting_two_duplicates_with_a_hash_function_when_the_data_is_longer_than_71_bytes()
        {
            const int minLengthOfDataThatCausesFailure = 72;

            //Page = 8192 - 26 = 8166
            //Key = 16 + 6 = 22
            //Data = 72 + 6 = 78
            //Record = 100

            var env = makeEnv();
            var cfg = getConfig(env);

            cfg.HashComparison = (dbt1, dbt2) => new Guid(dbt1.Data).CompareTo(new Guid(dbt2.Data));
            cfg.Duplicates = DuplicatesPolicy.UNSORTED;

            var db = HashDatabase.Open("db", cfg);
            var transaction = env.BeginTransaction();

            var data = Enumerable.Repeat((byte)'A', minLengthOfDataThatCausesFailure).ToArray();

            for(var i = 0; i < 1000; i++)
            {
                var key = Guid.NewGuid().ToByteArray();

                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
            }

            transaction.Commit();
            db.Close();
            env.Close();
        }

        [Test]
        public void i_work_when_inserting_two_duplicates_with_a_hash_function_when_the_data_is_longer_than_248_bytes_and_less_than_277_bytes()
        {
            const int minLengthOfDataThatCausesFailure = 249;

            //Page = 8192 - 26 = 8166
            //Key = 16 + 6 = 22
            //Data = 249 + 6 = 255
            //Record = 277

            var env = makeEnv();
            var cfg = getConfig(env);

            cfg.HashComparison = (dbt1, dbt2) => new Guid(dbt1.Data).CompareTo(new Guid(dbt2.Data));
            cfg.Duplicates = DuplicatesPolicy.UNSORTED;

            var db = HashDatabase.Open("db", cfg);
            var transaction = env.BeginTransaction();

            var data = Enumerable.Repeat((byte)'A', minLengthOfDataThatCausesFailure).ToArray();

            for(var i = 0; i < 1000; i++)
            {
                var key = Guid.NewGuid().ToByteArray();

                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
                db.Put(new DatabaseEntry(key), new DatabaseEntry(data), transaction);
            }

            transaction.Commit();
            db.Close();
            env.Close();
        }
    }
}