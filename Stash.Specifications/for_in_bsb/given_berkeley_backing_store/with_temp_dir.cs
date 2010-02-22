namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BerkeleyDB;
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
            Console.WriteLine("TempDir: "+ TempDir);
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