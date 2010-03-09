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

namespace Stash.BackingStore
{
    using System;
    using System.Runtime.Serialization;
    using Configuration;

    [Serializable]
    public class GraphForKeyNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GraphForKeyNotFoundException() {}
        public GraphForKeyNotFoundException(string message) : base(message) {}
        public GraphForKeyNotFoundException(string message, Exception inner) : base(message, inner) {}

        public GraphForKeyNotFoundException(Guid internalId, Exception inner)
            : base(
                string.Format(
                    "Graph stored for internalId {0} is not found.",
                    internalId),
                inner) {}

        protected GraphForKeyNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}