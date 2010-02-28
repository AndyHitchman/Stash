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
        IAllOfQuery<TKey> AllOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IBetweenQuery<TKey> Between<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IEqualToQuery<TKey> EqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IInsideQuery<TKey> Inside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        ILessThanQuery<TKey> LessThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IOutsideQuery<TKey> Outside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>;
        IAndQuery And(IQuery lhs, IQuery rhs);
        IAndQuery Or(IQuery lhs, IQuery rhs);
    }
}