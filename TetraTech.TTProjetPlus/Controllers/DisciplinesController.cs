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
    public class DisciplinesController : ControllerBase
    {
         public DisciplinesService _DisciplineService = new DisciplinesService();

        public static List<usp_getDisciplineAdminList_Result> ListeAdmins = new List<usp_getDisciplineAdminList_Result>();

        public static DisciplineDocProj disciplineItems = new DisciplineDocProj();

        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
            ViewBag.displayRetour = displayRetour;
            ViewBag.MenuVisible = menuVisible;
            
            disciplineItems = _DisciplineService.GetDisciplineList(UserService.CurrentUser.EmployeeId);

            ManageDisciplineMembersViewModel result = new ManageDisciplineMembersViewModel(CurrentBaseViewModel)
            {
                DisciplineGroupsList = new List<SelectListItem>(),
                DiscDetails = new DisciplineMemberDetails(),
                Details = new EmployeeGroup()

            };

            ViewBag.isAllowedDiscipline = disciplineItems.disciplineItem.Count > 0 || disciplineItems.docProjItem.Count > 0;
            ListeAdmins = _DisciplineService.GetDisciplineAdminList();
            return View(result);
        }

        public JsonResult GetDetailsForSelectedDiscipline(string selectedDiscipline)
        {
            CommonModel model = _DisciplineService.GetDetailsForSelectedDiscipline(selectedDiscipline);

            string viewContent = ConvertViewToString("_DetailsGroupMembers", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsForSelectedDocProj(string selectedDocProj)
        {
            CommonModel model = _DisciplineService.GetDetailsForSelectedDocProj(selectedDocProj);

            string viewContent = ConvertViewToString("_DetailsGroupMembers", model);
            return Json(new { result = viewContent, result2 = model.AvailableToEmployeesMembersOf }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnListEmployeeForDocProj(string selectedDocProj)
        {
            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = _DisciplineService.GetListEmployeeForDocProj(selectedDocProj);

            return PartialView("EmployeeListPartial", model);
        }

        public ActionResult returnListEmployeeForDiscipline(string selectedDiscipline)
        {            
            if (Session["ListOfPotentialDisciplineMembers"] == null)
            {
                Session["ListOfPotentialDisciplineMembers"] = _DisciplineService.GetListEmployeeForDiscipline(selectedDiscipline);
            }

            ViewBag.lang = Session["lang"].ToString().ToLower();

            var model = Session["ListOfPotentialDisciplineMembers"];

            return PartialView("EmployeeListPartial", model);
        }

        public ActionResult ListeAdmin(string displayRetourLM = "true", string menuVisible = "true")
        {
            ViewBag.displayRetourLM = displayRetourLM;
            ViewBag.MenuVisible = menuVisible;
            var result = _DisciplineService.GetDisciplineAdminList();
            ListeAdmins = result;
            return View();
        }

        public JsonResult returnLists()
        {
            var listDiscipline = _DisciplineService.GetDisciplineList(UserService.CurrentUser.EmployeeId);
            var listDocProj = _DisciplineService.GetDocProjList(UserService.CurrentUser.EmployeeId);
            return Json(new { ListDisc = listDiscipline, ListDocProj = listDocProj }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EmployeeGroup(string selectedGroup, string groupType, OperationStatus operationStatus)
        {
            return PartialView("_AddEmployeeToGroup", new EmployeeGroup { SelectedGroup = selectedGroup, GroupType = groupType, OperationStatus = operationStatus });
        }

        public ActionResult AddEmployeeToADGroup(string groupSelected, string TTAccount, string discDocProjId)
        {
            try
            {
                OperationStatus operationStatus = new OperationStatus { Status = false };

                if (groupSelected == "DocProj")
                {
                    operationStatus = _DisciplineService.AddEmployeeToDocProjGroup(TTAccount, discDocProjId);
                }
                else
                {
                    operationStatus = _DisciplineService.AddEmployeeToDisciplineGroup(TTAccount, discDocProjId);
                }

                return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { resultFail = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult RemoveEmployeeFromGroup(string SelectedGroup, string SelectedGroupToRemove, string SelectedEmployeeNumber, string DiscOrDocProjID)
        {
            OperationStatus operationStatus = new OperationStatus { Status = false };

            try
            {
                operationStatus = _DisciplineService.RemoveEmployeeFromGroup(SelectedGroup, SelectedGroupToRemove,  SelectedEmployeeNumber, DiscOrDocProjID);
            }
            catch (Exception ex)
            {
                return Json(new { resultFail = "failed", details = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "Success", details = operationStatus.Message }, JsonRequestBehavior.AllowGet);
        }

        private string ConvertViewToString(string viewName, object model)
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
    }
}