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

namespace Stash.Engine.Partitioning
{
    using Queries;

    /// <summary>
    ///   A partitioned query excutes completely and consistently within a single partition.
    ///   Partitioning is governed by the graph, and all indexes are ported with the graph into
    ///   the same partition. All queries can be resolved for a single graph, so by having
    ///   the graph and it's indexes locally, we do not require any cross-partition resolution.
    ///   In particular, this greatly simplified Unions and Intersects.
    /// </summary>
    public interface IPartitionedQuery : IQuery
    {
        IQuery GetQueryForPartition(IPartition partition);
    }
}