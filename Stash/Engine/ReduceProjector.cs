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

namespace Stash.Engine
{
    using System;

    public class ReduceProjector
    {
        /// <summary>
        /// Requires a function that accepts a <see cref="Projection{TKey,TValue}"> aggregate and <see cref="Projection{TKey,TValue}"> 
        /// instance and returns an accumulated aggregate <see cref="Projection{TKey,TValue}">projections</see> of the same shape.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance"></param>
        /// <param name="func"></param>
        /// <param name="originalId"></param>
        /// <param name="key"></param>
        /// <param name="accumulator"></param>
        public virtual TrackedProjection Emit<TKey, TValue>(
            Guid originalId, TKey key, TValue accumulator, TValue instance, Func<TKey, TValue, TValue> func)
        {
            throw new NotImplementedException();
        }
    }
}