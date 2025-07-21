using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class LitigeController : Controller
    {
        private readonly LitigeService _LitigeService = new LitigeService();

        // GET: Ltige
        public ActionResult Index(string menuVisible = "true")
        {
            DirectoryService obj = new DirectoryService();
            ViewBag.isAllowedAdministration = obj.isAllowedAdministration();
            ViewBag.MenuVisible = menuVisible;
            LitigeModel model = new LitigeModel();
            model.projectListe = _LitigeService.returnLitigeProjects();
            return View(model);
        }

        public ActionResult insertEditInLitige(string type, string projectNumber, string dateAdded, string comment, string closedFile)
        {
            var result = "";
            if (type =="insert")
            {
                result = _LitigeService.insertInLitige(projectNumber, dateAdded, comment, closedFile);
            }
            else if(type == "edit")
            {
                result = _LitigeService.editInLitige(projectNumber, dateAdded, comment, closedFile);

            }
            return Json(result.Split('.') , JsonRequestBehavior.AllowGet);
        }
    }
}