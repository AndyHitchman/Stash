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
    using Engine;
    using Microsoft.WindowsAzure.StorageClient;
    using Stash.BackingStore;
    using Stash.Engine;
    using Support;

    public class when_deleting_a_tracked_graph : with_real_configuration
    {
        private string expectedTitle;

        protected override void Given()
        {
            var stashedPost = new Post
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

            var internalId = new InternalId(Guid.NewGuid());
            var registeredGraph = Kernel.Registry.GetRegistrationFor<Post>();
            var serializedGraph = registeredGraph.Serialize(stashedPost, null);
            var projectedIndexes = registeredGraph.IndexersOnGraph.SelectMany(_ => _.GetUntypedProjections(stashedPost));
            var tracked = new TrackedGraph(new StoredGraph(internalId, registeredGraph.GraphType, AccessCondition.None, serializedGraph), projectedIndexes, registeredGraph);

            Kernel.Registry.BackingStore.InTransactionDo(_ => _.InsertGraph(tracked));
        }

        protected override void When()
        {
            var deletingSession = Kernel.SessionFactory.GetSession();
            var stashedSet = deletingSession
                .GetStashOf<Post>()
                .Matching(_ => _.Where<NumberOfCommentsOnPost>().GreaterThanEqual(1));

            var postToDelete = stashedSet.FirstOrDefault();
            stashedSet.Destroy(postToDelete);

            deletingSession.Complete();
        }

        [Then]
        public void it_should_have_removed_the_post()
        {
            var querySession = Kernel.SessionFactory.GetSession();
            var deletedPost =
                querySession.GetStashOf<Post>()
                    .Matching(_ => _.Where<NumberOfCommentsOnPost>().GreaterThanEqual(1))
                    .FirstOrDefault();

            deletedPost.ShouldBeNull();
        }
    }
}