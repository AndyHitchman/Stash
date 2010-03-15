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

namespace Stash.BackingStore.BDB
{
    using System;
    using System.Collections.Generic;
    using BerkeleyQueries;
    using Configuration;
    using Queries;

    public class BerkeleyQueryFactory : IQueryFactory
    {
        public IAllOfQuery<TKey> AllOf<TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new AllOfQuery<TKey>(indexer, set);
        }

        public IIntersectOperator IntersectionOf(IQuery lhs, IQuery rhs)
        {
            throw new NotImplementedException();
        }

        public IAnyOfQuery<TKey> AnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IBetweenQuery<TKey> Between<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IEqualToQuery<TKey> EqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            return new EqualToQuery<TKey>(indexer, key);
        }

        public IGreaterThanQuery<TKey> GreaterThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IGreaterThanEqualQuery<TKey> GreaterThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IInsideQuery<TKey> Inside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public ILessThanQuery<TKey> LessThan<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public ILessThanEqualQuery<TKey> LessThanEqual<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public INotAnyOfQuery<TKey> NotAllOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public INotAnyOfQuery<TKey> NotAnyOf<TGraph, TKey>(IRegisteredIndexer indexer, IEnumerable<TKey> set) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public INotEqualToQuery<TKey> NotEqualTo<TGraph, TKey>(IRegisteredIndexer indexer, TKey key) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IIntersectOperator UnionOf(IQuery lhs, IQuery rhs)
        {
            throw new NotImplementedException();
        }

        public IOutsideQuery<TKey> Outside<TGraph, TKey>(IRegisteredIndexer indexer, TKey lowerKey, TKey upperKey) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public IIsIndexedQuery IsIndexed(IRegisteredIndexer indexer)
        {
            throw new NotImplementedException();
        }
    }
}