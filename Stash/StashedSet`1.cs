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

namespace Stash
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Configuration;
    using Queries;

    public class StashedSet<TGraph> : IEnumerable<TGraph>
    {
        public StashedSet() : this(Kernel.Registry, Kernel.SessionFactory.GetSession()) {}

        public StashedSet(Registry registry, ISession session)
        {
            Registry = registry;
            Session = session;
        }

        public Registry Registry { get; private set; }
        public ISession Session { get; set; }

        public void Where(IQuery query)
        {
        }

        public IEnumerator<TGraph> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}