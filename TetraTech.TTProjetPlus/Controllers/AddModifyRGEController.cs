using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class AddModifyRGEController : Controller
    {
        // GET: AjoutModificationRGE

        private readonly AddModifyRGEService _AddModifyRGE = new AddModifyRGEService();
        public static List<structureItem> ListeCompanies = new List<structureItem>();
        private readonly HomeService _HomeService = new HomeService();
        private readonly LocalBPRService _LocalBPRService = new LocalBPRService();


        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            ListeCompanies = _HomeService.GetORGs();
            //ViewBag.Companies = ListeCompanies.ToArray();
            AddModifyRGEModel modelRGE = new AddModifyRGEModel();
            modelRGE.ListAddModifyRGE = _AddModifyRGE.GetListeRGEInclureKM();
            return View(modelRGE);
            
        }

        public ActionResult returnListEmployeesInfos()
        {
            LocalBPRModel model = new LocalBPRModel();
            model.ListADTTLocalBPR = _LocalBPRService.GetListeADLocalBPR();
            return PartialView("EmployeesInfosPartial", model);
        }

        public ActionResult ModifiyInRGE(int id, string First_Name, string Employee_Number, string Organization, string Email_Address, string DateDebutProjet, string NoProjet)
        {
            var result = "";
            
            result = _AddModifyRGE.ModifyRGE(id, First_Name, Employee_Number, Organization, Email_Address, DateDebutProjet, NoProjet);

            
            return Json(result.Split('.'), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteInRGE(int id)
        {
            var result = "";
           
            result = _AddModifyRGE.DeleteInRGE(id);

            
            return Json(result.Split('.'), JsonRequestBehavior.AllowGet);
        }


        public ActionResult insertNewRGE(string First_Name, string Employee_Number, string Organization, string Email_Address, string DateDebutProjet, string NoProjet)
        {
            var result = "";            
            
            result = _AddModifyRGE.insertInRGE(First_Name, Employee_Number, Organization, Email_Address, DateDebutProjet, NoProjet);
            
           
            return Json(result.Split('.'), JsonRequestBehavior.AllowGet);
        }


    }
}