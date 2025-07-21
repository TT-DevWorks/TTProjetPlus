using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{

    public class InfoProjetController : ControllerBase
    {
        private readonly DashBoardService _DashboardService = new DashBoardService();
        private readonly HomeService _HomeService = new HomeService();
        private readonly InfoProjetService _InfoProjetService = new InfoProjetService();

        public static List<structureItem> ListeNumerosProjet = new List<structureItem>();
        public string sFilePath = HttpRuntime.AppDomainAppPath;
        // GET: InfoProjet	
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            SearchCriteriasModel model = _InfoProjetService.returnSearchCriterias();
            return View(model);
        }
        //public ActionResult IndexDT(bool notIncludeInactiveOffers, bool notIncludeInactiveProjects)	//, bool includeInactiveProjects = false
        //{
        //    ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();
        //    Session["offerCheck"] = notIncludeInactiveOffers;
        //    Session["projectCheck"] = notIncludeInactiveProjects;
        //    ListOfProjectsAndOS List_Projects_offers = new ListOfProjectsAndOS();
        //    List_Projects_offers.ProjectsList = _HomeService.getProjectList(notIncludeInactiveProjects);
        //    List_Projects_offers.OffersList = _HomeService.getOffersList(notIncludeInactiveOffers);
        //    return PartialView("IndexDT", List_Projects_offers);
        //}

        public ActionResult submitSearch(
                            string ProjectNumbers = "",
                            string ProjectDescription = "",
                            string CustomersNumber = "",
                            string CustomersName = "",
                            string FinalClient = "",
                            string AcctCenterId = "",
                            string InitiatedBy = "",
                            string ProgramMgrNumber = "",
                            string ProjectMgrNumber = "",
                            string BillSpecialistNumber = "",
                            string StatusID = "",
                            string SubmittedDate = "",
                            string Archivedate = "",
                            string DocPostponedDate = "",
                            string DocStatutID = "",
                            string archiveParam = "no",
                            string comments = "")
        {
            var listeProjet = _InfoProjetService.returnListProjets(
                                ProjectNumbers,
                                ProjectDescription,
                                CustomersNumber,
                                CustomersName,
                                FinalClient,
                                AcctCenterId,
                                InitiatedBy,
                                ProgramMgrNumber,
                                ProjectMgrNumber,
                                BillSpecialistNumber,
                                StatusID,
                                SubmittedDate,
                                Archivedate,
                                DocPostponedDate,
                                DocStatutID,
                                archiveParam,
                                comments
                            );
            return PartialView("_searchProjectResultPartial", listeProjet);
        }
        //a mettre dans un service	
        public List<OfferProject> Read()
        {
            var res = new List<OfferProject>();

            return res;
        }
        public ActionResult convertOfferToProject(string title = "", string projectNumber = "", string masterProject = "", string codePhase = "",
            int clientNumber = 0, string clientName = "", string company = "", string ua = "", string org = "", string projectManager = ""
            , string finalClient = "", string contactClient = "", int approximateAmount = 0)
        {
            OfferProject model = new OfferProject();
            model.Title = title;
            model.ProjectNumber = projectNumber;
            model.MasterProject = masterProject;
            model.CodePhase = codePhase;
            model.ClientNumber = clientNumber;
            model.ClientName = clientName;
            model.Company = company;
            model.BussinessUnit = ua;
            model.ORG = org;
            model.ProjectManager = projectManager;
            model.FinalClient = finalClient;
            model.ContactClient = contactClient;
            model.ApproximateAmount = approximateAmount;
            return PartialView("convertOfferToProject", model);
        }


        public ActionResult createBatchFile(string folderLocation)
        {
            try
            {
                var sFileName = "TTProjetPlusRepertoire.bat";
                
                using (StreamWriter sw = new StreamWriter(Path.Combine(sFilePath, sFileName)))
                {                
                    sw.WriteLine("@echo off");
                    sw.WriteLine("%SystemRoot%\\explorer.exe " + folderLocation);
                    sw.WriteLine(":End");
                    sw.WriteLine("");
                    sw.Close();

                }

                string contentType = "text/plain";
                return File(sFilePath + "\\" + sFileName, contentType, "TTProjetPlusRepertoire.bat");

            }
            catch (Exception e)
            {
                return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult isFolderExists(string folderLocation)
        {
            try
            {
                Boolean exists = false;

                if (Directory.Exists(folderLocation))
                {
                    exists = true;
                }

                return Json(new { result = exists }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = "failed", error = e.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}