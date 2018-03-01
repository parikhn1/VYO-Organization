using System.Web.Mvc;
using VYODAL.DB.Repository;
using VYO.Models;
using System.Collections.Generic;
using VYODomain.Entities;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

namespace VYO.Controllers
{
    public class BookletController : Controller
    {
        // GET: Default
        public ActionResult List()
        {
            BhajanDocumentModels BhajanDocModel = new BhajanDocumentModels();
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            BhajanDocModel.VYODocumnetExploreList = DocExploreObj.GetDocuments();
            ViewBag.PDFBhajanType = "PDF";
            ViewBag.TextBhajanType = "text";
            return View(BhajanDocModel);
        }
        public ActionResult CoverPages()
        {
            BhajanDocumentModels CovePageDocModel = new BhajanDocumentModels();
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            CovePageDocModel.VYOCoverPagesExplorList = DocExploreObj.GetCoverPages();
            return View("CoverPage", CovePageDocModel);
        }
        public ActionResult View(string[] SelectedBhajanList)
        {
            BhajanDocumentModels BhajanDocModel = new BhajanDocumentModels();
            List<BhajanName> BhajanList = new List<BhajanName>();

            foreach (string BhajanName in SelectedBhajanList)
            {
                BhajanName BhajanObj = new BhajanName();
                BhajanObj.Bhanjan = BhajanName;
                BhajanList.Add(BhajanObj);
            }
            BhajanDocModel.BhajanNameList = BhajanList;
            return View("View", BhajanDocModel);
        }


        public ActionResult GetCoverPages(string[] SelectedBhajanList)
        {
            string CurrentUserDir = Session["User"].ToString();
            var latestTime = DateTime.UtcNow.AddHours(-24);
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\";
            //Create User Dir
            bool exists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Documents\" + CurrentUserDir);

            if (!exists)
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Documents\" + CurrentUserDir);


            CurrentUserDir = (BaseDir + CurrentUserDir);

            foreach (string bhajan in SelectedBhajanList)
            {
                string BaseFile = BaseDir + bhajan;
                string DestFile = CurrentUserDir + "\\" + bhajan;
                System.IO.File.Copy(BaseFile, DestFile, true);
            }

            BhajanDocumentModels CovePageDocModel = new BhajanDocumentModels();
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            CovePageDocModel.VYOCoverPagesExplorList = DocExploreObj.GetCoverPages();
            return View("CoverPage", CovePageDocModel);

        }
        public ActionResult CustomIndex(string[] SelectedCoverPage)
        {
            string CurrentUserDir = Session["User"].ToString();
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\";
            string ImageDir = AppDomain.CurrentDomain.BaseDirectory + @"\Assets\Images\";
            foreach (string coverPage in SelectedCoverPage)
            {
                string BaseFile = ImageDir + coverPage;
                string DestFile = BaseDir + CurrentUserDir + "\\" + coverPage;
                System.IO.File.Copy(BaseFile, DestFile, true);
            }

            return View("CustomIndex");

        }

        public ActionResult SetProfile(FormCollection Collection)
        {
            BhajanDocumentModels BhajanDocModel = new BhajanDocumentModels();
            VYODocumentsExploreRepository DocExploreObj = new VYODocumentsExploreRepository();
            string UserName = Collection["UserName"];
            Session["User"] = UserName + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            if (Session["User"] == null)
            {
                ViewBag.Message = "Please Create Session to create booklet";
            }
            else
            {
                BhajanDocModel.VYODocumnetExploreList = DocExploreObj.GetDocuments();
                ViewBag.PDFBhajanType = "PDF";
                ViewBag.TextBhajanType = "text";
            }

            return View("List", BhajanDocModel);
        }

        //public ActionResult SaveFile(BhajanDocumentModels model)
        //{
        //    string CurrentUserDir = Session["User"].ToString();
        //    var latestTime = DateTime.UtcNow.AddHours(-24);
        //    string BaseDir = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\";
        //    //Create User Dir
        //    bool exists = Directory.Exists(Server.MapPath(@"~/Documents/" + CurrentUserDir));

        //    if (!exists)
        //        Directory.CreateDirectory(Server.MapPath(@"~/Documents/" + CurrentUserDir));

        //    DirectoryInfo directory = new DirectoryInfo(BaseDir + CurrentUserDir);

        //    //string MostRecentCustomFile = (from f in directory.GetFiles()
        //    //              .OrderByDescending(f => f.LastWriteTime)
        //    //              .Where(f => f.Name.Contains("CustomIndexFile"))
        //    //                     select f).First().ToString();

        //    //Directory.GetFiles(BaseDir)
        //    //    .Select(f => new FileInfo(f))
        //    //    .Where(f => f.Name.Contains("CustomIndexFile"))
        //    //    .Where(f => f.LastAccessTime < DateTime.Now.AddMinutes(-20))
        //    //    .ToList()
        //    //    .ForEach(f => f.Delete());

        //    string NewCustomFile = CurrentUserDir+"_CustomIndexFile"+".doc";
        //    //if (MostRecentCustomFile.Contains("CustomIndexFile"))
        //    //{
        //    //    int FileNumber = Convert.ToInt32(Regex.Match(MostRecentCustomFile, @"\d").Value);
        //    //    NewCustomFile = NewCustomFile + (FileNumber++) + ".doc";
        //    //}
        //    //else
        //    //    NewCustomFile = NewCustomFile + "1.doc";

        //    var i = model.CustomIndexDetail;
        //    System.IO.File.WriteAllText(BaseDir + CurrentUserDir + @"\" + NewCustomFile, i[0]);

        //    return View("CoverPage");
        //}

        public ActionResult SaveFile(BhajanDocumentModels model)
        {

            //Save custom File as pdf
            var latestTime = DateTime.UtcNow.AddHours(-24);
            string CurrentUserDir = Session["User"].ToString();
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\";
            string NewCustomFile = CurrentUserDir + "_CustomIndexFile" + ".doc";
            var i = model.CustomIndexDetail;

            //if User dir not exist then create User Dir
            bool exists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Documents\" + CurrentUserDir);

            if (!exists)
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Documents\" + CurrentUserDir);

            DirectoryInfo directory = new DirectoryInfo(BaseDir + CurrentUserDir);

            //create pdf file
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "My First PDF";
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            graph.DrawString(i[0], font,
                                XBrushes.Black, new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point),
                                XStringFormats.Center);
            string pdfFilename = BaseDir + CurrentUserDir + @"\" + NewCustomFile + ".pdf";
            pdf.Save(pdfFilename);

            //Delete all old directories
            string[] Directories = Directory.GetDirectories(BaseDir);

            foreach (string Dir in Directories)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(Dir);
                DateTime dt = Directory.GetCreationTime(Dir);
                if (DirInfo.CreationTime <= DateTime.Now.AddHours(-5))
                {
                    DirInfo.Delete(true);
                }
                DirInfo = null;
            }

            //Convert JPG to PDF files
            string dir = BaseDir + CurrentUserDir;
            List<string> myFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
      .Where(file => new string[] { ".jpeg", ".jpg", ".png" }
      .Contains(Path.GetExtension(file)))
      .ToList();

            foreach (string file in myFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                PdfDocument doc = new PdfDocument();
                doc.Pages.Add(new PdfPage());
                XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                XImage img = XImage.FromFile(file);
                xgr.DrawImage(img, 0, 0);
                doc.Save(BaseDir + CurrentUserDir + @"\" + fileName + ".pdf");
                doc.Close();
                doc.Dispose();
            }

            List<string> FinalBookletFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
      .Where(file => new string[] { ".PDF", ".pdf" }
      .Contains(Path.GetExtension(file)))
      .ToList();

            //Take all files from directory and send to model 
            //string[] Files = Directory.GetFiles(BaseDir + CurrentUserDir);

            FinalBookletModel bookletModelObj = new FinalBookletModel();
            List<FinalBookletPages> BhajanList = new List<FinalBookletPages>();

            bookletModelObj.SelectedDocuments = BhajanList;

            foreach (string filePath in FinalBookletFiles)
            {
                FinalBookletPages BhajanObj = new FinalBookletPages();
                string CurrentFileName = Path.GetFileName(filePath);
                BhajanObj.fileName = CurrentFileName;
                BhajanList.Add(BhajanObj);

            }
            ViewBag.FilePath = (@"\Documents\" + CurrentUserDir + "\\");

            return View("FinalBooklet", bookletModelObj);
        }
        public ActionResult FinalBooklet()
        {
            return View();
        }
        public ActionResult FinalBookletModel()
        {
            FinalBookletModel model = new FinalBookletModel();
            return View(model);
        }
        public ActionResult PrintDocuments(FinalBookletModel finalBookletModelObj)
        {
            string CurrentUserDir = Session["User"].ToString();
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\";

            var FinalsetupFiles = (from f in finalBookletModelObj.SelectedDocuments
                  .OrderBy(f => f.BhajanID)
                                   select f.fileName);

            //Combined doc
            PdfDocument outputDocument = new PdfDocument();
            foreach (string file in FinalsetupFiles)
            {
                string filePath = BaseDir + CurrentUserDir + @"\" + file;
                PdfDocument inputDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }
            // Save the document...
            string filename = BaseDir+CurrentUserDir+ @"\FinalBooklet.pdf";
            outputDocument.Save(filename);
            //
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=pdffile.pdf");
            Response.TransmitFile(Server.MapPath(@"~/Documents/" + CurrentUserDir + @"\FinalBooklet.pdf"));
            Response.End();
            return RedirectToAction("Index", "HomeController");
        }

    }
}