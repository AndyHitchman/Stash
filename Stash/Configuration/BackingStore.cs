namespace Stash.Configuration
{
    public interface BackingStore
    {
        void OpenDatabase();
        void CloseDatabase();
    }
}