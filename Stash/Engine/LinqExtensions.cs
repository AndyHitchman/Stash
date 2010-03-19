namespace Stash.Engine
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static IEnumerable<T> Materialize<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }
    }
}