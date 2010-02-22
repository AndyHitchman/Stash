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
    using In.BDB;
    using In.BDB.Configuration;
    using Rhino.Mocks;
    using Support;

    public abstract class with_temp_dir : Specification<BerkeleyBackingStore>
    {
        protected string TempDir;

        protected override void BaseContext()
        {
            base.BaseContext();

            TempDir = Path.Combine(Path.GetTempPath(), "Stash" + Guid.NewGuid());
            Console.WriteLine("TempDir: " + TempDir);
            if(!Directory.Exists(TempDir)) Directory.CreateDirectory(TempDir);

            var databaseConfigs = new Dictionary<Type, IndexDatabaseConfig>
                {
                    {typeof(int), new IntIndexDatabaseConfig()},
                    {typeof(Type), new TypeIndexDatabaseConfig()},
                    {typeof(object), new ObjectIndexDatabaseConfig()},
                };

            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.DatabaseDirectory).Return(TempDir);
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.DatabaseEnvironmentConfig).Return(new DefaultDatabaseEnvironmentConfig());
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.ValueDatabaseConfig).Return(new ValueDatabaseConfig());
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.IndexDatabaseConfigForTypes).Return(databaseConfigs);
        }

        protected override void BaseTidyUp()
        {
            base.BaseTidyUp();

            Subject.Dispose();
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}