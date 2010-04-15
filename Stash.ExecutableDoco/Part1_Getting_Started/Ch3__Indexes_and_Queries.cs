// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToLambdaExpression
namespace Stash.ExecutableDoco.Part1_Getting_Started
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BackingStore;
    using BackingStore.BDB;
    using Engine.Serializers.Binary;
    using NUnit.Framework;
    using Support;


    /// <summary>
    /// We can create an index over our customer object. In this case we index
    /// the customer number. This is analogous to the primary key.
    /// </summary>
    public class CustomersByNumber : IIndex<Customer, int>
    {
        /// <summary>
        /// For each Customer we yield one value for the customer number.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IEnumerable<int> Yield(Customer customer)
        {
            yield return customer.Number;
        }
    }


    /// <summary>
    /// This index yields multiple keys, one per contact for the customer. The index
    /// is always against the graph, rather than a member (i.e. customer, not the contact).
    /// Indexes can be against supertypes or interfaces, which provides for some powerful
    /// polymorphic behaviour in queries. TODO Demo this
    /// </summary>
    public class CustomersByContactFamilyName : IIndex<Customer, string>
    {
        public IEnumerable<string> Yield(Customer customer)
        {
            return customer.Contacts.Select(contact => contact.FamilyName.ToUpper());
        }
    }


    /// <summary>
    /// </summary>
    public class CustomersByNumberOfContacts : IIndex<Customer, int>
    {
        public IEnumerable<int> Yield(Customer customer)
        {
            yield return customer.Contacts.Count();
        }
    }


    public class Ch3__Indexes_and_Queries : Chapter
    {
        //Note that these are NOT unit tests. The order of execution is significant


        private ISession session;


        /// <summary>
        /// We need to register indexes in addition to graphs. TODO Index on type hierarcy or interface
        /// </summary>
        [TestFixtureSetUp]
        public void Lets_set_things_up()
        {
            //Kickstart Stash by registering Customer as a persistent graph.
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                _ =>
                    {
                        _.Register<Customer>();
                        _.Index(new CustomersByNumber());
                        _.Index(new CustomersByContactFamilyName());
                        _.Index(new CustomersByNumberOfContacts());
                    });


            persist_some_customers();
        }


        private void persist_some_customers() 
        {
            var setUpSession = Kernel.SessionFactory.GetSession();

            var customerStash = setUpSession.GetStashOf<Customer>();

            var customer1 = new Customer { Number = 5, Name = "Acme Tackle" };
            customer1.Contacts.Add(new Contact { GivenName = "Bob", FamilyName = "Smith" });
            customer1.Contacts.Add(new Contact { GivenName = "Jane", FamilyName = "Jones" });

            var customer2 = new Customer { Number = 20, Name = "Waldo Robotics" };
            customer2.Contacts.Add(new Contact { GivenName = "Henry", FamilyName = "Dangerfield" });
            customer2.Contacts.Add(new Contact { GivenName = "Roberta", FamilyName = "Williams" });
            customer2.Contacts.Add(new Contact { GivenName = "Fred", FamilyName = "Smith" });

            //We have a Smith contact for both customers.

            var customer3 = new Customer { Number = 1, Name = "Spam4U" };
            customer3.Contacts.Add(new Contact { GivenName = "Dick", FamilyName = "Dastardly" });

            customerStash.Endure(customer1);
            customerStash.Endure(customer2);
            customerStash.Endure(customer3);

            setUpSession.Complete();
        }


        [SetUp]
        public void We_will_need_a_session_and_customers_for_each_fact()
        {
            session = Kernel.SessionFactory.GetSession();
        }


        [Fact]
        public void a___We_can_get_customers_by_their_number()
        {
            var customers = session.GetStashOf<Customer>();

            var theSpammer = customers.Matching(_ => _.Where<CustomersByNumber>().EqualTo(1)).Single();

            theSpammer.Number.ShouldEqual(1);
            theSpammer.Name.ShouldEqual("Spam4U");
        }


        [Fact]
        public void b___We_can_get_customers_with_more_than_one_contact()
        {
            var customersWithManyContacts = session.GetStashOf<Customer>().Matching(_ => _.Where<CustomersByNumberOfContacts>().GreaterThan(1));

            customersWithManyContacts.ShouldHaveCount(2);
        }


        [Fact]
        public void c___We_can_get_customers_with_contacts_having_the_family_name_Smith()
        {
            var customersEmployingSmiths = session.GetStashOf<Customer>().Matching(_ => _.Where<CustomersByContactFamilyName>().EqualTo("SMITH"));

            customersEmployingSmiths.ShouldHaveCount(2);
        }


        [Fact]
        public void d___We_can_get_customers_with_contacts_having_the_family_name_starting_with_Da()
        {
            var customersWithEmployeesHavingNamesStartingDa = session.GetStashOf<Customer>().Matching(_ => _.Where<CustomersByContactFamilyName>().StartsWith("DA"));

            customersWithEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
        }


        [Fact]
        public void e___We_can_get_join_customers_having_between_1_and_2_contacts()
        {
            var customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(_ => _.Where<CustomersByNumberOfContacts>().Between(1, 2));

            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(2);
        }


        [Fact]
        public void f___We_can_get_join_customers_with_contacts_having_the_family_name_starting_with_Da_and_between_1_and_2_contacts()
        {
            var customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa =
                session.GetStashOf<Customer>()
                    .Matching(
                        _ =>
                        _.IntersectionOf(
                            _.Where<CustomersByContactFamilyName>().StartsWith("DA"),
                            _.Where<CustomersByNumberOfContacts>().Between(1, 2)
                            )
                    );

            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.ShouldHaveCount(1);
            customersWithMoreThanOneContactAndEmployeesHavingNamesStartingDa.Single().Number.ShouldEqual(1);
        }


        [TearDown]
        public void Make_sure_we_complete_the_session()
        {
            session.Complete();
        }

        
        [TestFixtureTearDown]
        public void End_of_chapter()
        {
            Kernel.Shutdown();
        }
    }
}