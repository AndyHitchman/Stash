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

namespace Stash.Azure.Specifications.given_queries
{
    using Configuration;
    using JsonSerializer;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using Support;

    public abstract class with_string_indexer : with_development_storage
    {
        protected RegisteredGraph<ClassWithTwoAncestors> RegisteredGraph;
        protected RegisteredIndexer<ClassWithTwoAncestors, string> RegisteredIndexer;

        protected override void WithContext()
        {
            base.WithContext();

            CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().DeleteTableIfExist("idxStashAzureSpecificationsSupportStringIndex");
            CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient().DeleteTableIfExist("rdxStashAzureSpecificationsSupportStringIndex");

            RegisteredGraph = new RegisteredGraph<ClassWithTwoAncestors>(Registry);
            RegisteredGraph.TransformSerializer = new JsonSerializer<ClassWithTwoAncestors>(RegisteredGraph);
            RegisteredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, string>(new StringIndex(), Registry);
            Registry.RegisteredIndexers.Add(RegisteredIndexer);
            Subject.EnsureIndex(RegisteredIndexer);
        }
    }
}