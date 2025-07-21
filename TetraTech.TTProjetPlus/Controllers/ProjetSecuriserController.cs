using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Data;


namespace TetraTech.TTProjetPlus.Controllers
{
    public class ProjetSecuriserController : Controller
    {
        // GET: Customer

        public ProjetSecuriserService _ProjetSecuriserService = new ProjetSecuriserService();
        

        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            ProjetSecuriserModel model = new ProjetSecuriserModel();
            model.listProjetSecuriser = _ProjetSecuriserService.returnProjetSecuriserList();
            model.listeEmployeeActif_Inactive = _ProjetSecuriserService.returnListeEmployeeActif_Inactive();
            model.listProjetASecuriserStatut = _ProjetSecuriserService.returnlistProjetASecuriserStatut();
            return View(model);
        }
             
        public ActionResult InsertProject(string Projet, string Repertoire, string GroupeAD, string Chemin, string ListeUsers, string ListeUsersNames)
        {
            try
            {
                var result = _ProjetSecuriserService.InsertIntoProjetSecuriserTable(Projet, Repertoire, GroupeAD, Chemin, ListeUsers, ListeUsersNames);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult UpdateProject(string id, string Projet, string Repertoire, string GroupeAD, string Chemin, string ListeUsers, string ListeUsersNames)
        {
            try
            {
                var result = _ProjetSecuriserService.UpdateProjetSecuriserTable(id, Projet, Repertoire, GroupeAD, Chemin, ListeUsers, ListeUsersNames);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult submitModifierStatutProjectASecuriser(string Projet)
        {
            try
            {
                var result = _ProjetSecuriserService.ChangeProjectStatutSecuriser(Projet);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }



    }
}