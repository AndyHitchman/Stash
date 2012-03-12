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

namespace Stash.BerkeleyDB.Specifications.Support
{
    using StructureMap.AutoMocking;

    /// <summary>
    /// Thanks to GShutler and the OpenRasta.Wiki sample code for this BDD-style specification base class. No license specified.
    /// </summary>
    public abstract class AutoMockedSpecification<TSut> : Specification where TSut : class
    {
        protected override void BaseContext()
        {
            AutoMocker = new RhinoAutoMocker<TSut>(MockMode.AAA);
        }

        protected override void BaseTidyUp() {}

        protected T Dependency<T>() where T : class
        {
            return AutoMocker.Get<T>();
        }

        protected TSut Subject
        {
            get { return AutoMocker.ClassUnderTest; }
        }

        protected RhinoAutoMocker<TSut> AutoMocker { get; set; }
    }
}