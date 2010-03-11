#region License

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

namespace Stash.Specifications.for_backingstore_bsb
{
    using Configuration;
    using Support;

    public abstract class with_int_indexer : with_temp_dir
    {
        protected RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        protected RegisteredIndexer<ClassWithTwoAncestors, int> registeredIndexer;
        private Registry registry;

        protected override void WithContext()
        {
            base.WithContext();

            registry = new Registry();
            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>(registry);
            registeredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex());
            registry.RegisteredIndexers.Add(registeredIndexer);

            Subject.EnsureIndex(registeredIndexer);
        }
    }
}