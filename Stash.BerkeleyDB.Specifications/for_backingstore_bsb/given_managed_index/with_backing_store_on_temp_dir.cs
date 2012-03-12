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

namespace Stash.BerkeleyDB.Specifications.for_backingstore_bsb.given_managed_index
{
    using System;
    using System.IO;
    using global::BerkeleyDB;
    using Stash.BerkeleyDB;
    using Stash.BerkeleyDB.BerkeleyConfigs;
    using Stash.Configuration;
    using Stash.Engine;
    using Support;

    public abstract class with_backing_store_on_temp_dir : AutoMockedSpecification<BerkeleyBackingStore>
    {
        protected string TempDir;
        protected IRegistry registry;

        protected override void WithContext()
        {
            TempDir = Path.Combine(Path.GetTempPath(), "Stash-" + Guid.NewGuid());
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

            registry = new Registry();
            var typeHierarchyIndexer = new RegisteredIndexer<Type, string>(new StashTypeHierarchy(), registry);
            registry.RegisteredIndexers.Add(typeHierarchyIndexer);
            Subject.EnsureIndex(typeHierarchyIndexer);

        }

        protected override void TidyUp()
        {
            Subject.Dispose();
            if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}