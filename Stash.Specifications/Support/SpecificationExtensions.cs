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

namespace Stash.Specifications.Support
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using NUnit.Framework;

    /// <summary>
    /// specification extensions.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// attribute should equal.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The attribute should equal.
        /// </returns>
        public static object AttributeShouldEqual(
            this XmlElement element,
            string attributeName,
            object expected)
        {
            Assert.IsNotNull(element, "The Element is null");

            var actual = element.GetAttribute(attributeName);
            Assert.AreEqual(expected, actual);
            return expected;
        }

        /// <summary>
        /// does not have attribute.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <returns>
        /// </returns>
        public static XmlElement DoesNotHaveAttribute(this XmlElement element, string attributeName)
        {
            Assert.IsNotNull(element, "The Element is null");
            Assert.IsFalse(
                element.HasAttribute(attributeName),
                "Element should not have an attribute named " + attributeName);

            return element;
        }

        /// <summary>
        /// should be empty.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldBeEmpty<T>(this IEnumerable<T> actual)
        {
            ShouldEqual(actual.Count(), 0);
        }

        /// <summary>
        /// should be empty.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        /// <summary>
        /// should be empty.
        /// </summary>
        /// <param name="aString">
        /// The a string.
        /// </param>
        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        /// <summary>
        /// should be equal ignoring case.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The should be equal ignoring case.
        /// </returns>
        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return expected;
        }

        /// <summary>
        /// should be false.
        /// </summary>
        /// <param name="condition">
        /// The condition.
        /// </param>
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        /// <summary>
        /// should be greater than.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
            return arg2;
        }

        /// <summary>
        /// should be less than.
        /// </summary>
        /// <param name="arg1">
        /// The arg 1.
        /// </param>
        /// <param name="arg2">
        /// The arg 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
            return arg2;
        }

        /// <summary>
        /// should be null.
        /// </summary>
        /// <param name="anObject">
        /// The an object.
        /// </param>
        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        /// <summary>
        /// should be of type.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldBeOfType(typeof(T));
        }

        /// <summary>
        /// should be of type.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsInstanceOfType(expected, actual);
        }

        /// <summary>
        /// should be the same as.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The should be the same as.
        /// </returns>
        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        /// <summary>
        /// should be thrown by.
        /// </summary>
        /// <param name="exceptionType">
        /// The exception type.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// </returns>
        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        {
            Exception exception = null;

            try
            {
                method();
            }
            catch(Exception e)
            {
                Assert.AreEqual(exceptionType, e.GetType());
                exception = e;
            }

            if(exception == null)
                Assert.Fail(String.Format("Expected {0} to be thrown.", exceptionType.FullName));

            return exception;
        }

        /// <summary>
        /// should be true.
        /// </summary>
        /// <param name="condition">
        /// The condition.
        /// </param>
        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        /// <summary>
        /// should contain.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.Contains(expected, actual);
        }

        /// <summary>
        /// should contain.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldContain<T>(this IEnumerable<T> actual, T expected)
        {
            ShouldContain(actual, x => x.Equals(expected));
        }

        /// <summary>
        /// should contain.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
        {
            ShouldNotEqual(actual.Single(expected), default(T));
        }

        /// <summary>
        /// should contain.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        /// <summary>
        /// should contain error message.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
        }

        /// <summary>
        /// should end with.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
        }

        /// <summary>
        /// should equal.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The should equal.
        /// </returns>
        public static object ShouldEqual(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        /// <summary>
        /// should equal sql date.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldEqualSqlDate(this DateTime actual, DateTime expected)
        {
            var timeSpan = actual - expected;
            Assert.Less(Math.Abs(timeSpan.TotalMilliseconds), 3);
        }

        /// <summary>
        /// should have child.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="xpath">
        /// The xpath.
        /// </param>
        /// <returns>
        /// </returns>
        public static XmlElement ShouldHaveChild(this XmlElement element, string xpath)
        {
            var child = element.SelectSingleNode(xpath) as XmlElement;
            Assert.IsNotNull(child, "Should have a child element matching " + xpath);

            return child;
        }

        /// <summary>
        /// should have count.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldHaveCount<T>(this IEnumerable<T> actual, int expected)
        {
            ShouldEqual(actual.Count(), expected);
        }

        /// <summary>
        /// should not be empty.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsNotEmpty(collection);
        }

        /// <summary>
        /// should not be empty.
        /// </summary>
        /// <param name="aString">
        /// The a string.
        /// </param>
        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }

        /// <summary>
        /// should not be null.
        /// </summary>
        /// <param name="anObject">
        /// The an object.
        /// </param>
        public static void ShouldNotBeNull(this object anObject)
        {
            Assert.IsNotNull(anObject);
        }

        /// <summary>
        /// should not be of type.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void ShouldNotBeOfType<T>(this object actual)
        {
            actual.ShouldNotBeOfType(typeof(T));
        }

        /// <summary>
        /// should not be of type.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotInstanceOfType(expected, actual);
        }

        /// <summary>
        /// should not be the same as.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The should not be the same as.
        /// </returns>
        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        /// <summary>
        /// should not equal.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// The should not equal.
        /// </returns>
        public static object ShouldNotEqual(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        /// <summary>
        /// should start with.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        public static void ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
        }
    }
}