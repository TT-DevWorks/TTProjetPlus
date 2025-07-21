using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;
using ModeleSelectionRessources = TetraTech.TTProjetPlus.Views.ModeleSelection.ModeleSelectionManagementRessources;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ModeleSelectionController : ControllerBase
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly ModeleSelectionService _ModeleService = new ModeleSelectionService();
        //  public static  string user = "TT\\" + (UserService.CurrentUser).ToString().Replace(" ", ".");
        public static string stateModif = "";
        public static string stateModifNull = "";
        public static List<structureItem> ListeModelsMS = new List<structureItem>();


        // GET: ModeleSelection
        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
            //ViewBag.success = "";
            ViewBag.successModif = stateModif;
            ViewBag.successModifNull = stateModifNull;
            ListeModelsMS = _HomeService.GetModeleName();
            ViewBag.displayRetour = displayRetour;
            ViewBag.MenuVisible = menuVisible;
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Session["lang"] = lang;
            return RedirectToAction("Index");
        } 

        public ActionResult ModifyPartial()
        {
            return PartialView("_ModifyModelePartial");
        }


        public JsonResult returnModeleSelection(string modID)
        {
            var user = UserService.CurrentUser.TTAccount;
            var list = _ModeleService.GetModeleSelectionForModelId2(user, Int32.Parse(modID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnModeleSelectionAll()
        {
            var user = UserService.CurrentUser.TTAccount;
            var listAll = _HomeService.GetModeleSelectionAll(user);
            var listUserOnly = _ModeleService.GetModeleSelectionToDelete(user);
            return Json(new { ListAll = listAll, ListUserOnly = listUserOnly }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnModeleSelectionToDelete()
        {
            var user = UserService.CurrentUser.TTAccount;
            var list = _ModeleService.GetModeleSelectionToDelete(user);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult submitAction(string modeleSelectionName, string modeleSelectionDescription, string modID, string copyingFromExistingModeleSelection)// ActionResult 
        {
            var user = UserService.CurrentUser.TTAccount;
            var exists = _ModeleService.ReturnIsModeleNameExists(modeleSelectionName, user);
            if (exists == true)
            {
                return Json(new { result = "alreadyExists" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    var functionReturn = _ModeleService.AddNewModeleSelection(user, modeleSelectionName, modeleSelectionDescription, modID, copyingFromExistingModeleSelection);
                    if (functionReturn == "")
                    {
                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (functionReturn == "001")
                    {
                        return Json(new { result = "failed", details = ModeleSelectionRessources.EmptyListError }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = "failed", details = functionReturn }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception e)
                {
                    return Json(new { result = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }



        }


        public ActionResult returnStructureForModeleSelection(string modeleSelection)//, int modeleID)
        {
            //on verife que l on n'essaie pas de modifier des modèles de selection de type "commun"
            var canBeModified = _ModeleService.ReturnIsModeleModifiable(modeleSelection);
            if (canBeModified == false)
            {
                var structure = _HomeService.BuildTree("ModeleSelection", "", modeleSelection, true, false);
                node2 finalNode = new node2();
                node itemNode = new node("project", "0", null, false, "", false, true, "[PROJET]", false, "");
                finalNode.item = itemNode;
                finalNode.children = structure;

                return Json(new { tree = finalNode, status = "specialModele" }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                // on va presenter a l utilisateur l arbre des repertoires selon le modele de selection choisi;
                //dans cet arbre on va pre-cocher les repertoires qui existent deja pour ce projet 
                var structure = _HomeService.BuildTree("ModeleSelection", "", modeleSelection);
                node2 finalNode = new node2();
                node itemNode = new node("project", "0", null, false, "", false, true, "[PROJET]", false, "");
                finalNode.item = itemNode;
                finalNode.children = structure;
                return Json(finalNode, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult submitActionForModeleSelectionModif(string selectionModel, List<string> listOfNodes, List<string> listOfNodeToDelete)// ActionResult 
        {
            //on verife que l on n'essaie pas de modifier des modèles de selection de type "commun"
            var canBeModified = _ModeleService.ReturnIsModeleModifiable(selectionModel);
            if (canBeModified == false)
            {
                return Json(new { result = "specialModele" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if ((listOfNodes != null && listOfNodes.Count > 0) || (listOfNodeToDelete != null && listOfNodeToDelete.Count > 0))
                {
                    try
                    {
                        //la liste listOfNodes represente les repertoires qui sont restent cochés suite a la modification
                        //qui peut etre un ajout ou une supression. Nous sommes interessés par l etat actuel du treechecked:
                        //les repertoires cochés doivent a present reapparaitre dans le nouvel arbre

                        var ModId = _ModeleService.ReturnModIdForModeleSelection(selectionModel);
                        List<string> finalListOfRep = new List<string>();
                        var ListOfRepForModele = _ModeleService.ReturnRepForModele(Int32.Parse(selectionModel));
                        finalListOfRep = ListOfRepForModele;
                        //partie pour ajouter les repertoires été qui ont cochés dans l'arbre:
                        if (listOfNodes != null && listOfNodes.Count > 0)
                        {
                            List<string> NewListOfNode = new List<string>();
                            foreach (string item in listOfNodes)
                            {
                                var node = item.Split('/')[0].TrimStart().TrimEnd();
                                NewListOfNode.Add(node);
                            }


                            //on recupere la liste des repertoires du modele de selection:

                            finalListOfRep = finalListOfRep.Union(NewListOfNode).ToList();

                            //supprimer de la table [Modele_Selection_Repertoire] tous les repertoires et les remplacer par ceuux de la liste recue
                            _ModeleService.DeleteRepForModeleSelection(Int32.Parse(selectionModel));

                            //on repopule la table modele_selection_repertoire avec les nodes de listOfNodes:
                            _ModeleService.AddRepForModeleSelection(Int32.Parse(selectionModel), finalListOfRep);

                            //le nouvel arbre sera reconstruit a partir de ces nouvelles entrées dans la table 
                            //modele_slection_repertoires pour le modèle de sélection considéré
                        }

                        //partie pour supprimer les repertoires qui ont ete decochés dans l'arbre
                        if (listOfNodeToDelete != null && listOfNodeToDelete.Count > 0)
                        {
                            List<string> NewListOfNodeToDelete = new List<string>();
                            foreach (string item in listOfNodeToDelete)
                            {
                                
                                var node = item.Split('/')[0].TrimStart().TrimEnd();
                                NewListOfNodeToDelete.Add(node);
                            }

                            //on recherche les eventuels fichiers enfants (descendance complete) de chaque element de NewListOfNodeToDelete
                            List<string> notToExclude = new List<string>();
                            notToExclude.Add("1");
                            notToExclude.Add("2");
                            notToExclude.Add("3");
                            notToExclude.Add("559");

                            NewListOfNodeToDelete = NewListOfNodeToDelete.Except(notToExclude).ToList();
                            foreach (string repId in NewListOfNodeToDelete)
                            {
                                //if(repId != "1")
                                //{
                                var listChild = _HomeService.ReturnListOfAllDescendance(ModId, repId);
                                finalListOfRep = finalListOfRep.Except(listChild).ToList();

                                //}
                            }

                            //supprimer de la table [Modele_Selection_Repertoire] tous les repertoires et les remplacer par ceuux de la liste recue
                            _ModeleService.DeleteRepForModeleSelection(Int32.Parse(selectionModel));

                            //on repopule la table modele_selection_repertoire avec les nodes de listOfNodes:
                            _ModeleService.AddRepForModeleSelection(Int32.Parse(selectionModel), finalListOfRep);

                            //le nouvel arbre sera reconstruit a partir de ces nouvelles entrées dans la table 
                            //modele_slection_repertoires pour le modèle de sélection considéré



                        }
                        stateModif = "success";

                        return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        stateModif = "failed";
                        return Json(new { result = "failed", details = e.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = "notSubmitted" }, JsonRequestBehavior.AllowGet);

                }
            }

        }

        public ActionResult submitActionForModeleSelectionDelete(string modeleSelectionNameToDelete)// ActionResult 
        {
            //on verife que l on n'essaie pas de modifier des modèles de selection de type "commun"
            var canBeModified = _ModeleService.ReturnIsModeleModifiable(modeleSelectionNameToDelete);
            if (canBeModified == false)
            {
                return Json(new { result = "specialModele" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (modeleSelectionNameToDelete != "" || modeleSelectionNameToDelete != null)
                {
                    try
                    {
                        //on suprime les repertoires du modele de selection dans la table [Modele_Selection_Repertoire]
                        var selectionModel = Int32.Parse(modeleSelectionNameToDelete);
                        var functionReturn = _ModeleService.DeleteRepForModeleSelection(selectionModel);

                        if (functionReturn == "001")
                        {
                            var erreur = ModeleSelectionRessources.EmptyListToDeleteError;
                            return Json(new { result = "failedDel", details = erreur }, JsonRequestBehavior.AllowGet);
                        }
                        else if (functionReturn != "")
                        {
                            var erreur = ModeleSelectionRessources.DeleteError;
                            return Json(new { result = "failedDel", details = erreur + functionReturn }, JsonRequestBehavior.AllowGet);
                        }
                        //on supprime le modele en tant que tels dans la table [Modele_Selection]
                        var functionReturn2 = _ModeleService.DeleteModeleSelection(selectionModel);

                        if (functionReturn2 != "")
                        {
                            var erreur = ModeleSelectionRessources.DeleteErrorModelTemplate;
                            return Json(new { result = "failedDel", details = erreur + functionReturn2 }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new { result = "successDel" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { result = "failedDel", details = e.ToString() }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { result = "notSubmittedDel" }, JsonRequestBehavior.AllowGet);

                }
            }

        }
    }
}