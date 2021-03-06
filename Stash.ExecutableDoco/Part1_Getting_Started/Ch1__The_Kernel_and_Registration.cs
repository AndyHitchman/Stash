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

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToLambdaExpression
namespace Stash.ExecutableDoco.Part1_Getting_Started
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using BerkeleyDB;
    using BerkeleyDB.Engine;
    using NUnit.Framework;
    using Support;

    /// <summary>
    ///   The Stash Registry is used to configure the Stash runtime engine.
    ///   <para />
    /// </summary>
    public class Ch1__The_Kernel_and_Registration : Chapter
    {
        //Note that these are NOT unit tests. The order of execution is significant


        /// <summary>
        ///   You specify a backing store (<see cref = "IBackingStore" />) that manages the data persisted by 
        ///   the Stash engine.
        ///   <para />
        ///   In this example we use the <see cref = "BerkeleyBackingStore" /> (this is actually the only backing 
        ///   store implementation at present).
        /// </summary>
        [Fact]
        public void a___The_Kernel_static_class_is_used_to_kickstart_Stash_over_a_backing_store()
        {
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                register => { });
        }


        /// <summary>
        ///   If you do not call shutdown, then the Dispose() method on the backing store will do this eventually.
        /// </summary>
        [Fact]
        public void b___You_must_Shutdown_the_Kernel_to_ensure_Backing_Store_resources_are_released_cleanly()
        {
            Kernel.Shutdown();
        }


        /// <summary>
        ///   Here is a simple object graph (a class with members of arbitrary depth) that we want to persist.
        /// </summary>
        [Serializable]
        public class Customer
        {
            public Customer()
            {
                Contacts = new List<Contact>();
            }

            public int Number { get; set; }
            public string Name { get; set; }
            public IList<Contact> Contacts { get; private set; }
        }

        /// <summary>
        ///   <see cref = "Customer">Customers</see> can have multiple contacts. We're just assuming these are people.
        /// </summary>
        [Serializable]
        public class Contact
        {
            public string GivenName { get; set; }
            public string Initial { get; set; }
            public string FamilyName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }


        /// <summary>
        ///   Register the graph to be persisted by referring to the root class of the hierarchy.
        /// </summary>
        [Fact]
        public void c___Graphs_that_you_wish_to_persist_should_be_registered_during_kickstart()
        {
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                register => { register.Graph<Customer>(); });
        }


        /// <summary>
        ///   In addition to using the static Kernel to get a session, it is possible to use an inversion
        ///   of control container to inject a session into your application. TODO show example of IoC
        /// </summary>
        [Fact]
        public void d___The_simplest_way_to_interact_with_Stash_is_through_a_session()
        {
            var session = Kernel.SessionFactory.GetSession();
        }


        [TestFixtureTearDown]
        public void End_of_chapter()
        {
            Kernel.Shutdown();
        }
    }
}