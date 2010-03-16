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
    using System;
    using System.Collections.Generic;
    using Configuration;

    public interface IQueryFactory
    {
        IAllOfQuery<TKey> AllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IIntersectOperator IntersectionOf(IEnumerable<IQuery> queries);
        IAnyOfQuery<TKey> AnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IBetweenQuery<TKey> Between<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IEqualToQuery<TKey> EqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IGreaterThanQuery<TKey> GreaterThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IGreaterThanEqualQuery<TKey> GreaterThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IInsideQuery<TKey> Inside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        ILessThanQuery<TKey> LessThan<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        ILessThanEqualQuery<TKey> LessThanEqual<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotAnyOfQuery<TKey> NotAllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotAnyOfQuery<TKey> NotAnyOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotEqualToQuery<TKey> NotEqualTo<TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IUnionOperator UnionOf(IEnumerable<IQuery> queries);
        IOutsideQuery<TKey> Outside<TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IIsIndexedQuery IsIndexed(IRegisteredIndexer indexer);
    }
}