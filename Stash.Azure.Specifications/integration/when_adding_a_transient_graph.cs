﻿#region License
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
    using System.Linq;
    using Support;

    public class when_adding_a_transient_graph : with_real_configuration
    {
        private Post persistedPost;
        private Post transientPost;

        protected override void Given()
        {
            transientPost = new Post
                {
                    Title = "My Super Blog",
                    PostedAt = new DateTime(2010, 03, 19, 13, 33, 34),
                    Text = "This is my super blog. Check out my awesome post",
                    Comments = new List<Comment>
                        {
                            new Comment
                                {
                                    Author = "Andy Hitchman",
                                    AuthorsEmail = "noone@nowhere.com",
                                    CommentedAt = new DateTime(2010, 03, 19, 13, 36, 01),
                                    Text = "This blog is teh suck"
                                }
                        }
                };
        }

        protected override void When()
        {
            var addingSession = Kernel.SessionFactory.GetSession();
            addingSession.GetStashOf<Post>().Endure(transientPost);
            addingSession.Complete();

            var querySession = Kernel.SessionFactory.GetSession();
            persistedPost = querySession.GetStashOf<Post>().FirstOrDefault();
        }

        [Then]
        public void it_should_persist_my_post()
        {
            persistedPost.ShouldNotBeNull();
        }

        [Then]
        public void it_should_stash_the_correct_title()
        {
            persistedPost.Title.ShouldEqual(transientPost.Title);
        }
    }
}