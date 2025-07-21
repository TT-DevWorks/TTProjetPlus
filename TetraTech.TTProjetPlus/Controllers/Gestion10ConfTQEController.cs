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
    public class Gestion10ConfTQEController : ControllerBase
    {
        public Gestion10ConfTQEService _Gestion10ConfTQEService = new Gestion10ConfTQEService();        

        public static Gestion10ConfTQEList Gestion10ConfTQEItems = new Gestion10ConfTQEList();

        // GET: Gestion10Conf
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
                ViewBag.displayRetour = displayRetour;
                ViewBag.MenuVisible = menuVisible;

                Gestion10ConfTQEItems = _Gestion10ConfTQEService.GetGestion10ConfTQEList(UserService.CurrentUser.EmployeeId);

           
                ViewBag.isAllowedGestion10ConfTQE = Gestion10ConfTQEItems.disciplineItem.Count > 0;
                //ViewBag.lang = Session["lang"].ToString().ToLower();

            return View(Gestion10ConfTQEItems);
        }

        public JsonResult GetDetailsForSelectedGroupTQE(string selectedGroup)
        {
            CommonModelGestion10ConfTQE model = _Gestion10ConfTQEService.GetDetailsForSelectedGroup(selectedGroup);
            
            string viewContent = ConvertViewToStringGestion10ConfTQE("_DetailsGroupMembersGestion10ConfTQE", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf, result3 = model.SelectedGestion10ConfTQE}, JsonRequestBehavior.AllowGet);
        }

        private string ConvertViewToStringGestion10ConfTQE(string viewName, object model)
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

        public ActionResult returnListEmployeeForGestion10ConfTQE(string selectedGroup)
        {
            if (Session["ListOfPotentialGestion10ConfTQEMembers"] == null)
            {
                Session["ListOfPotentialGestion10ConfTQEMembers"] = _Gestion10ConfTQEService.GetListEmployeeForGestion10ConfTQE(selectedGroup);
            }

            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = Session["ListOfPotentialGestion10ConfTQEMembers"];

            return PartialView("EmployeeListPartialGestion10ConfTQE", model);
        }

        public ActionResult AddEmployeeToADGroup10ConfTQE(string TTAccount, string discGroupID)
        {
            try
            {
                OperationStatus operationStatus = new OperationStatus { Status = false };
                              
                operationStatus = _Gestion10ConfTQEService.AddEmployeeToGroupGestion10ConfTQE(TTAccount, discGroupID);
                

                return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { resultFail = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}