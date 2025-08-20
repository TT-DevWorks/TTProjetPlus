using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class FolderStructureController : ControllerBase
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly FolderStructureService _FSService = new FolderStructureService();
        private readonly DashBoardService _DashboardService = new DashBoardService();
        public static List<structureItem> ListeProjetsWSDR = new List<structureItem>();
        private readonly ArchiveService _ArchiveService = new ArchiveService();
        public static List<structureItem> ListeServers = new List<structureItem>();
        private readonly LocalBPRService _LocalBPRService = new LocalBPRService();

        // GET: testFolderStructure
        //pour passer numero de projet: http://localhost:53460/FolderStructure/Index?projectToModify=00150

        public ActionResult Index(string displayRetour = "true", string menuVisible = "true", string projectToModify ="")
        {
           // ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();
            //ListProjectsIDModel listProjets = new ListProjectsIDModel();
            //listProjets.listOfProjectsIDs = _HomeService.getProjectsIDList();
            //listProjets.listOfModels = _HomeService.getModelsList();
            ListeProjetsWSDR = _HomeService.getListWSDR(); //with structure de repertoire
            ViewBag.displayRetour = displayRetour;
            ViewBag.MenuVisible = menuVisible;
            ViewBag.ProjectToModify = projectToModify;
            ViewBag.lauchnEvent = false; 
            if (projectToModify != "" && projectToModify != null)
            {
                ViewBag.lauchnEvent = true;
            }
            return View();
        }


        public ActionResult Index2(string displayRetour = "true", string menuVisible = "true", string projectToModify = "")        {
           
            ListeProjetsWSDR = _HomeService.getListWSDR(); //with structure de repertoire
            ListeServers = returnListServer();
            ViewBag.displayRetour = displayRetour;
            ViewBag.MenuVisible = menuVisible;
            ViewBag.ProjectToModify = projectToModify;
            return View();
        }

        public ActionResult Index3(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            LocalBPRModel model = new LocalBPRModel();
            model.ListADTTLocalBPR = _LocalBPRService.GetListeADLocalBPR();
            return View(model);
        }


        [HttpGet]
        public ActionResult returnStructure(string projectNumber, string modeleSelection = "")//, int modeleID)
        {
            // on va presenter a l utilisateur l arbre des repertoires selon le modele de selection choisi;
            //dans cet arbre on va pre-cocher les repertoires qui existent deja pour ce projet 

            var structure = _HomeService.BuildTree("FolderStructure", projectNumber, modeleSelection, false, false);
            node2 finalNode = new node2();
            node itemNode = new node("project", "0", null, false, "", false, false, "[PROJET]", false,"" );
            finalNode.item = itemNode;
            finalNode.children = structure;
            return Json(finalNode, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult returnStructureWMDS(string projectNumber, string modeleSelection = "")//, int modeleID)
        {
            // on va presenter a l utilisateur l arbre des repertoires selon le modele de selection choisi;
            //dans cet arbre on va pre-cocher les repertoires qui existent deja pour ce projet 

            // var structure = _HomeService.BuildTreeForProjectAndMS("FolderStructure", projectNumber, modeleSelection, false, false);
            var structure = _HomeService.BuildTreeForProjectAndMSLG("FolderStructure", projectNumber, modeleSelection, false, false);
            
            node2 finalNode = new node2();
            node itemNode = new node("project", "0", null, false, "", false, false, "[PROJET]", false, "");
            finalNode.item = itemNode;
            finalNode.children = structure;
            return Json(finalNode, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnServerUsed(string projectNumber)
        {
            var servers = _FSService.getServersUsed(projectNumber);
            var resume = _FSService.getResumeForProject(projectNumber)  ; //peut etre null
            var modId = _FSService.ReturnModId(projectNumber);
            var modNom = _FSService.ReturnModNom(modId);
            return Json(new { Server = servers , Resume = resume, ModId = modNom }, JsonRequestBehavior.AllowGet);
        
                }

        public JsonResult returnBPRProjectsList()
        {
            return Json(ListeProjetsWSDR, JsonRequestBehavior.AllowGet);
        }


        public JsonResult returnModeleSelectionAll(string projectNumber)
        {
            string user = UserService.CurrentUser.TTAccount;
            var list = _HomeService.GetModeleSelectionAll2(user, projectNumber);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult existsOnServer ( string projectNumber)
        {
            ////ceci est pour la version de production
            //var server = _ArchiveService.returnPathPartage(projectNumber);
            //string path = server + "\\" + projectNumber;
            //var exist = Directory.Exists(path);
            ////fin//

            //string path1 = @"\\TTS349TEST03\prj_reg" + "\\" + projectNumber;
            //if (Directory.Exists(path1)) // Directory.Exists(path1)
            //{
            //    return Json(new { result = "existing" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //    return Json(new { result = "notExisting" }, JsonRequestBehavior.AllowGet);
            return Json(new { result = "existing" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult submitAction(string projectId, string selectionModel, List<string> listOfNodes)// ActionResult 
        {
            //pour tqe:
            //
            string trace = "";            

            var ModId = _FSService.ReturnModId(projectId);            
            var isProjetEnvDiligence = _HomeService.isProjetEnvDiligence(projectId);

            if (listOfNodes != null)
            {
                try
                {
                    List<NodeItem> listNodeIdAndDescription = new List<NodeItem>();
                    foreach (string item in listOfNodes)
                    {
                        NodeItem nodeItem = new NodeItem();
                        nodeItem.nodeID = item.Split('/')[0].TrimStart().TrimEnd();
                        //nodeItem.nodeDescription = item.Split('-')[1].TrimStart().TrimEnd();
                        listNodeIdAndDescription.Add(nodeItem);
                    }

                    //1.identifier les nouveaux repertoires a ajouter
                    var listPresentFolders = _HomeService.FolderUsedInProject(projectId);
                    List<NodeItem> listOfFolderToAdd = new List<NodeItem>();
                    foreach (NodeItem item in listNodeIdAndDescription)
                    {
                        if (!listPresentFolders.Contains(item.nodeID))
                        {
                            listOfFolderToAdd.Add(item);
                        }

                    }
                    // var listOfFolderToAdd = listOfNodes.Except(listPresentFolders).ToList();

                    //2. trouver tous les repertoires parents de ces nouveaux repertoires (listOfFolderToAdd)
                    List<parentChildItemString> listOfParentChild = new List<parentChildItemString>();
                    if (listOfFolderToAdd.Count > 0)
                    {

                        foreach (NodeItem item in listOfFolderToAdd)
                        {
                            parentChildItemString parentChildItem = new parentChildItemString();
                            parentChildItem.child = item.nodeID;
                            parentChildItem.parentList = new List<string>();
                            var findParentForThisItem = item;
                            bool parentExist = true;
                            while (parentExist)
                            {
                                try
                                {

                                    var newParent = _FSService.ReturnParentFolder(findParentForThisItem, ModId);
                                    if (newParent != null && newParent.nodeID != "0" && newParent.nodeID != "1")
                                    {
                                        parentChildItem.parentList.Add(newParent.nodeID);
                                        findParentForThisItem = newParent;
                                        parentExist = true;
                                    }

                                    else parentExist = false;
                                }
                                catch (Exception e)
                                {
                                    parentExist = false;
                                }

                            }

                            listOfParentChild.Add(parentChildItem);
                        }

                        //=>nous avons a present une liste des repertoires a ajouter et de leur parent respectifs

                        //3. ajouter les repertoires supplementaires sur le serveur
                        //  **Specify the directory you want to manipulate.

                        //on va maintenant enregistrer les nouveux folder dans la base de donnéés pour le projet considéré:
                        trace = trace + "; " + "Appel à AddFoldersToProjectOnDB";
                        _FSService.AddFoldersToProjectOnDB(listOfFolderToAdd, projectId);                        
                        trace = trace + "; " + "Retour de AddFoldersToProjectOnDB";

                       
                        trace = trace + "; " + "Appel à checkFoldersForProject";
                        if (isProjetEnvDiligence == true)
                        {
                            var checkedFolders = _FSService.checkFoldersForProjectEnvDiligence(projectId);
                            trace = trace + "; " + "Retour de checkFoldersForProject";
                            return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var checkedFolders = _FSService.checkFoldersForProject(projectId);
                            trace = trace + "; " + "Retour de checkFoldersForProject";
                            return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //ici il faut verifier si la liste des repertoires sur le serveur est conforme a 
                        //celle qu il y a sur le serveur; si ce n est pas le cas il faut ajouter ceux qui manquent.
                        trace = trace + "; " + "else Appel à checkFoldersForProject";
                        if (isProjetEnvDiligence == true)
                        {
                            var checkedFolders = _FSService.checkFoldersForProjectEnvDiligence(projectId);
                            trace = trace + "; " + "else Retour de checkFoldersForProject";
                            return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var checkedFolders = _FSService.checkFoldersForProject(projectId);
                            trace = trace + "; " + "else Retour de checkFoldersForProject";
                            return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                catch (Exception e)
                {
                    _HomeService.addErrorLog(e, "FolderStructure", "submitAction", projectId, trace);

                    return Json(new { result = "failedMDSR", error = e.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                //var outputResult = _HomeService.addSecurityOnFolders(projectId, ModId, 1, "submitAction", "FolderStructure");
                if (isProjetEnvDiligence == true)
                {
                    var checkedFolders = _FSService.checkFoldersForProjectEnvDiligence(projectId);
                    return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var checkedFolders = _FSService.checkFoldersForProject(projectId);
                    return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        //4. mettre a jour la base de données: 
        //return View("TTProjetFolderStructure");

        public ActionResult copyFileFromServer()
        {
            try
            {
                if (System.IO.File.Exists(@"\\tts372fs1\prj_reg\06792\test.txt"))
                {
                    System.IO.File.Delete(@"\\tts372fs1\prj_reg\06792\test.txt");
                }

                System.IO.File.Copy(@"C:\TTProjetPlus\TQEDocs\test.txt", @"\\tts372fs1\prj_reg\06792\test.txt");
        //   System.IO.File.Copy(@"C:\Users\Frederic.Ohnona\source\Workspaces\TTProjetPlus\DEV\TetraTech.TTProjetPlus\TQEDocs\test.txt", @"\\tts349test03\Prj_Reg\test20220316\test.txt");

            return Json(new { result = "success"}, JsonRequestBehavior.AllowGet);

            }
            catch(Exception e)
            {
                return Json(new { result = e.InnerException + " / "+ e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public List<structureItem> returnListServer()
        {
            List<structureItem> liste = new List<structureItem>();
                      
            liste = _HomeService.GetServerName();

            return liste;
        }

        public ActionResult ModifiyServer(string projectNumber, string newServer)
        {
            try
            {
                var result = _FSService.ModifyServer(projectNumber, newServer);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
             catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult AddEmp60Conf(string TTAccount, string ProjectID)
        {
            try
            {
                var result = _FSService.AddEmp60Conf(TTAccount, ProjectID);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ModifySecurityProject60Conf(string projectId)
        {
            try
            {
                var checkedFolders = _FSService.checkFoldersForProject(projectId);
                return Json(new { result = checkedFolders }, JsonRequestBehavior.AllowGet);
            }
            
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);
            }          
           
        }
    }    

}
