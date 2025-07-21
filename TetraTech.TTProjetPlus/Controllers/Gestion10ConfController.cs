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
    public class Gestion10ConfController : ControllerBase
    {
        public Gestion10ConfService _Gestion10ConfService = new Gestion10ConfService();        

        public static Gestion10ConfList Gestion10ConfItems = new Gestion10ConfList();

        // GET: Gestion10Conf
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
                ViewBag.displayRetour = displayRetour;
                ViewBag.MenuVisible = menuVisible;

                Gestion10ConfItems = _Gestion10ConfService.GetGestion10ConfList(UserService.CurrentUser.EmployeeId);

            // Manage10ConfMembersViewModel result = new ManageDisciplineMembersViewModel(CurrentBaseViewModel)
            //{
            //    DisciplineGroupsList = new List<SelectListItem>(),
            //    DiscDetails = new DisciplineMemberDetails(),
            //    Details = new EmployeeGroup()

            //};

                ViewBag.isAllowedGestion10Conf = Gestion10ConfItems.disciplineItem.Count > 0;

            return View(Gestion10ConfItems);
        }

        public JsonResult GetDetailsForSelectedGroup(string selectedGroup)
        {
            CommonModelGestion10Conf model = _Gestion10ConfService.GetDetailsForSelectedGroup(selectedGroup);
            
            string viewContent = ConvertViewToStringGestion10Conf("_DetailsGroupMembersGestion10Conf", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf, result3 = model.SelectedGestion10Conf}, JsonRequestBehavior.AllowGet);
        }

        private string ConvertViewToStringGestion10Conf(string viewName, object model)
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

        public ActionResult returnListEmployeeForGestion10Conf(string selectedGroup)
        {
            if (Session["ListOfPotentialGestion10ConfMembers"] == null)
            {
                Session["ListOfPotentialGestion10ConfMembers"] = _Gestion10ConfService.GetListEmployeeForGestion10Conf(selectedGroup);
            }

            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = Session["ListOfPotentialGestion10ConfMembers"];

            return PartialView("EmployeeListPartialGestion10Conf", model);
        }

        public ActionResult AddEmployeeToADGroup10Conf(string TTAccount, string discGroupID)
        {
            try
            {
                OperationStatus operationStatus = new OperationStatus { Status = false };
                              
                operationStatus = _Gestion10ConfService.AddEmployeeToGroupGestion10Conf(TTAccount, discGroupID);
                

                return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { resultFail = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}