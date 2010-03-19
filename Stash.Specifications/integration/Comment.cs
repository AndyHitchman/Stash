namespace Stash.Specifications.integration
{
    using System;

    [Serializable]
    public class Comment
    {
        public string Author { get; set; }
        public string AuthorsEmail { get; set; }
        public DateTime CommentedAt { get; set; }
        public string Text { get; set; }
    }
}