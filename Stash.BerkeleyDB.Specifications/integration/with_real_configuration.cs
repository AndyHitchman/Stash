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

namespace Stash.BerkeleyDB.Specifications.integration
{
    using Stash.BerkeleyDB;
    using Stash.Engine.Serializers.Binary;

    public abstract class with_real_configuration : with_temp_directory
    {
        protected override void TidyUp()
        {
            Kernel.Shutdown();
            base.TidyUp();
        }

        protected override void WithContext()
        {
            base.WithContext();
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                register =>
                    {
                        register.Graph<Post>(
                            _ =>
                            _.SerializeWith(new BinarySerializer<Post>()));
                        register.Index(new NumberOfCommentsOnPost());
                    }
                );
        }
    }
}