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

namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BerkeleyDB;
    using In.BDB;
    using In.BDB.BerkeleyConfigs;
    using Support;

    public abstract class with_temp_dir : Specification<BerkeleyBackingStore>
    {
        protected string TempDir;

        protected override void WithContext()
        {

            TempDir = Path.Combine(Path.GetTempPath(), "Stash" + Guid.NewGuid());
            Console.WriteLine("TempDir: " + TempDir);
            if (!Directory.Exists(TempDir)) Directory.CreateDirectory(TempDir);

            AutoMocker.Container.Configure(
                _ =>
                _.For<IBerkeleyBackingStoreEnvironment>()
                    .Use<BerkeleyBackingStoreEnvironment>()
                    .Ctor<string>("databaseDirectory").Is(TempDir)
                    .Ctor<DatabaseEnvironmentConfig>().Is(new DefaultDatabaseEnvironmentConfig())
                    .Ctor<ValueDatabaseConfig>().Is(new ValueDatabaseConfig())
                    .Ctor<ReverseIndexDatabaseConfig>().Is(new ReverseIndexDatabaseConfig())
                    .Ctor<IntIndexDatabaseConfig>().Is(new IntIndexDatabaseConfig())
                    .Ctor<TypeIndexDatabaseConfig>().Is(new TypeIndexDatabaseConfig())
                    .Ctor<ObjectIndexDatabaseConfig>().Is(new ObjectIndexDatabaseConfig())
                );
        }

        protected override void TidyUp()
        {
            Subject.Dispose();
            if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}