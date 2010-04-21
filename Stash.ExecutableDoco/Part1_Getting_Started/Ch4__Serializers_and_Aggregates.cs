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
    using BackingStore.BDB;
    using Engine.Serializers.Binary;
    using NUnit.Framework;
    using Support;


    public class Ch4__Serializers_and_Aggregates : Chapter
    {
        //Note that these are NOT unit tests. The order of execution is significant


        /// <summary>
        /// Customer represents an aggregrate root (in the Domain-driven design sense).
        /// </summary>
        [Serializable]
        public class Customer
        {
            public Customer()
            {
                Orders = new List<Order>();
            }

            public int Number { get; set; }
            public string Name { get; set; }
            public IList<Order> Orders { get; set; }
        }


        /// <summary>
        /// Order is another distinct aggregate root.
        /// </summary>
        [Serializable]
        public class Order
        {
            public int Number { get; set; }
            public Customer Customer { get; set; }
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

        public class OrdersByNumber : IIndex<Order, int> 
        {
            public IEnumerable<int> Yield(Order graph)
            {
                yield return graph.Number;
            }
        }

        public class OrdersByCustomerNumber : IIndex<Order,int> 
        {
            public IEnumerable<int> Yield(Order graph)
            {
                yield return graph.Customer.Number;
            }
        }


        private ISession session;


        /// <summary>
        ///   When we register the Customer and Order graphs, we use the <see cref="AggregateBinarySerializer{TGraph}"/>.
        ///   This causes all references to other registered graphs to be serialised as references.
        ///   For example, the member <see cref="Order.Customer"/> will be serialised using the Stash internal id for the
        ///   customer, rather than a copy of the Customer instance. When deserialising, the Customer instance will be added
        ///   to the session, if it isn't already being tracked, by deserialising it from the backing store. Lazy loading is not
        ///   yet implemented, but is planned.
        /// </summary>
        [TestFixtureSetUp]
        public void Lets_set_things_up()
        {
            //Kickstart Stash by registering Customer and Order as a persistent graphs using the AggregateBinarySerializer.
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(TempDir)),
                register =>
                    {
                        register.Graph<Customer>(_ => _.SerializeWith(new AggregateBinarySerializer<Customer>(_.RegisteredGraph)));
                        register.Graph<Order>(_ => _.SerializeWith(new AggregateBinarySerializer<Order>(_.RegisteredGraph)));
                        register.Index(new CustomersByNumber());
                        register.Index(new OrdersByNumber());
                        register.Index(new OrdersByCustomerNumber());
                    });
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


        /// <summary>
        /// When we persist a customer, the list of references to the orders is persisted. The opposite reference
        /// from Order to Customer is also persisted as a reference. The association does not need to be navigable in
        /// both directions. Stash does nothing to maintain these associations for you. Your model is responsible for
        /// this behaviour.
        /// <para/>
        /// Also, Stash must be explicitly told to endure both the customer and the order. It will not persist an order
        /// on your behalf because it is referenced by a customer that you have endured.
        /// </summary>
        [Fact]
        public void a___When_we_persist_a_customer_it_persists_a_reference_to_any_orders()
        {
            var customer = new Customer {Number = 1, Name = "Acme Bolts"};
            var order1 = new Order {Customer = customer, Number = 1001};
            var order2 = new Order {Customer = customer, Number = 1002};
            var order3 = new Order {Customer = customer, Number = 1003};

            customer.Orders.Add(order1);
            customer.Orders.Add(order2);
            customer.Orders.Add(order3);

            session.Endure(customer);
            session.Endure(order1);
            session.Endure(order2);
            session.Endure(order3);

            session.Complete();
        }
        

        [Fact]
        public void b___When_we_fetch_these_graphs_they_are_reference_equal()
        {
            var customersStash = session.GetStashOf<Customer>();
            var ordersStash = session.GetStashOf<Order>();

            var order1 = ordersStash.Matching(_ => _.Where<OrdersByNumber>().EqualTo(1001)).Single();
            var customer = customersStash.Matching(_ => _.Where<CustomersByNumber>().EqualTo(1)).Single();
            var order2 = ordersStash.Matching(_ => _.Where<OrdersByNumber>().EqualTo(1002)).Single();

            customer.Orders.ShouldContain(order1);
            customer.Orders.ShouldContain(order2);
            customer.Orders.ShouldContain(_ => _.Number == 1003);

            order1.Customer.ShouldBeTheSameAs(customer);
            order2.Customer.ShouldBeTheSameAs(customer);
        }
        
        
        
        [TestFixtureTearDown]
        public void End_of_chapter()
        {
            Kernel.Shutdown();
        }
    }
}