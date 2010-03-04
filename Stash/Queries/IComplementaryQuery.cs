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

namespace Stash.Queries
{
    public interface IComplementaryQuery<TQuery> where TQuery : IQuery
    {
        /// <summary>
        /// Used by the not unary operator to get the complement (i.e. yielding the inverse set of 
        /// results) of its given query.
        /// The implementor has two basic strategies and may decide which to implement
        /// given their knowledge of the underlying storage architecture.
        /// 1) Actually return the complementary query and let the not operator execture this, or
        /// 2) Return a graph table scan wrapper around the original query that gets everything
        /// but the results of the original query.
        /// The first option will more generally be more efficient.
        /// </summary>
        /// <returns></returns>
        TQuery GetComplementaryQuery();
    }
}