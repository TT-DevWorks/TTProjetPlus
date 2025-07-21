using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{


    public class TTProjetController : ControllerBase
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly TTProjetMenuService _TTMService = new TTProjetMenuService();

        public static List<structureItem> ListeProjets = new List<structureItem>();
        public static List<structureItem> ListeProjetsForArb = new List<structureItem>();
        public static List<structureItem> ListeSecurityModelsTT = new List<structureItem>();
        public static List<structureItem> ListeSelectionModelsTT = new List<structureItem>();
        public static List<structureItem> ListeServers = new List<structureItem>();
        public static List<structureItem> ListeModels = new List<structureItem>();
        public static List<string> ListeFinalProjects = new List<string>();
        // public static string user = "";
        public static List<string> ListeProjetAvecStrucDeRepertoires = new List<string>();
        public static List<string> ListeProjectcNumbercInTTProjet = new List<string>();

        public  List<string>listeProjetTQE = new List<string>();
        public List<string> listeProjetTQE_titre = new List<string>();
        public List<string> listeProjetTQE_CP = new List<string>();
        public List<string> listeProjetTQE_DP = new List<string>();
        public List<string> listeProjetTQE_CP_email = new List<string>();
        public List<string> listeProjetTQE_client_email = new List<string>();

        public List<string> listeProjetQIB = new List<string>();
        public List<string> listeProjetQIB_titre = new List<string>();
        public List<string> listeProjetQIB_CP = new List<string>();
        public List<string> listeProjetQIB_DP = new List<string>();
        public List<string> listeProjetQIB_CP_email = new List<string>();
        public List<string> listeProjetQIB_client_email = new List<string>();

        // GET: TTProjet
        public ActionResult Index(string selectedPanel, string menuVisible = "true")
        {
            // Stockez la valeur dans ViewData ou ViewBag
            ViewBag.SelectedPanel = selectedPanel;

            var browser = System.Web.HttpContext.Current.Request.Browser;
            if (browser.Browser == "InternetExplorer")
            {
                ViewBag.IE = "true";
            }

            ViewBag.successs = "";

            Session["isTQEModelException"] = UserService.isTQEModelException();
            Session["isProductionMode"] = UserService.isPRODEnvironment();

            ListeProjets = renderList("GetProjects");
            ListeModels = renderList("GetModeleName");
            //    user = "TT\\" + (UserService.CurrentUser).ToString().Replace(" ", ".");
            ViewBag.MenuVisible = menuVisible;
            ViewBag.isAdminTI = _TTMService.returnUserSatus(UserService.CurrentUser.EmployeeId);
            ViewBag.isAdminInfo = _TTMService.returnAdditionalInfoPermission(UserService.CurrentUser.EmployeeId);
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Session["lang"] = lang;
            return RedirectToAction("Index");
        }

        public ActionResult Index2()
        {

            return View();
        }

        public JsonResult returnListProjet()
        {

            var list = renderList("GetProjects");
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        private List<string> getProjetAvecStrucDeRepertoires()
        {

            var liste = _TTMService.getProjetAvecStrucDeRepertoires();
            return liste;
        }

        private List<string> getProjectsNumbersinTTProjet()
        {

            var liste = _TTMService.getProjectsNumbersinTTProjet();
            return liste;
        }


        public JsonResult returnModeleSelection(string modelId)
        {
            var user = UserService.CurrentUser.TTAccount;
            var list = _HomeService.GetModeleSelectionForModelId(user, Int32.Parse(modelId));
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public JsonResult returnModeleSecurite(string modelId)
        {
            var list = _TTMService.GetModeleSecuriteForModelId(Int32.Parse(modelId));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnListServer()
        {
            var list = renderList("GetServerName");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnListServerFromTTProjetFolderStructureView()
        {
            var list = renderList("GetServerNameFromTTProjetFolderStructureView");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnListServerModel()
        {
            var list = renderList("GetModeleName");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnListServerModelFromTTProjetFolderStructureView()
        {
            var list = renderList("GetModeleNameFromTTProjetFolderStructureView");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TTProjetFolderStructure(string displayRetour = "true", string menuVisible = "true")
        {
            ListeProjets = renderList("GetProjects");
            ViewBag.displayRetour = displayRetour;
            DirectoryService obj = new DirectoryService();
            ViewBag.isAllowedAdvanced = obj.isAllowedAdvanced();
            ViewBag.MenuVisible = menuVisible;
            return View();
        }

        public List<structureItem> renderList(string methodName)
        {
            List<structureItem> liste = new List<structureItem>();

            switch (methodName)
            {
                case "GetServerName":
                    if ((Session["isProductionMode"] == null ? 1 : (int)Session["isProductionMode"]) == 1)
                    {
                        liste = _HomeService.GetServerName();
                    }
                    else
                    {
                        liste = _HomeService.GetTestServerName();
                    }
                    break;

                case "GetServerNameFromTTProjetFolderStructureView":
                    //if ((Session["isProductionMode"] == null ? 1 : (int)Session["isProductionMode"]) == 1)
                    //{
                    //    if ((Session["isTQEModelException"] == null ? 0 : (int)Session["isTQEModelException"]) == 1)
                    //    {
                    //        liste = _HomeService.GetTQEExceptionServerName();
                    //    }
                    //    else
                    //    {
                    //        liste = _HomeService.GetServerName();
                    //    }
                    //}
                    //else
                    //{
                    //    liste = _HomeService.GetTestServerName();
                    //}


                    liste = _HomeService.GetServerName();

                    break;

                case "GetProjects":
                    liste = _HomeService.GetProjects();
                    break;
                case "GetProjectsForArb":
                    liste = _HomeService.GetProjectsForArb();
                    break;

                case "GetModeleName":
                    liste = _HomeService.GetModeleName();
                    break;

                case "GetModeleNameFromTTProjetFolderStructureView":
                    if ((Session["isTQEModelException"] == null ? 0 : (int)Session["isTQEModelException"]) == 1)
                    {
                        liste = _HomeService.GetTQEExceptionModeleName();
                    }
                    else
                    {
                        liste = _HomeService.GetModeleName();
                    }

                    break;

                case "GetModeleSecurite":
                    liste = _HomeService.GetModeleSecurite();
                    break;

                case "GetModeleSelection":
                    liste = _HomeService.GetModeleSelection();
                    break;

                case "GetUAs":
                    liste = _HomeService.GetUAs();
                    break;

                case "GetORGs":
                    liste = _HomeService.GetORGs();
                    break;
                case "GetCompanies":
                    liste = _HomeService.GetCompanies();
                    break;
                case "ProjectManagers":
                    liste = _HomeService.GetProjectManagers2();
                    break;
            }


            return liste;
        }

        public ActionResult SubmitFormAddSDR(
                   string projetNumber,
                   string serverName,
                   string modID,
                   string modeleSecurite,
                   string modeleSelection,
                   string customername,
                   bool proposal

            )
        {
            // var successAddProject = _BLLService.AddProjectInTTProjetForExistingProject(projetNumber, serverName, modID, modeleSecurite, modeleSelection);



            try
            {
                var successAddProject = _HomeService.AddProjectInTTProjet(projetNumber, Int32.Parse(modeleSecurite), Int32.Parse(serverName), modID, 0, UserService.CurrentUser.EmployeeId, customername, null, 1, false, modeleSelection);


                if (successAddProject == "successfullProcess")
                {
                    ListeProjets = renderList("GetProjects");

                    structureItem result = new structureItem();
                    result.Value = "successfullProcess";
                    return Json(new { result = result.Value }, JsonRequestBehavior.AllowGet);
                }
                else if (successAddProject == "failedIsertionInBPR")
                {
                    return Json(new { result = "failedIsertionInBPR" }, JsonRequestBehavior.AllowGet);
                }
                else if (successAddProject == "failedAdditionFolder")
                {
                    return Json(new { result = "failedAdditionFolder" }, JsonRequestBehavior.AllowGet);
                }

                else if (successAddProject == "failedApplySecurity")
                {
                    return Json(new { result = "failedApplySecurity" }, JsonRequestBehavior.AllowGet);
                }
                else if (successAddProject == "failedCreateFolderOnServer")
                {
                    return Json(new { result = "failedCreateFolderOnServer" }, JsonRequestBehavior.AllowGet);
                }
                else if (successAddProject == "alreadyExistingProject")
                {
                    return Json(new { result = "alreadyExistingProject" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "TTProjetController", "SubmitFormAddSDR", projetNumber);
                return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);

            }


        }

        public int getModIdForProjectId(string pro_id)
        {
            return _TTMService.getModSecIdForProjecctNumber(pro_id);
        }

        public ActionResult FolderStructureForProject(string displayRetourP = "true", string menuVisible = "true")
        {
            ListeProjetsForArb = renderList("GetProjectsForArb");
            ViewBag.displayRetourP = displayRetourP;
            ViewBag.MenuVisible = menuVisible;

            return View();
        }

        public ActionResult ArchiveProject()
        {
            return View();
        }

        public ActionResult RapportSatisfactionClient(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            return View();
        }

        public ActionResult SatisfactionClient(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            ViewBag.isAllowedSatisfactionClient = _HomeService.GetSecuritySatisfactionClient(UserService.CurrentUser.EmployeeId);
            //la liste des projets TQE qui preteront a un envoi de courriel pour surveyMonkey

            
         //   var filename = @"\\tt.local\gfs\CAVolume2\Legacy\tts349fs3\Dept\AssQualite\Tableau de bord qualité TQE - Copie.xlsx";
            return View();
        }

        public ActionResult RapportSatisfactionClientFinale(string ProjectStartDate, string ProjectEndDate)
        {
            try
            {
                var result = _TTMService.getRapportSatisfactionClientFinale(ProjectStartDate, ProjectEndDate);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult IndexActiverProjet(string menuVisible = "true")
        {
            var listProjectsToActived = _TTMService.returnProjectsToActived();
            ViewBag.MenuVisible = menuVisible;
            return View(listProjectsToActived);
        }


        public ActionResult submitActiveProject(string projectId)
        {
            var result = _TTMService.ActiveProject(projectId);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult SendCourrielSatisfactionClient( string UniteAffaire, string TypeEnvoi )//,DateTime ProjectStartDate , DateTime ProjectEndDate
        {
            try
            {
                if (UniteAffaire == "QIB")
                {
                    var successUpdateBDWithExcel = _HomeService.UpdateBDWithExcelSatisfactionClient( );// ProjectStartDate, ProjectEndDate

                    if (successUpdateBDWithExcel == "success")
                    {
                        _HomeService.SendCourrielSatisfactionClient( TypeEnvoi); // ProjectStartDate, ProjectEndDate,

                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        return Json(new { result = successUpdateBDWithExcel }, JsonRequestBehavior.AllowGet);
                    }

                    
                }
                else //TQE
                {
                    var successUpdateBDWithExcel_TQE = _HomeService.UpdateBDWithExcelSatisfactionClient_TQE();//ProjectStartDate, ProjectEndDate

                    if (successUpdateBDWithExcel_TQE == "success")
                    {
                        _HomeService.SendCourrielSatisfactionClient_TQE( TypeEnvoi);//ProjectStartDate, ProjectEndDate,

                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        return Json(new { result = successUpdateBDWithExcel_TQE }, JsonRequestBehavior.AllowGet);
                    }
                }                

            }
            catch (Exception e)
            {
                return Json(new { result = e.InnerException + " / " + e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public  ActionResult SendCourrielSatisfactionClient_2 (string surveyURL, string UniteAffaire, string TypeEnvoi)//,DateTime ProjectStartDate , DateTime ProjectEndDate
        {
            try
            {
                if (UniteAffaire == "QIB")
                {

                    var line = 2;
                    using (SLDocument sl = new SLDocument(@"\\tt.local\gfs\CAVolume2\Legacy\tts349fs3\Dept\AssQualite\SSS\Satisfaction Clients - QIB - Copie.xlsm", "SSS"))
                    {
                        while (sl.GetCellValueAsString(line, 49) != "") //courriel client
                        {
                            if (sl.GetCellValueAsString(line, 49).Contains("Évaluation de la qualité")) //courriel client
                            {
                                if (sl.GetCellValueAsString(line, 21) == "") // Date envoie client
                                {
                                    if (sl.GetCellValueAsString(line, 25) == "") // Date info note chargé de projet
                                    {
                                        if (sl.GetCellValueAsString(line, 23) == "") // Relance client 2
                                        {
                                            listeProjetQIB.Add(sl.GetCellValueAsString(line, 1)); //Numéro de projet
                                            listeProjetQIB_titre.Add(sl.GetCellValueAsString(line, 2));//titre du mandat
                                            listeProjetQIB_CP.Add(sl.GetCellValueAsString(line, 5)); // Chargé de projet
                                            listeProjetQIB_DP.Add(sl.GetCellValueAsString(line, 4)); // directeur de projet
                                            listeProjetQIB_CP_email.Add(_HomeService.Get_CP_mail(sl.GetCellValueAsString(line, 4)));// email cp
                                            listeProjetQIB_client_email.Add(sl.GetCellValueAsString(line, 17));
                                        }
                                    }
                                }
                            }
                            line++;
                        }

                    }

                    _HomeService.SendCourrielSatisfactionClient_phase2(TypeEnvoi, "QIB", listeProjetQIB, listeProjetQIB_titre, listeProjetQIB_CP, listeProjetQIB_DP, listeProjetQIB_CP_email, listeProjetQIB_client_email);//ProjectStartDate, ProjectEndDate,

                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);


                }
                else //TQE
                {
                    var line = 2;
                    using (SLDocument sl = new SLDocument(@"\\tt.local\gfs\CAVolume2\Legacy\tts349fs3\Dept\AssQualite\Tableau de bord qualité TQE - Copie.xlsx", "SSS"))
                    {
                        while (sl.GetCellValueAsString(line, 52) != "") //courriel client
                        {
                            if (sl.GetCellValueAsString(line, 52).Contains("Évaluation de la qualité")) //courriel client
                            {
                                if (sl.GetCellValueAsString(line, 22) == "") // Date envoie client
                                {
                                    if (sl.GetCellValueAsString(line, 26) == "") // Date info note chargé de projet
                                    {
                                        if (sl.GetCellValueAsString(line, 24) == "") // Relance client 2
                                        {
                                            listeProjetTQE.Add(sl.GetCellValueAsString(line, 1)); //Numéro de projet
                                            listeProjetTQE_titre.Add(sl.GetCellValueAsString(line, 8));//titre du mandat
                                            listeProjetTQE_CP.Add(sl.GetCellValueAsString(line, 11)); // Chargé de projet
                                            listeProjetTQE_DP.Add(sl.GetCellValueAsString(line, 10)); // directeur de projet
                                            listeProjetTQE_CP_email.Add(_HomeService.Get_CP_mail(sl.GetCellValueAsString(line, 11)));// email cp
                                            listeProjetTQE_client_email.Add(sl.GetCellValueAsString(line, 18));
                                        }
                                    }
                                }
                            }
                            line++;
                        }

                    }



                    _HomeService.SendCourrielSatisfactionClient_phase2(TypeEnvoi, "TQE", listeProjetTQE, listeProjetTQE_titre, listeProjetTQE_CP, listeProjetTQE_DP, listeProjetTQE_CP_email,  listeProjetTQE_client_email);//ProjectStartDate, ProjectEndDate,

                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { result = e.InnerException + " / " + e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}