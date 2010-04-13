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

namespace Stash
{
    using System;
    using System.Web;
    using BackingStore;
    using BackingStore.BDB;
    using Configuration;
    using Engine;
    using Queries;

    public static class Kernel
    {
        public static ISessionFactory SessionFactory { get; private set; }

        public static IRegistry Registry { get; set; }

        /// <summary>
        /// Kickstart Stash by providing a <paramref name="backingStore"/> and registering graphs
        /// and indexes inside the context of the <paramref name="persistenceConfigurationAction"/> delegate.
        /// Finally take the final configuration in the <paramref name="delegatedConfigurationAction"/> as an 
        /// opportunity to perform external configuration, such as setting up an inversion of control container.
        /// </summary>
        /// <typeparam name="TBackingStore"></typeparam>
        /// <param name="backingStore"></param>
        /// <param name="persistenceConfigurationAction"></param>
        /// <param name="delegatedConfigurationAction"></param>
        public static void Kickstart<TBackingStore>(
            TBackingStore backingStore,
            Action<PersistenceContext<TBackingStore>> persistenceConfigurationAction,
            Action<IRegistry, IBackingStore, IQueryFactory> delegatedConfigurationAction)
            where TBackingStore : IBackingStore
        {
            var registrar = new Registrar<TBackingStore>(backingStore);
            registrar.PerformRegistration(persistenceConfigurationAction);
            registrar.ApplyRegistration();
            Registry = registrar.Registry;
            SessionFactory = new SessionFactory(Registry);

            delegatedConfigurationAction(Registry, backingStore, backingStore.QueryFactory);
        }

        /// <summary>
        /// Kickstart Stash by providing a <paramref name="backingStore"/> and registering graphs
        /// and indexes inside the context of the <paramref name="persistenceConfigurationAction"/> delegate.
        /// </summary>
        /// <param name="backingStore"></param>
        /// <param name="configurationAction"></param>
        public static void Kickstart(BerkeleyBackingStore backingStore, Action<PersistenceContext<BerkeleyBackingStore>> configurationAction)
        {
            Kickstart(backingStore, configurationAction, (registry, store, queryFactory) => { });
        }

        /// <summary>
        /// Shutdown Stash. The backing store resource is released and no further work with Stash can be safely performed.
        /// </summary>
        public static void Shutdown()
        {
            Registry.BackingStore.Close();
            Registry = null;
        }

        /// <summary>
        /// The HttpRequestManager provides methods that bind to <see cref="HttpApplication"/> events to provide
        /// a simple central configuration for Stash-related configuration per Http Request.
        /// </summary>
        public static class HttpRequestManager
        {
            /// <summary>
            /// Stores a <see cref="ISession"/> in <see cref="HttpContext.Items"/>.
            /// </summary>
            public static ISession CurrentSession
            {
                get { return (ISession)HttpContext.Current.Items["__Stash__CurrentSession"]; }
                private set { HttpContext.Current.Items["__Stash__CurrentSession"] = value; }
            }

            /// <summary>
            /// Use this method to take an action, such as setting-up an IoC container, on each request.
            /// It is preferable to use the built-in lifecycle support of your container if possible.
            /// </summary>
            /// <param name="onBeginRequest"></param>
            /// <param name="onEndRequest"></param>
            /// <param name="onUnhandledException"></param>
            public static void ByActingPerHttpRequest(
                Action<ISessionFactory> onBeginRequest,
                Action onEndRequest,
                Action onUnhandledException)
            {
                var application = HttpContext.Current.ApplicationInstance;
                application.BeginRequest += (sender, args) => onBeginRequest(SessionFactory);
                application.EndRequest += (o, eventArgs) => onEndRequest();
                application.Error += (sender, eventArgs) => onUnhandledException();
            }

            /// <summary>
            /// This configuration puts a new session into <see cref="HttpContext.Items"/> on <see cref="HttpApplication.BeginRequest"/>
            /// and calls <see cref="ISession.Complete"/> on <see cref="HttpApplication.EndRequest"/>
            /// and <see cref="ISession.Abandon"/> on <see cref="HttpApplication.Error"/>
            /// </summary>
            public static void ByCreatingANewCurrentSession()
            {
                ByActingPerHttpRequest(
                    factory => CurrentSession = factory.GetSession(),
                    () => CurrentSession.Complete(),
                    () => CurrentSession.Abandon());
            }
        }
    }
}