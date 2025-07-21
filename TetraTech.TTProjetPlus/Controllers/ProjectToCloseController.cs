using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;
using ProjectToCloseRessources = TetraTech.TTProjetPlus.Views.ProjectToClose.ProjectToCloseRessources;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ProjectToCloseController : ControllerBase
    {
        private readonly ProjectToCloseService _projectToCloseService = new ProjectToCloseService();
        public static List<SelectItem> ListeProjectForUser = new List<SelectItem>();
        public static List<SelectItem> ListeProjectForUserAll = new List<SelectItem>();
        public static string pageVisible = "true"; //pour empecher le reload de la apge lorsque l on est dans TTlx+

        public ActionResult Index(string tlxprojectnumber, string menuVisible = "true")
        {
            if (menuVisible == "false")
            {
                Session["lang"] = "Fr";
            }
            Session["pageVisible"] = menuVisible;
            ProjectToCloseViewModel result = _projectToCloseService
                .GetProjectToCloseViewModel(CurrentBaseViewModel, CurrentUser.EmployeeId, tlxprojectnumber, menuVisible);

            ViewBag.ProjectNumberCalled = (!string.IsNullOrEmpty(tlxprojectnumber)) ? "true" : "false";
            ViewBag.MenuVisible = menuVisible;
            ListeProjectForUser = GetProjectToCloseForUser2();
            ListeProjectForUserAll = GetProjectToCloseForUser2("no", "", "");

            return View("Index", result);
        }

        public ActionResult Index2(string tlxprojectnumber, string menuVisible = "false")
        {
            if (menuVisible == "false")
            {
                Session["lang"] = "Fr";
            }
            Session["pageVisible"] = menuVisible;
            ProjectToCloseViewModel result = _projectToCloseService
                .GetProjectToCloseViewModel(CurrentBaseViewModel, CurrentUser.EmployeeId, tlxprojectnumber, menuVisible);

            ViewBag.ProjectNumberCalled = (!string.IsNullOrEmpty(tlxprojectnumber)) ? "true" : "false";
            ViewBag.MenuVisible = menuVisible;
            ListeProjectForUser = GetProjectToCloseForUser2();
            ListeProjectForUserAll = GetProjectToCloseForUser2("no", "", "");

            return View("Index", result);
        }

        [HttpPost]
        public ActionResult ProjectDetails(int osProjectID, string menuVisible)
        {
            ProjectToCloseModel details = _projectToCloseService.GetProjectToCloseDetais(CurrentUser.EmployeeId, osProjectID, menuVisible);

            if (details.HasAccess)
                return PartialView("_ProjectDetails", details);

            else
                return Json(new { notaproject = details.AccessType, message = details.AccessMessage }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult SaveProjectToClose(ProjectToCloseModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //pour envoie de mail si la date de fin estimée a changé:
                    var currentEstimatedEndDate = _projectToCloseService.returnCurrentEstimatedEndDate(data.ProjectToCloseId);

                    var result = new { osProjectID = data.OSProjectID, success = false, message = string.Empty };

                    TetraTechUser currentuser = CurrentUser;
                    bool sendRequestCloseProject = false;
                    _projectToCloseService.SaveProjectToClose(currentuser.EmployeeId, currentuser.FullName, data, out sendRequestCloseProject);

                    if (sendRequestCloseProject)
                    {

                        int emailID = _projectToCloseService.SendProject2CloseEmail(data.OSProjectID);

                        if (emailID > 0)
                            result = new { osProjectID = data.OSProjectID, success = true, message = Resources.TTProjetPlusResource.SavedSuccessfully };

                        else
                            result = new { osProjectID = data.OSProjectID, success = false, message = Resources.TTProjetPlusResource.SendMailErrorMessage };

                    }


                    else
                    {

                        if (currentEstimatedEndDate == "-1" || (currentEstimatedEndDate != "-1" && currentEstimatedEndDate != data.EstimateEndDateString))
                        {
                            int emailID = _projectToCloseService.SendProjectEstimatedEndDate(data.OSProjectID);

                            if (emailID > 0)
                                result = new { osProjectID = data.OSProjectID, success = true, message = Resources.TTProjetPlusResource.SavedSuccessfully };

                            else
                                result = new { osProjectID = data.OSProjectID, success = false, message = Resources.TTProjetPlusResource.SendMailErrorMessage };


                        }

                        else
                            result = new { osProjectID = data.OSProjectID, success = true, message = Resources.TTProjetPlusResource.SavedSuccessfully };

                    }


                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }

            return PartialView("_ActiveProjectToClose", data);
        }

        public List<SelectItem> GetProjectToCloseForUser2(string active = "yes", string filter = "", string category = "")
        {
            return _projectToCloseService.GetProjectToCloseForUser(CurrentUser.EmployeeId, active, filter, category, -1);

        }

        public JsonResult GetProjectToCloseForUser3(string active = "no", string filter = "", string category = "")
        {

            var list = _projectToCloseService.GetProjectToCloseForUser(CurrentUser.EmployeeId, active, filter, category, -1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectToCloseForUseByCategory(string active = "no", string filter = "", string category = "")
        {
            string color = "";
            List<SelectItem> Liste = new List<SelectItem>();
            switch (category)
            {
                case "blanc":
                    color = "#ffffff";
                    break;
                case "vert":
                    color = "#32CD32";
                    break;
                case "jaune":
                    color = "#FFA500";
                    break;
                case "gris":
                    color = "#999999";
                    break;
                case "rouge":
                    color = "#ff6666";
                    break;
                case "":
                    color = "";
                    break;
            }
            if (active == "no")
            {
                if (color != "")
                {
                    Liste = ListeProjectForUserAll.FindAll(p => p.Color == color);

                }
                else Liste = ListeProjectForUserAll;
            }

            else
            {
                if (color != "")
                {
                    Liste = ListeProjectForUser.FindAll(p => p.Color == color);

                }
                else Liste = ListeProjectForUser;
            }
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult returnIdForProjectFromTTLinx(string TlinxProjectNumber)
        {
            var id = _projectToCloseService.returnIdForProjectFromTTLinx(TlinxProjectNumber);
            return Json(new { idReturn = id }, JsonRequestBehavior.AllowGet);
        }
    }
}
