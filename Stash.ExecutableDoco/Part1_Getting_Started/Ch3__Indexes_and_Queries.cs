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
    using System.Linq;
    using BerkeleyDB;
    using NUnit.Framework;
    using Support;


    public class Ch3__Indexes_and_Queries : Chapter
    {
        //Note that these are NOT unit tests. The order of execution is significant


        /// <summary>
        /// Here is a simple object graph (a class with members of arbitrary depth) that we want to persist.
        /// </summary>
        [Serializable]
        public partial class Customer
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
        /// <see cref="Customer">Customers</see> can have multiple contacts. We're just assuming these are people.
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
        ///   We can create an index over our customer object. In this case we index
        ///   the customer number. This is analogous to the primary key.
        /// </summary>
        public class CustomersByNumber : IIndex<Customer, int>
        {
            /// <summary>
            ///   For each Customer we yield one value for the customer number.
            /// </summary>
            /// <param name = "customer"></param>
            /// <returns></returns>
            public IEnumerable<int> Yield(Customer customer)
            {
                yield return customer.Number;
            }
        }


        /// <summary>
        ///   This index yields multiple keys, one per contact for the customer. The index
        ///   is always against the graph root, rather than a member (i.e. customer, not the contact).
        ///   Indexes can be against supertypes or interfaces, which provides for some powerful
        ///   polymorphic behaviour in queries. TODO Demo this
        /// </summary>
        public class CustomersByContactFamilyName : IIndex<Customer, string>
        {
            public IEnumerable<string> Yield(Customer customer)
            {
                return customer.Contacts.Select(contact => contact.FamilyName.ToUpper());
            }
        }


        /// <summary>
        ///   This index is returning aggregate information for a customer, in this case the number of
        ///   contacts.
        /// </summary>
        public class CustomersByNumberOfContacts : IIndex<Customer, int>
        {
            public IEnumerable<int> Yield(Customer customer)
            {
                yield return customer.Contacts.Count();
            }
        }


        private ISession session;


        /// <summary>
        ///   We need to register indexes in addition to graphs. We register instances on indexes. They are used
        ///   at runtime by Stash. Indexes should be stateless and not hold on to resources.
        /// </summary>
        [TestFixtureSetUp]
        public void Lets_set_things_up()
        {
            //Kickstart Stash by registering Customer as a persistent graph.
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                register =>
                    {
                        register.Graph<Customer>();
                        register.Index(new CustomersByNumber());
                        register.Index(new CustomersByContactFamilyName());
                        register.Index(new CustomersByNumberOfContacts());
                    });


            persist_some_customers();
        }


        private void persist_some_customers()
        {
            var setUpSession = Kernel.SessionFactory.GetSession();

            var customerStash = setUpSession.GetStashOf<Customer>();

            var customer1 = new Customer {Number = 5, Name = "Acme Tackle"};
            customer1.Contacts.Add(new Contact {GivenName = "Bob", FamilyName = "Smith"});
            customer1.Contacts.Add(new Contact {GivenName = "Jane", FamilyName = "Jones"});

            var customer2 = new Customer {Number = 20, Name = "Waldo Robotics"};
            customer2.Contacts.Add(new Contact {GivenName = "Henry", FamilyName = "Dangerfield"});
            customer2.Contacts.Add(new Contact {GivenName = "Roberta", FamilyName = "Williams"});
            customer2.Contacts.Add(new Contact {GivenName = "Fred", FamilyName = "Smith"});

            //We have a Smith contact for both of these customers.

            var customer3 = new Customer {Number = 1, Name = "Spam4U"};
            customer3.Contacts.Add(new Contact {GivenName = "Dick", FamilyName = "Dastardly"});

            customerStash.Endure(customer1);
            customerStash.Endure(customer2);
            customerStash.Endure(customer3);

            setUpSession.Complete();
        }


        [SetUp]
        public void We_will_need_a_session_for_each_fact()
        {
            session = Kernel.SessionFactory.GetSession();
        }

        [TearDown]
        public void Make_sure_we_complete_the_session()
        {
            session.Complete();
        }


        [Fact]
        public void a___We_can_get_customers_by_their_number()
        {
            var customers = session.GetStashOf<Customer>();

            var theSpammer =
                customers.Matching(_ => _.Where<CustomersByNumber>().EqualTo(1)).Single();

            theSpammer.Number.ShouldEqual(1);
            theSpammer.Name.ShouldEqual("Spam4U");
        }


        [Fact]
        public void b___We_can_get_customers_with_more_than_one_contact()
        {
            var customersWithManyContacts =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByNumberOfContacts>().GreaterThan(1))
                    .ToList();

            customersWithManyContacts.ShouldHaveCount(2);
        }


        [Fact]
        public void c___We_can_get_customers_with_contacts_having_the_family_name_Smith()
        {
            var customersEmployingSmiths =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByContactFamilyName>().EqualTo("SMITH"))
                    .ToList();

            customersEmployingSmiths.ShouldHaveCount(2);
        }


        [Fact]
        public void d___We_can_get_customers_with_contacts_having_the_family_name_starting_with_Da()
        {
            var customersWithEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByContactFamilyName>().StartsWith("DA"));

            customersWithEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
        }


        [Fact]
        public void e___We_can_get_customers_having_between_1_and_2_contacts()
        {
            var customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByNumberOfContacts>().Between(1, 2));

            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
        }


        [Fact]
        public void f___We_can_join_customers_with_contacts_having_the_family_name_starting_with_Da_and_between_1_and_2_contacts()
        {
            var customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(
                        _ =>
                        _.IntersectionOf(
                            _.Where<CustomersByContactFamilyName>().StartsWith("DA"),
                            _.Where<CustomersByNumberOfContacts>().Between(1, 2)
                            )
                    )
                    .ToList();

            //It is important to take notice of the ToList() method which materialises the query (i.e. executes
            //the deferred queries). Without the ToList(), each assertion below would re-execute the query.

            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(1);
            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.Single().Number.ShouldEqual(1);
        }


        [Fact]
        public void g___We_can_union_customers_with_contacts_having_the_family_name_starting_with_Da_and_more_than_2_contacts()
        {
            var customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(
                        _ =>
                        _.UnionOf(
                            _.Where<CustomersByContactFamilyName>().StartsWith("DA"),
                            _.Where<CustomersByNumberOfContacts>().GreaterThan(2))
                    )
                    .Materialize();

            //Materialize() is a synonym for ToList(). Use Materialize rather than ToList as the intent is clearer.

            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldContain(_ => _.Name == "Waldo Robotics");
            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldContain(_ => _.Name == "Spam4U");
        }


        [Fact]
        public void h___We_can_get_union_everything_with_contacts_having_the_family_name_starting_with_Da_and_more_than_2_contacts()
        {
            var everythingWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetEntireStash()
                    .Matching(
                        _ =>
                        _.UnionOf(
                            _.Where<CustomersByContactFamilyName>().StartsWith("DA"),
                            _.Where<CustomersByNumberOfContacts>().GreaterThan(2))
                    )
                    .Materialize();

            everythingWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
        }


        [Fact]
        public void i___We_need_the_call_to_Materialize_as_we_can_extend_our_queries_by_adding_additional_matching_clauses()
        {
            var withTwoOrMoreContacts =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByNumberOfContacts>().GreaterThanEqual(2));

            var twoOrMoreAndEmployingSmith =
                withTwoOrMoreContacts
                    .Matching(_ => _.Where<CustomersByContactFamilyName>().EqualTo("SMITH"))
                    .Materialize();

            var twoOrMoreAndCustomerNumberGreaterThan10 =
                withTwoOrMoreContacts
                    .Matching(_ => _.Where<CustomersByNumber>().GreaterThan(10))
                    .Materialize();

            twoOrMoreAndEmployingSmith.ShouldHaveCount(2);
            twoOrMoreAndCustomerNumberGreaterThan10.ShouldHaveCount(1);
        }


        [TestFixtureTearDown]
        public void End_of_chapter()
        {
            Kernel.Shutdown();
        }
    }
}