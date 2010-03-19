namespace Stash.Specifications.integration
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Post
    {
        public string Title { get; set; }
        public DateTime PostedAt { get; set; }
        public string Text { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class NumberOfCommentsOnPost : IIndex<Post,int>
    {
        public IEnumerable<int> Yield(Post graph)
        {
            yield return graph.Comments.Count;
        }
    }
}