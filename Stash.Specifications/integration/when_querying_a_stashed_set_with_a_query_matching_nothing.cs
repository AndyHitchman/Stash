﻿#region License
// Copyright 2009 Andrew Hitchman
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

namespace Stash.Specifications.integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using Queries;
    using Support;

    public class when_querying_a_stashed_set_with_a_query_matching_nothing : with_real_configuration
    {
        private int actual;
        private Post stashedPost;

        protected override void Given()
        {
            stashedPost = new Post
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

            var internalId = Guid.NewGuid();
            var registeredGraph = Kernel.Registry.GetRegistrationFor<Post>();
            var serializedGraph = registeredGraph.Serialize(stashedPost);
            var projectedIndexes = registeredGraph.IndexersOnGraph.SelectMany(_ => _.GetUntypedProjections(stashedPost));
            var tracked = new TrackedGraph(internalId, serializedGraph, projectedIndexes, registeredGraph);

            Kernel.Registry.BackingStore.InTransactionDo(_ => _.InsertGraph(tracked));
        }

        protected override void When()
        {
            actual =
                new StashedSet<Post>(Kernel.SessionFactory.GetSession())
                    .Where(Index<NumberOfCommentsOnPost>.GreaterThan(1))
                    .Count();
        }

        [Then]
        public void it_should_not_find_my_post_with_a_comment()
        {
            actual.ShouldEqual(0);
        }
    }
}