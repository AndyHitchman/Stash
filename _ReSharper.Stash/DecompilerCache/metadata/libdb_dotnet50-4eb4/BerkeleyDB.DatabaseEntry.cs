// Type: BerkeleyDB.DatabaseEntry
// Assembly: libdb_dotnet50, Version=5.0.21.0, Culture=neutral
// Assembly location: C:\Projects\Stash\lib\BerkeleyDB\libdb_dotnet50.dll

using System;

namespace BerkeleyDB
{
    public class DatabaseEntry : IDisposable
    {
        public DatabaseEntry();
        public DatabaseEntry(byte[] data);
        public byte[] Data { get; set; }
        public virtual void Dispose();
    }
}
