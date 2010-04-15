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
    /// Here is a simple object graph (a class with members of arbitrary depth) that we want to persist.
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


    
    
    public class Ch2__The_Stashed_Set : Chapter
    {
        //Note that these are NOT unit tests. The order of execution is significant


        private ISession session;


        [TestFixtureSetUp]
        public void Lets_set_things_up()
        {
            //Kickstart Stash by registering Customer as a persistent graph.
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                _ =>
                    {
                        _.Register<Customer>();
                    });
        }


        [SetUp]
        public void We_will_need_a_session_for_each_fact()
        {
            //Get a session to do work.
            session = Kernel.SessionFactory.GetSession();
        }

        [TearDown]
        public void And_we_should_make_sure_we_complete_the_session_too()
        {
            session.Complete();
        }


        /// <summary>
        /// With a <see cref="StashedSet{TGraph}"/> use the <see cref="StashedSet{TGraph}.Endure"/>
        /// method to persist a registered graph. The default serialiser is <see cref="BinarySerializer{TGraph}"/>.
        /// </summary>
        [Fact]
        public void a___Persist_a_registred_graph_by_calling_Endure()
        {
            var customerStash = session.GetStashOf<Customer>();

            var customer = new Customer { Number = 123, Name = "Acme Bolts" };
            customerStash.Endure(customer);

            //Endure simply starts tracking the graph. To actually perform a save, you must 
            //complete the session.

            session.Complete();
        }


        /// <summary>
        /// From the session you can get a <see cref="StashedSet{TGraph}"/> to manipulate persisted graphs of a specific type.
        /// </summary>
        [Fact]
        public void b___Use_a_StashedSet_to_manipulate_persisted_sets_of_graphs()
        {
            var customers = session.GetStashOf<Customer>();

            customers.Count().ShouldEqual(1);
        }


        /// <summary>
        /// From the session you can get every persisted graph by calling <see cref="ISession.GetEntireStash"/>.
        /// However, you can't enumerate everything---you need to match with a query.
        /// </summary>
        [Fact]
        public void c___Use_Session_GetEntireStash_to_manipulate_every_persisted_graph()
        {
            var everything = session.GetEntireStash();
        }


        [Fact]
        public void d___Delete_a_persisted_graph_by_calling_Destroy()
        {
            var customers = session.GetStashOf<Customer>();
            var theOnlyCustomer = customers.First();

            customers.Destroy(theOnlyCustomer);

            session.Complete();
        }


        [TestFixtureTearDown]
        public void End_of_chapter()
        {
            Kernel.Shutdown();
        }
    }
}