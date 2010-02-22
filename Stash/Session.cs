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
    using Engine;

    public interface Session : IDisposable
    {
        /// <summary>
        /// Abandon the session. Throw away all work.
        /// </summary>
        void Abandon();

        /// <summary>
        /// Complete the session. Persist all work.
        /// </summary>
        void Complete();

        /// <summary>
        /// Tell the session to track the work performed with the given repository.
        /// </summary>
        /// <param name="unenlistedRepository"></param>
        EnlistedRepository EnlistRepository(UnenlistedRepository unenlistedRepository);

        /// <summary>
        /// Get the <see cref="InternalSession"/> used by Stash. Not for external use.
        /// </summary>
        /// <returns></returns>
        InternalSession Internalize();
    }
}