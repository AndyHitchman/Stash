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

namespace Stash.Azure.Specifications.integration
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

    public class NumberOfCommentsOnPost : IIndex<Post, int>
    {
        public IEnumerable<int> Yield(Post graph)
        {
            yield return graph.Comments.Count;
        }
    }
}