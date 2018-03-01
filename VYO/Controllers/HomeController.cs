using System.Web.Mvc;
using VYODAL.DB.Repository;

namespace VYO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            return View(DocExploreObj.GetDocuments());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}