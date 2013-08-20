using StoneFinch.SmpMaintenance.Views.Web.Interop;
using System.Web.Mvc;

namespace StoneFinch.SmpMaintenance.Views.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {
            // no-op
        }

        public ActionResult Unknown()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}