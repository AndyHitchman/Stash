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

namespace Stash.Example
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Spark;
    using Spark.Web.Mvc;
    using StructureMap;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                // Route name
                "{controller}/{action}/{id}",
                // URL with parameters
                new {controller = "Home", action = "Index", id = ""} // Parameter defaults
                );
        }

        public void ConfigureIoC(string configPath)
        {
            ObjectFactory.Configure(
                c =>
                {
                    c.ForRequestedType<IViewEngine>()
                        .TheDefaultIsConcreteType<SparkViewFactory>();

                    c.ForRequestedType<IViewActivatorFactory>()
                        .TheDefaultIsConcreteType<StructureMapViewActivator>();

                    c.Scan(scanner => scanner.AddAllTypesOf<IController>().NameBy(type => type.Name.ToUpper()));
                });

            // Place this container as the dependency resolver and hook it into
            // the controller factory mechanism
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            ViewEngines.Engines.Add(ObjectFactory.GetInstance<IViewEngine>());

        }

        protected void Application_Start()
        {
            var settings = new SparkSettings()
                .AddNamespace("System")
                .AddNamespace("System.Collections.Generic")
                .AddNamespace("System.Linq")
                .AddNamespace("System.Web.Mvc")
                .AddNamespace("System.Web.Mvc.Html");

            ViewEngines.Engines.Add(new SparkViewFactory(settings));

            RegisterRoutes(RouteTable.Routes);
        }
    }
}