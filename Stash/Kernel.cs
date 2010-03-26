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

        public static void Kickstart<TBackingStore>(
            TBackingStore backingStore,
            Action<PersistenceContext<TBackingStore>> configurationAction,
            Action<IRegistry, IBackingStore, IQueryFactory> iocSingletonConfiguration)
            where TBackingStore : IBackingStore
        {
            var registrar = new Registrar<TBackingStore>(backingStore);
            registrar.PerformRegistration(configurationAction);
            registrar.ApplyRegistration();
            Registry = registrar.Registry;
            SessionFactory = new SessionFactory();

            iocSingletonConfiguration(Registry, backingStore, backingStore.QueryFactory);
        }

        public static void Kickstart(BerkeleyBackingStore backingStore, Action<PersistenceContext<BerkeleyBackingStore>> configurationAction)
        {
            Kickstart(backingStore, configurationAction, (registry, store, arg3) => { });
        }

        public static void Shutdown()
        {
            Registry.BackingStore.Close();
            Registry = null;
        }

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
            /// This configuration puts a new session into <see cref="HttpContext.Items"/>
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