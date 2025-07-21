using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using System.IO;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class GestionGroupeADController : ControllerBase
    {
        public GestionGroupeADService _GestionGroupeADService = new GestionGroupeADService();        

        public static GestionGroupeADList GestionGroupeADItems = new GestionGroupeADList();

        // GET: GestionGroupeAD
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
                ViewBag.displayRetour = displayRetour;
                ViewBag.MenuVisible = menuVisible;

            GestionGroupeADItems = _GestionGroupeADService.GetGestionGroupeADList(UserService.CurrentUser.EmployeeId);
                
                ViewBag.isAllowedGestionGroupeAD = GestionGroupeADItems.disciplineItem.Count > 0;

            return View(GestionGroupeADItems);
        }

        public JsonResult GetDetailsForSelectedGroup(string selectedGroup)
        {
            CommonModelGestionGroupeAD model = _GestionGroupeADService.GetDetailsForSelectedGroup(selectedGroup);
            
            string viewContent = ConvertViewToStringGestionGroupeAD("_DetailsGroupMembersGestionGroupeAD", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf, result3 = model.SelectedGestionGroupeAD }, JsonRequestBehavior.AllowGet);
        }

        private string ConvertViewToStringGestionGroupeAD(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        public ActionResult returnListEmployeeForGestionGroupeAD(string selectedGroup)
        {
            if (Session["ListOfPotentialGestionGroupeADMembers"] == null)
            {
                Session["ListOfPotentialGestionGroupeADMembers"] = _GestionGroupeADService.GetListEmployeeForGestionGroupeAD(selectedGroup);
            }

            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = Session["ListOfPotentialGestionGroupeADMembers"];

            return PartialView("EmployeeListPartialGestionGroupeAD", model);
        }

        public ActionResult AddEmployeeToADGroupGroupeAD(string TTAccount, string discGroupID)
        {
            try
            {
                OperationStatus operationStatus = new OperationStatus { Status = false };
                              
                operationStatus = _GestionGroupeADService.AddEmployeeToGroupGestionGroupeAD(TTAccount, discGroupID);
                

                return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { resultFail = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}