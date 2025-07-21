using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly DashBoardService _DashboardService = new DashBoardService();

       
        public static List<structureItem> ListeCompanies = new List<structureItem>();
        public static List<structureItem> ListeUA = new List<structureItem>();
        public static List<structureItem> ListeORG = new List<structureItem>();
        public static List<structureItem> ListeServers = new List<structureItem>();
        public static List<structureItem> ListeModels = new List<structureItem>();
        public static List<structureItem> ListeSecurityModels = new List<structureItem>();
        public static List<structureItem> ListeSelectionModels = new List<structureItem>();
        public static List<structureItem> ListeProjectManager = new List<structureItem>();
        public static List<structureItem> ListeProjects = new List<structureItem>();
        public static List<structureItem> ListeAdministrators = new List<structureItem>();
        public static string masterProjectPanelState = "";

        public string previewNumber = "";
      //  public static string masterProjectStruct = "";
        public static int? countOfMasterProjects = 0;        

        public string[] returnNameArray()
        {

            var rtnlist = new List<string>();

            rtnlist = _HomeService.GetCustomersNames();

            return rtnlist.ToArray();
        }


        public ActionResult Index(string state="", string structure="open", string displayRetour = "true", string menuVisible = "true")
        {
            // ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();
            ViewBag.MenuVisible = menuVisible;
            ViewBag.NumberArray = NumberArrayFromEntity();
            ViewBag.projectsNumberArray = ProjectNumberListArrayFromEntity();
            ViewBag.NameArray = returnNameArray();
            ViewBag.GestionnairesNumberArray= GestionnairesArrayFromEntity();
            ViewBag.GestionnairesPrgrNumberArray = GestionnairesArrayFromEntity();


            masterProjectPanelState = state;
            Session["masterProjectPanelState"] = state;
            Session["masterProjectStruct"] = structure;
            ListeServers = renderList("GetServerName");
            ListeModels= renderList("GetModeleName");
            ListeSecurityModels = renderList("GetModeleSecurite");
            ListeSelectionModels = renderList("GetModeleSelection");
            ListeORG = renderList("GetORGs");
            ListeCompanies = renderList("GetCompanies");
            ViewBag.Companies= ListeCompanies.ToArray();
            ListeUA = renderList("GetUAs");
            ListeORG = renderList("GetORGs");
            ListeProjectManager = renderList("ProjectManagers");
            ListeAdministrators = renderList("Administrators");
            countOfMasterProjects = returnNumberOfMasterProjects();
            Session["nextBatchOfMasterProject_start"] = 0;
            Session["nextBatchOfMasterProject_limit"] = 100;
            Session["cancelled"] = "false";
            Session["searchTerm"] = "";
            // ListeProjects = returnListProjects2(); commenté pour test
            Session["HasAccessToNewProjectNumber"] = _HomeService.HasAccessToNewProjectNumber(UserService.CurrentUser.EmployeeId);
            Session["isProductionMode"] = UserService.isPRODEnvironment();
            return View();
        }
        public int? returnNumberOfMasterProjects()
        {
            return _HomeService.returnNumberOfMasterProjects();
        }
        public ActionResult displayNextResults (string searchTerm)
        {
            if (Session["searchTerm"].ToString() == searchTerm && Session["cancelled"].ToString() == "false")
            {
                Session["nextBatchOfMasterProject_start"] = (int)Session["nextBatchOfMasterProject_start"] + 100;
            }
            else
            {
                Session["searchTerm"] = searchTerm;
                Session["nextBatchOfMasterProject_start"] = 0;

            }
            searchModelForMasterProject model = new searchModelForMasterProject();
            List <masterProjectItem> listOfResult = new List<masterProjectItem>();
            listOfResult = _HomeService.displayNextResults(searchTerm, (int)Session["nextBatchOfMasterProject_start"], (int)Session["nextBatchOfMasterProject_limit"]);
            model.listOfItems = listOfResult;
            if(listOfResult.Count==0)
            {
                Session["nextBatchOfMasterProject_start"] = 0;
            }
            model.searchTerm = searchTerm;
            Session["searchTerm"] = searchTerm;

            return PartialView("SearchMasterProjectPartial", model);
        }

        public void resetCounter(string searchTerm)
        {
            if (searchTerm == "")
            {
                Session["nextBatchOfMasterProject_start"] = 0;
                
            }
        Session["cancelled"] = "true";
        }

        public ActionResult displayNextResults2(string searchTerm_internal)
        {
            if (Session["searchTerm"].ToString() == searchTerm_internal)
            {
                Session["nextBatchOfMasterProject_start"] = (int)Session["nextBatchOfMasterProject_start"] + 100;
            }
            else
            {
                Session["searchTerm"] = searchTerm_internal;
                Session["nextBatchOfMasterProject_start"] = 0;

            }
            searchModelForMasterProject model = new searchModelForMasterProject();
            List<masterProjectItem> listOfResult = new List<masterProjectItem>();
            listOfResult = _HomeService.displayNextResults(searchTerm_internal, (int)Session["nextBatchOfMasterProject_start"], (int)Session["nextBatchOfMasterProject_limit"]);
            model.listOfItems = listOfResult;
            model.searchTerm = searchTerm_internal;

            
            return PartialView("SearchMasterProjectPartial2", model);
        }

        
        public List<structureItem> renderList(string methodName)
        {
            List<structureItem> liste= new List<structureItem>();

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

                case "GetModeleName":
                    liste = _HomeService.GetModeleName();
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
                case "Administrators":
                    liste = _HomeService.GetAdministratorsList("1");
                    break;
            }


            return liste;
        }
        
        
       


        //pour dropdown en cascade
        public JsonResult  returnSelectedUA(string company)
        {
            var list = _HomeService.GetUAsForSelectedCompanies(company);
            Session["listeUA"] = list;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnSelectedORG(string UAId)
        {
            var list = _HomeService.GetUAsForSelectedORG(Int32.Parse(UAId));
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public string[] NumberArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.GetCustomersNumbers();

            return rtnlist.ToArray();
        }

        public string[] GestionnairesArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.GetGestionnairesNames();

            return rtnlist.ToArray();
        }


        public string[] ProjectNumberListArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.ProjectNumberListArrayFromEntity();


            return rtnlist.ToArray();
        }

        public ActionResult returnListCustomerNumberName()
        {
            var model =  _HomeService.GetCustomersNumberName();

            return PartialView("CustomersNumberNamePartial", model);
        }

        //decommnenter lorsque test02 est operationnel
        public ActionResult returnListProjectManagers()
        {
            var model = _HomeService.GetProjectManagers();

            return PartialView("ProjectManagerPartial", model);
        }

        public ActionResult returnListProjects()
        {
            var model = _HomeService.GetProjectFiltered2();

            return PartialView("ProjectsListPartial", model);
        }

        public List<structureItem> returnListProjects2()
        { 
            var list =_HomeService.GetProjectFiltered2();
            return list;
        }

        public ActionResult returnListPhases (string masterProjectId)
        {
            PhaseModel phaseModel1 = new PhaseModel();
            PhaseModel phaseModel2 = new PhaseModel();

            var phases1 = _HomeService.ObtainAvailablePhaseCodes1(masterProjectId);
            phaseModel1.structureItem = phases1;
            phaseModel1.phaseId = 1;

            var phases2 = _HomeService.ObtainAvailablePhaseCodes2(masterProjectId);
            phaseModel2.structureItem = phases2;
            phaseModel2.phaseId = 2;


            return Json(new { phasesList1 = phases1 , phasesList2 = phases2 }, JsonRequestBehavior.AllowGet);
            

        }

        public ActionResult returnListProjectFiltered( string numeroProjet, string titreProjet, string gestionnairesList,
             string companiesList, string UAList, string ORGList)
        {
            var model = _HomeService.GetProjectFiltered(numeroProjet, titreProjet, gestionnairesList, companiesList, UAList, ORGList);

            return PartialView("ProjectsListPartial", model);
        }


        public ActionResult returnListProject()
        {
            var model = _HomeService.GetProjects();

            return PartialView("ProjectsListPartial", model);
        }



        // pour les radio boutons oui-non//////////////////

        [HttpGet]
        public void changeMasterProjectPanelSate(string state)
        {
             Session["masterProjectPanelState"] = state;
        }
        //[HttpGet]
        //public ActionResult returnPanelSate()
        //{
        //    return Json(masterProjectPanelState, JsonRequestBehavior.AllowGet); 
        //    // return masterProjectPanelState;
        //}

        [HttpGet]
        public void changeMasterProjectStructure(string state)
        {
            Session["masterProjectStruct"] = state;
        }

        //[HttpGet]
        //public string returnProjectStruture()
        //{
        //    return Session["masterProjectStruct"].ToString();
        //}

        //
        ///////////////////////////////////////////////////////
        

        public ActionResult submitAction (
            string AbreviatedTitle,
            string   HasAMasterProject,
            string MasterProjectNumber,
            string PhaseCode,
            string PhaseCodeId,
            string ClientNumber,
            string ClientName,
            string CompanyId,
            string BusinessUnit,
            string ORG,
            string ProjectMgr,
            string ProgramMgr,
            string currentUser,
            string ClientFinal,
            string ClientContact,
            string ApproximateAmount,
            string RefAndComments,
            string Server,
            string ServerModel,
            string SecurityModel,
            string SelectionModel,
            string ClientProjectManager,
            string ClientEmailProjectManager,
            bool HasProjectDiligente,
            bool   HasprojectStruture
            )
        {

            numberGenerationModel modelToSend = new numberGenerationModel();
            modelToSend.AbreviatedTitle = AbreviatedTitle;
            modelToSend.HasAMasterProject = HasAMasterProject;
            modelToSend.MasterProjectNumber = MasterProjectNumber;
            modelToSend.PhaseCodeFinal = PhaseCode;
            modelToSend.PhaseCodeId = PhaseCodeId;
            modelToSend.ClientNumber = ClientNumber;
            modelToSend.ClientName = ClientName;
            modelToSend.CompanyName = CompanyId;//_HomeService.GetCompanyName(CompanyId);
            modelToSend.Company = CompanyId;
            modelToSend.BusinessUnit = BusinessUnit;
            modelToSend.BusinessUnitName = _HomeService.GetUAName(BusinessUnit);
            modelToSend.ORG = ORG;
            modelToSend.ProjectMgr = ProjectMgr;
            modelToSend.ProgramMgr = ProgramMgr;
            modelToSend.ProjectMgrNumber = _HomeService.GetProjectManagersNumber(ProjectMgr);
            modelToSend.ProgramMgrNumber = _HomeService.GetProjectManagersNumber(ProgramMgr);
            modelToSend.ORGName = _HomeService.GetORGName(ORG);
            modelToSend.ClientFinal = ClientFinal;
            modelToSend.ClientContact = ClientContact;
            modelToSend.ApproximateAmount = (ApproximateAmount =="" || ApproximateAmount==null)?"": ApproximateAmount.Split(' ')[0]; // a verifier
            modelToSend.RefAndComments = RefAndComments;
            modelToSend.HasprojectStruture = HasprojectStruture;

            modelToSend.ServerName = _HomeService.GetNameForServer(Server);
            modelToSend.Server = Server;
            modelToSend.ServerCheminPartage = _HomeService.returnCheminPartage(Server);

            modelToSend.ServerModelName = _HomeService.GetServerModelName(ServerModel);
            modelToSend.ServerModel = ServerModel;

            modelToSend.SecurityModelName = _HomeService.GetSecurityModelName(SecurityModel);
            modelToSend.SecurityModel = SecurityModel;

            modelToSend.SelectionModelName = _HomeService.SelectionModelName(SelectionModel);
            modelToSend.SelectionModel = SelectionModel;
            
            modelToSend.ClientProjectManager  = ClientProjectManager;
            modelToSend.ClientEmailProjectManager  = ClientEmailProjectManager;

            modelToSend.ShowAutomaticEmailAddresses = _HomeService.AutomaticEmailAddresses(BusinessUnit, ORG);

            if (HasAMasterProject !="0")
            {
            modelToSend.previewNumber = PreviewNumber(HasAMasterProject, MasterProjectNumber, PhaseCode, PhaseCodeId, CompanyId);

            }
            else
            {
                modelToSend.previewNumber = "";

            }

            modelToSend.HasProjectDiligente = HasProjectDiligente;
            // Session["previewNumber"] = modelToSend.previewNumber;
            ////GNP = Generation Numero Projet
            return PartialView("resumeGNP", modelToSend);
            //var serialized = JsonConvert.SerializeObject(modelToSend);
            //Session["modelFinal"] = modelToSend;// JsonConvert.DeserializeObject<numberGenerationModel>(serialized);
            //return Json(new { redirectToUrl = Url.Action("resumeGNP", "Home") }, JsonRequestBehavior.AllowGet);

        }
  
        public ActionResult resumeGNP(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;

            return View(Session["modelFinal"]);
        }

        public ActionResult returnToIndex (numberGenerationModel model)
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        public string returnClientName(string custNumber)
        {
            return _HomeService.GetCustomerNameByNumber(custNumber);
        }

        public string returnClientNumber(string custName)
        {
            return _HomeService.GetCustomerNumberByName(custName);
        }

        public string  returnTitleforProjectNumber(string projectNumber)
        {
            return _HomeService.GetProjectTitleByNumber(projectNumber);
        }

        [HttpGet]
        public ActionResult returnFolderStructureForCreation(string modID, string modSelectionID)
        {
            //        [ {
            //           item:{id:'id', label:'label', checked:false}, 
            //              chidren:[{
            //              item:{id:'id', label:'label', checked:false}, 
            //             chidren:[...]
            //                       }]
            //              }, ....]
            var structure = _HomeService.BuildTreeCreation(Int32.Parse(modID), Int32.Parse(modSelectionID));
            node2 finalNode = new node2();
            node itemNode = new node("project", "0", null, false, "", false, false, "[PROJET]", false, "");
            finalNode.item = itemNode;
            finalNode.children = structure;
            return Json(finalNode, JsonRequestBehavior.AllowGet);
        }


        public JsonResult returnModeleSelection(string modID)
        {
            string user = UserService.CurrentUser.TTAccount;
            var list = _HomeService.GetModeleSelectionForModelId(user, Int32.Parse(modID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #region creation de numero de projet
        [ValidateInput(false)]
        public ActionResult GetProjectNumber(  //ProjectNumberCreation
            string AbreviatedTitle,
            string HasAMasterProject,
            string MasterProjectNumber,
            string PhaseCode,
            string PhaseCodeID,
            string ClientNumber,
            string ClientName,
            string CompanyId,
            string BusinessUnit,
            string ORG,
            string ProjectMgr,
            string ProjectMgrNumber,
            string ProgramMgrNumber,
            string currentUser,
            string ClientFinal,
            string ClientContact,
            string ApproximateAmount,
            string RefAndComments,
            string Server,
            string ServerModel,
            string SecurityModel,            
            string SelectionModel,
            Boolean HasprojectStruture,
            string ClientProjectManager,
            string ClientEmailProjectManager,
            Boolean HasProjectDiligente,
            string TQELang ="",
            string EmailAddresses=""            
            )
        {
            try
            {
                //var PhaseCode1 = PhaseCode;
                //if (PhaseCodeID == "1" || PhaseCodeID == "3")
                //{
                //    PhaseCode1 = "-" + PhaseCode;
                //}

                ////1. enregistrement dans la table os_projects: 
                ////attribution d'un numero de projet
                //var projectNumber = _HomeService.returnProjectNumber();
                ////regles pour les numeros de projet:
                ////              SI Projet Maitre demandé ALORS: 
                //if (HasAMasterProject == "0")
                //{
                //    if (CompanyId != "711 TQE") //
                //    {
                //        projectNumber = projectNumber + "TT";
                //    }
                //    else
                //    {
                //        projectNumber = projectNumber + "";
                //    }
                //}

                //else
                //{
                //    if (CompanyId != "711 TQE")
                //    {
                //        if (!MasterProjectNumber.Contains("TT"))
                //        {
                //            projectNumber = MasterProjectNumber + "TT" + PhaseCode1;
                //        }
                //        else
                //        {
                //            projectNumber = MasterProjectNumber + PhaseCode1;
                //        }
                //    }
                //    else
                //    {
                //        if (MasterProjectNumber.Contains("TT"))
                //        {
                //            projectNumber = MasterProjectNumber.Replace("TT", "") + PhaseCode1;
                //        }
                //        else
                //        {
                //            projectNumber = MasterProjectNumber + PhaseCode1;
                //        }
                //    }
                //}
                var projectNumber = PreviewNumber(HasAMasterProject, MasterProjectNumber, PhaseCode,  PhaseCodeID, CompanyId);// Session["previewNumber"].ToString();
                var statusAddToTT = _HomeService.registerProjectInOSProject(

                  AbreviatedTitle,
                  HasAMasterProject,
                  MasterProjectNumber,
                  PhaseCode,
                  PhaseCodeID,
                  ClientNumber,
                  ClientName,
                  CompanyId,
                  BusinessUnit,
                  ORG,
                  ProjectMgrNumber,
                  ProgramMgrNumber,
                  ClientFinal,
                  ClientContact,
                  ApproximateAmount,
                  RefAndComments,
                  currentUser,
                  projectNumber,
                  1,
                  HasprojectStruture,
                  ClientProjectManager,
                  ClientEmailProjectManager);

                var statusAddToBPR = "";
                var statusAddToOSProjectEnv = "";
                if (statusAddToTT == "successInsertionInTT")
                {
                    //2. enregistrement dans bprprojet dans le cas ou on a demandé une structure de répertoires:
                    if (HasprojectStruture == true)
                    {
                        if (BusinessUnit == "10640") //Projet 715-Environnement
                        {
                            if (HasProjectDiligente == true) //Sécurité diligente
                            {
                                string EmailEnvDiligente = "";
                                statusAddToOSProjectEnv = _HomeService.registerProjectInOSProjectEnv(projectNumber, AbreviatedTitle, ProjectMgrNumber, currentUser);
                                var SelectionModelNumber = SelectionModel == "" ? 0 : Int32.Parse(SelectionModel);
                                statusAddToBPR = _HomeService.AddProjectInTTProjetWithDiligenteEnv(
                                   projectNumber,
                                   Int32.Parse(SecurityModel),
                                   Int32.Parse(Server),
                                   ServerModel,
                                   0,
                                   currentUser,
                                   ClientName,
                                   null,
                                   1,
                                   false,
                                   SelectionModel
                                        );
                                
                                     EmailEnvDiligente = _HomeService.SendEmailEnvDiligente(projectNumber);
                            }
                            else //Sécurité normale
                            {
                                var SelectionModelNumber = SelectionModel == "" ? 0 : Int32.Parse(SelectionModel);
                                statusAddToBPR = _HomeService.AddProjectInTTProjet(
                                   projectNumber,
                                   Int32.Parse(SecurityModel),
                                   Int32.Parse(Server),
                                   ServerModel,
                                   0,
                                   currentUser,
                                   ClientName,
                                   null,
                                   1,
                                   false,
                                   SelectionModel
                                        );
                            }

                        }
                        else //tout autre Projet que 715-Environnement
                        {
                            var SelectionModelNumber = SelectionModel == "" ? 0 : Int32.Parse(SelectionModel);
                            statusAddToBPR = _HomeService.AddProjectInTTProjet(
                                projectNumber,
                                Int32.Parse(SecurityModel),
                                Int32.Parse(Server),
                                ServerModel,
                                0,
                                currentUser,
                                ClientName,
                                null,
                                1,
                                false,
                                SelectionModel
                                    );
                            
                        }
                    }

                    // Si compagnie = 711 TQE, on appelle la SP qui envoi courriel automatique
                    if (CompanyId == "711 TQE" && TQELang != "")
                    {
                        EmailAddresses = _HomeService.SendOSEmail(projectNumber, TQELang);
                    }
                }

                return Json(new { insertionInBPR = statusAddToBPR, insertionInTT = statusAddToTT, insertionInOSProjectEnv = statusAddToOSProjectEnv, number = projectNumber, result = "", details = "", emailAddresses = EmailAddresses }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { resultFail = "failedCreation", details = e.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
        // partie pour ouvrir unione windows explorer au niveau du chemin de partage du serveur choisi
        public ActionResult openServerLocation (string serverId, string projectFolder)
        {
              string path = _HomeService.returnPath(serverId)+ "\\"+ projectFolder;

            //_HomeService.SetFolderPermission(path);

            var outputResult = _HomeService.addSecurityOnFolders(projectFolder, "", 1, "AddProjectInTTProjet", "FolderStructure");

            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = "explorer";
            //info.Arguments = path;
            //Process.Start(info);
            //Process.Start("explorer.exe", path); //@"C:\..."
            return Json(new { result = path }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult checkNameAvailability (string AbreviatedTitle)
        {
            var available = _HomeService.checkNameAvailability(AbreviatedTitle);
            return Json(new { result = available }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult applyDefaultValuesFromMasterProject (string masterProjectNumber)
        {
            
            var modelFromMasterProject = _HomeService.returnDataForMasterProject(masterProjectNumber);
            return Json(modelFromMasterProject, JsonRequestBehavior.AllowGet);

        }

        public ActionResult checkForDoublons(string master, string phase, string company)
        {
            if(phase=="")
                return Json(new { result = "canContinue" }, JsonRequestBehavior.AllowGet);
            else
            {
                var doublons = _HomeService.checkForDoublons(master,  phase,  company);
            return Json(new { result = doublons }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult checkIfMasterProjectNumberExists(string master)
        {
            var doublons = _HomeService.checkIfMasterProjectNumberExists(master);
            return Json(new { result = doublons }, JsonRequestBehavior.AllowGet);
        }

        public void addSecurityOnFolders(string projectid, string modID, Nullable<byte> DoDelBatFile, string function, string controller)
        {
            int intModId = 0;
            if (modID == "")
            {
                intModId = _HomeService.GetProjectModId(projectid);
                if (intModId != 0)
                {
                    modID = intModId.ToString();
                }
            }            

            _HomeService.addSecurityOnFolders(projectid, modID, DoDelBatFile, function, controller);
        }

        public Boolean isProjectOnTestServer(string pro_id)
        {
            return _HomeService.isProjectOnTestServer(pro_id);
        }

        public string PreviewNumber(string HasAMasterProject,string MasterProjectNumber,string PhaseCode,string PhaseCodeID, string CompanyId)
        {
            var PhaseCode1 = PhaseCode;
            if (PhaseCodeID == "1" || PhaseCodeID == "3")
            {
                PhaseCode1 = "-" + PhaseCode;
            }

            //1. enregistrement dans la table os_projects: 
            //attribution d'un numero de projet
            var projectNumber = "";
            //regles pour les numeros de projet:
            //              SI Projet Maitre demandé ALORS: 
            if (HasAMasterProject == "0")
            {
                projectNumber = _HomeService.returnProjectNumber();
                if (CompanyId != "711 TQE") //
                {
                    projectNumber = projectNumber + "TT";
                }
                else
                {
                    projectNumber = projectNumber + "";
                }
            }

            else
            {
                if (CompanyId != "711 TQE")
                {
                    if (!MasterProjectNumber.Contains("TT"))
                    {
                        projectNumber = MasterProjectNumber + "TT" + PhaseCode1;
                    }
                    else
                    {
                        projectNumber = MasterProjectNumber + PhaseCode1;
                    }
                }
                else
                {
                    if (MasterProjectNumber.Contains("TT"))
                    {
                        projectNumber = MasterProjectNumber.Replace("TT", "") + PhaseCode1;
                    }
                    else
                    {
                        projectNumber = MasterProjectNumber + PhaseCode1;
                    }
                }
            }
            return projectNumber;
        }
    }
}