namespace Stash
{
    public interface SessionFactory
    {
        /// <summary>
        /// Get a session.
        /// </summary>
        /// <returns></returns>
        Session GetSession();
    }
}