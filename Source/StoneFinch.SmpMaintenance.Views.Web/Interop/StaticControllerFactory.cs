using StoneFinch.SmpMaintenance.Data;
using StoneFinch.SmpMaintenance.Views.Web.Controllers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StoneFinch.SmpMaintenance.Views.Web.Interop
{
    public class StaticControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var referenceDictionaryProvider = new HttpRuntimeCacheReferenceDictionaryProvider(HttpRuntime.Cache);
            var connectionStringProvider = new ReferenceDictionaryConnectionStringProvider(referenceDictionaryProvider);
            var userRepository = new UserRepository(connectionStringProvider);

            switch (controllerName)
            {
                case "Home":
                    return new HomeController(referenceDictionaryProvider, userRepository);

                case "Error":
                    return new ErrorController();
            }

            return base.CreateController(requestContext, controllerName);
        }
    }
}