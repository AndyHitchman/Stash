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

namespace Stash.Engine.PersistenceEvents
{
    using System;
    using System.IO;

    public class Endure<TGraph> : Track<TGraph>
    {
        /// <summary>
        /// We are persisting a new graph here, so create a new <see cref="Guid"/>.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="session"></param>
        public Endure(TGraph graph, IInternalSession session)
            : base(Guid.NewGuid(), graph, new MemoryStream(), session) {}


        public override void Complete()
        {
            Session.PersistenceEventFactory.MakeInsert(this).EnrollInSession();
        }
    }
}