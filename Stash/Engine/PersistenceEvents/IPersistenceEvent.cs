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

namespace Stash.Engine.PersistenceEvents
{
    using System;
    using BackingStore;

    /// <summary>
    /// An event that must be handled by the active <see cref="ISession"/>.
    /// </summary>
    public interface IPersistenceEvent
    {
        /// <summary>
        /// Get the untyped graph.
        /// </summary>
        object UntypedGraph { get; }

        /// <summary>
        /// The internal id of the graph affected by the persistence event
        /// </summary>
        Guid InternalId { get; }

        /// <summary>
        /// Complete all work for the persistence event.
        /// </summary>
        /// <param name="work"></param>
        void Complete(IStorageWork work);

        PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(IPersistenceEvent @event);
    }
}