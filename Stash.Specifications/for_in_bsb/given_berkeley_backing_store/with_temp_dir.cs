namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.IO;
    using In.BDB;
    using Rhino.Mocks;
    using Support;

    public abstract class with_temp_dir : Specification<BerkeleyBackingStore>
    {
        protected string TempDir;

        protected override void BaseContext()
        {
            base.BaseContext();

            TempDir = Path.Combine(Path.GetTempPath(), "Stash" + Guid.NewGuid());
            if(!Directory.Exists(TempDir)) Directory.CreateDirectory(TempDir);

            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.DatabaseDirectory).Return(TempDir);
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.DatabaseEnvironmentConfig).Return(new DefaultDatabaseEnvironmentConfig());
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.ValueDatabaseConfig).Return(new ValueDatabaseConfig());
            AutoMocker.Get<IBerkeleyBackingStoreParams>().Stub(_ => _.IndexDatabaseConfig).Return(new IndexDatabaseConfig());
        }

        protected override void BaseTidyUp()
        {
            base.BaseTidyUp();

            Subject.Dispose();
            if(Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
        }
    }
}