using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class SecurityProjectEquivalenceController : Controller
    {
        // GET: SecurityProjectEquivalence

        private readonly SecurityProjectEquivalenceService _SecurityProjectEquivalenceService = new SecurityProjectEquivalenceService();

        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            SecurityProjectEquivalenceModel model = new SecurityProjectEquivalenceModel();
            model.ListSecurityProjectEquivalence  = _SecurityProjectEquivalenceService.GetListeSecurityProjectEquivalence();
            return View(model);

        }


        public ActionResult insertModifiyInProject(string type, string TTprojetID, string TlinxProjetID)
        {
            var result = "";
            if (type == "insert")
            {
                result = _SecurityProjectEquivalenceService.insertInProject(TTprojetID, TlinxProjetID);
            }
            else if (type == "modify")
            {
                result = _SecurityProjectEquivalenceService.ModifyInProject(TTprojetID, TlinxProjetID);

            }
            else if (type == "delete")
            {
                result = _SecurityProjectEquivalenceService.DeleteInProject(TTprojetID);

            }
            return Json(result.Split('.'), JsonRequestBehavior.AllowGet);
        }


    }
}