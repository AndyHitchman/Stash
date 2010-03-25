namespace Stash.Example
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using StructureMap;

    public class StructureMapControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            return ObjectFactory.GetNamedInstance<IController>(controllerName.ToUpper() + "CONTROLLER");
        }

        public void ReleaseController(IController controller)
        {
        }
    }
}