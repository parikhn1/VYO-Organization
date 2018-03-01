using System.IO;
using System.Web;
using System.Web.Mvc;
using VYODAL.DB.Repository;

namespace VYO.Controllers
{

    public class BhajanDocumentsController : Controller
    {
        enum BhajanSpecificationEnum { ShreenathjiBhajan, YamunajiBhajan, VallabhBhajan }
        public ActionResult Add()
        {
            return View();
        }
        // GET: BhajanDocumnets
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase file, FormCollection Collection)
        {
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            string BhajanName = string.Empty;
            int BhajanSpecification = 1;
            int BhajanClassification = 1;
            if (file != null)
            {
                foreach (string Key in Collection)
                {
                    if (Key.Contains(BhajanSpecificationEnum.ShreenathjiBhajan.ToString()))
                        {
                        BhajanSpecification = 1;
                    }

                    else if (Key.Contains(BhajanSpecificationEnum.YamunajiBhajan.ToString()))
                    {
                        BhajanSpecification = 2;
                    }
                    else
                    {
                        BhajanSpecification = 3;
                    }
                }
                BhajanName = Path.GetFileName(file.FileName.ToString());
                var RootedPath = Server.MapPath("~/Documents/");
                file.SaveAs(Path.Combine(RootedPath, BhajanName));
                
                if (DocExploreObj.AddBhajan(BhajanSpecification, BhajanClassification, BhajanName))
                    ViewBag.Message = "Successfully Added record";
                else
                    ViewBag.Message = "Something wrong to add record";
            }
            else
                ViewBag.Message = "Not able to passed file";

            return View();
        }
        public ActionResult CustomIndex()
        {
            return View("CustomIndex");

        }
        public ActionResult AddCoverPage(HttpPostedFileBase file, FormCollection Collection)
        {
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            string CoverPageName = string.Empty;
           
            if (file != null)
            {
                CoverPageName = Path.GetFileName(file.FileName.ToString());
                var RootedPath = Server.MapPath("~/Assets/Images");
                file.SaveAs(Path.Combine(RootedPath, CoverPageName));
               
                if (DocExploreObj.AddCoverPage(CoverPageName))
                    ViewBag.Message = "Successfully Added record";
                else
                    ViewBag.Message = "Something wrong to add record";
            }
            else
                ViewBag.Message = "Not able to passed file";

            return View();
        }
        public ActionResult pdfView()
        {

            return View("PdfView");
        }
       
    }
}