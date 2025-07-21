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
    public class GestionProjetSecuriserController : ControllerBase
    {
        public GestionProjetSecuriserService _GestionProjetSecuriserService = new GestionProjetSecuriserService();        

        public static GestionProjetSecuriserList GestionProjetSecuriserItems = new GestionProjetSecuriserList();

        
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
                ViewBag.displayRetour = displayRetour;
                ViewBag.MenuVisible = menuVisible;

            GestionProjetSecuriserItems = _GestionProjetSecuriserService.GetGestionProjetSecuriserList(UserService.CurrentUser.EmployeeId, UserService.CurrentUser.DisplayName);

           
                ViewBag.isAllowedGestionProjetSecuriser = _GestionProjetSecuriserService.GetSecurityProjetSecuriser(UserService.CurrentUser.EmployeeId);
            
            return View(GestionProjetSecuriserItems);
        }

        public JsonResult GetDetailsForSelectedGroup(string selectedGroup)
        {
            CommonModelGestionProjetSecuriser model = _GestionProjetSecuriserService.GetDetailsForSelectedGroup(selectedGroup);
            
            string viewContent = ConvertViewToStringGestionProjetSecuriser("_DetailsGroupMembersGestionProjetSecuriser", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf, result3 = model.SelectedGestionProjetSecuriser}, JsonRequestBehavior.AllowGet);
        }

        private string ConvertViewToStringGestionProjetSecuriser(string viewName, object model)
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

        public ActionResult returnListEmployeeForGestionProjetSecuriser(string selectedGroup)
        {
            if (Session["ListOfPotentialGestionProjetSecuriserMembers"] == null)
            {
                Session["ListOfPotentialGestionProjetSecuriserMembers"] = _GestionProjetSecuriserService.GetListEmployeeForGestionProjetSecuriser(selectedGroup);
            }

            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = Session["ListOfPotentialGestionProjetSecuriserMembers"];

            return PartialView("EmployeeListPartialGestionProjetSecuriser", model);
        }

        public ActionResult AddEmployeeToADGroupProjetSecuriser(string TTAccount, string discGroupID)
        {
            try
            {
                OperationStatus operationStatus = new OperationStatus { Status = false };
                              
                operationStatus = _GestionProjetSecuriserService.AddEmployeeToGroupGestionProjetSecuriser(TTAccount, discGroupID);
                

                return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { resultFail = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}