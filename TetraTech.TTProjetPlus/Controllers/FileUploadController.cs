using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly FileUploadService _FLService = new FileUploadService();        

        // GET: FileUpload
        public ActionResult Index(string menuVisible = "true" )
        {
            FileUploadModel model = new FileUploadModel();            
            model .listFolders = _FLService.returnFolderList();
            model.listDocNames = _FLService.returnDocNameList();
            model.listeAllClient = _FLService.GetListCustomer();

            ViewBag.MenuVisible = menuVisible;
            if (Session["message"] != null && Session["message"].ToString() == "Success")
            {
               ViewBag.result = "Success";
            }
            else if(Session["message"] != null && Session["message"].ToString() == "Failed")
            {
                ViewBag.result = "Failed";
            }
            else if (Session["message"] != null && Session["message"].ToString() == "Exists")
            {
                ViewBag.result = "Exists";
               
            }
            Session["message"] = "";
            return View(model);
        }

        //https://stackoverflow.com/questions/37791462/mvc-application-that-uploads-a-users-file-to-the-server
        //https://www.aurigma.com/upload-suite/developers/aspnet-mvc/how-to-upload-files-in-aspnet-mvc

        public ActionResult UploadFile(string file, string repCode, string repID)
        {
            try
            {
                var filePath = @"C:\Users\Frederic.Ohnona\Documents\" + file;
                System.IO.File.Copy(filePath, @"\\tts349test04\D$\TTProjetPlus\TQEDocs\"+file);
               
                return Json("Transfer successful", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Transfer failed: "+ e.InnerException.Message, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult UploadFile2(HttpPostedFileBase file, string repCode, string subRepId, string customerName, string customerNumber, string continueProcess = "")
        //{
        //    try
        //    {
        //        BinaryReader b = new BinaryReader(file.InputStream);
        //        byte[] binData = b.ReadBytes((int)file.InputStream.Length);


        //        string[] itemsName = customerName.Split(',');
        //        string[] itemsNumber = customerNumber.Split(',');                

        //        //System.IO.File.WriteAllBytes(@"\\tts349test04\D$\TTProjetPlus\TQEDocs\"+ file.FileName, binData); //dev
        //        System.IO.File.WriteAllBytes(@"\\tqcs349iis1\D$\TTProjetPlus\TQEDocs\" + file.FileName, binData); // prod


        //        var rep_id = _FLService.returnRepId(repCode);
        //        var subRepCode = _FLService.returnSubRepCode(subRepId);
        //        var sub_RepId = Int32.Parse(subRepId);
        //        ///////////////////////////////////////////////////////////////////
        //        ///TRES IMPORTANT : pour prod changer C: par D: dans fullname///////////////////
        //        ///////////////////////////////////////////////////////////////////
        //        //var fullName = @"\\tts349test04\D$\TTProjetPlus\TQEDocs\" + file.FileName; //dev
        //        var fullName = @"\\tqcs349iis1\D$\TTProjetPlus\TQEDocs\" + file.FileName; //prod

        //        if (customerNumber.Length == 0 )
        //        {

        //            var insertStatus = _FLService.insertIntoTable(repCode, rep_id, subRepCode, sub_RepId, fullName, "NULL", "NULL");
        //        }
        //        else
        //        {
        //            for (int i = 0; i < itemsNumber.Length; i++)
        //            {
        //                var insertStatus = _FLService.insertIntoTable(repCode, rep_id, subRepCode, sub_RepId, fullName, itemsName[i].ToString(), itemsNumber[i].ToString());

        //            }
        //        }

        //        Session["message"] = "Success";
        //        return Json("success", JsonRequestBehavior.AllowGet);
        //        //var insertStatus =  _FLService.insertIntoTable(repCode, rep_id, subRepCode, sub_RepId, fullName, customerName);
        //        //Session["message"]  = "Success";
        //        //return Json("success", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        Session["message"] = "Failed";
        //        return Json("failed", JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult UploadFile2()
        {
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                return Json("Failed", JsonRequestBehavior.AllowGet);

            var file = Request.Files[0];

            try
            {
                // Read other parameters
                string repCode = Request.Form["repCode"];
                string subRepId = Request.Form["subRepId"];
                string customerName = Request.Form["customerName"];
                string customerNumber = Request.Form["customerNumber"];
                string continueProcess = Request.Form["continueProcess"];

                // Folder + file saving
                string targetFolder = @"\\tqcs349iis1\D$\TTProjetPlus\TQEDocs";
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                string safeFileName = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(targetFolder, safeFileName);

                file.SaveAs(fullPath);

                // Split customer info
                string[] itemsName = string.IsNullOrEmpty(customerName) ? new string[0] : customerName.Split(',');
                string[] itemsNumber = string.IsNullOrEmpty(customerNumber) ? new string[0] : customerNumber.Split(',');

                int sub_RepId = int.TryParse(subRepId, out var temp) ? temp : 0;
                var rep_id = _FLService.returnRepId(repCode);
                var subRepCode = _FLService.returnSubRepCode(subRepId);

                // Insert records into DB
                if (itemsNumber.Length == 0)
                {
                    _FLService.insertIntoTable(repCode, rep_id, subRepCode, sub_RepId, fullPath, "NULL", "NULL");
                }
                else
                {
                    for (int i = 0; i < itemsNumber.Length && i < itemsName.Length; i++)
                    {
                        _FLService.insertIntoTable(repCode, rep_id, subRepCode, sub_RepId, fullPath, itemsName[i], itemsNumber[i]);
                    }
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult returnSubFolders(string repCode)
        {
            var list = _FLService.GetSubFolders(repCode);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        // public JsonResult checkIfFilexists(HttpPostedFileBase file)
        // {
        //     //string path = @"\\tts349test04\D$\TTProjetPlus\TQEDocs\" + file.FileName; //dev
        //     string path = @"\\tqcs349iis1\D$\TTProjetPlus\TQEDocs\" + file.FileName; //prod
        //     try
        //     {
        //     if (System.IO.File.Exists(path))
        //     {
        //         return Json("Exists", JsonRequestBehavior.AllowGet);
        //     }
        //     else
        //     return Json("NotExisting", JsonRequestBehavior.AllowGet);

        //     }
        //     catch (Exception e)
        //     {
        //         return Json("NotExisting", JsonRequestBehavior.AllowGet);
        //     }
        // }


        [HttpPost]
        public JsonResult checkIfFilexists(string fileName)
        {
            string path = @"\\tqcs349iis1\D$\TTProjetPlus\TQEDocs\" + fileName;

            try
            {
                if (System.IO.File.Exists(path))
                    return Json("Exists", JsonRequestBehavior.AllowGet);
                else
                    return Json("NotExisting", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("NotExisting", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult returnDocNames(string repCode, string repSubCode)
        {
            var repId = _FLService.returnRepId(repCode);
            var sousRepId = Int32.Parse(repSubCode);
            var list = _FLService.GetDocList(repId, sousRepId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }




       public JsonResult removeDocFromDBandServer(string repCode, string repSubCode, string nomDoc, string customerNumber)
        {
            var repId = _FLService.returnRepId(repCode);
            var sousRepId = Int32.Parse(repSubCode);
            var result = _FLService.removeDocFromDB(repId, sousRepId, nomDoc, customerNumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
          

        public JsonResult finalRemovingOfForm(string nomDoc)
        {
            var result = _FLService.removeDocFromDB_Final(nomDoc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}