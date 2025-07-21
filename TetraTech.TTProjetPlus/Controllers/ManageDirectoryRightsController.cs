using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Security;
using ManageDirectoryRightsRessources = TetraTech.TTProjetPlus.Views.ManageDirectoryRights.MDRessources;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ManageDirectoryRightsController : ControllerBase
    {
        private readonly ManageDirectoryRightsService _manageDirectoryRightsService = new ManageDirectoryRightsService();
        private readonly HomeService _HomeService = new HomeService();
        public static List<SelectListItem> ListeDirectoryRights = new List<SelectListItem>();
        public static List<SelectListItem> ListeProjets = new List<SelectListItem>();

        public ActionResult Index()
        {
            ViewBag.MenuVisible = "true";
            ViewBag.EmployeesArray = EmployeesArrayFromEntity();

            ManageDirectoryRightsViewModel result = new ManageDirectoryRightsViewModel(CurrentBaseViewModel)
            {
                DirectoryRightList = new List<SelectListItem>(),
                DirectoryRightsDetails = new ManageDirectoryRightsDetails()
            };

            ListeDirectoryRights = GetListeDirectoryRights();
            ListeProjets = GetListeProjets();

            return View(result);
        }

        public ActionResult returnListEmployes()
        {
            var model = _HomeService.GetProjectManagers();

            return PartialView("EmployeePartial", model);
        }

        public List<SelectListItem> GetListeProjets()
        {
            ManageDirectoryRightsDetails model = _manageDirectoryRightsService.GetProjectList(UserService.CurrentUser.TTAccount.Replace("TT\\", ""));
            return model.ProjectList;
        }

        public List<SelectListItem> GetListeDirectoryRights()
        {
            return _manageDirectoryRightsService.GetDirectoryRightsList();      
        }

        public PartialViewResult GetDirectoryList(string SelectedProject)
        {
            ManageDirectoryRightsDetails model = _manageDirectoryRightsService.GetDirectoryList(UserService.CurrentUser.TTAccount.Replace("TT\\", ""), SelectedProject);
            ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();

            if (Session["objDetails"] != null)
            {
                objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
            }

            Session["OriginalListeOfDirectory"] = model.OriginalDirectoryList;
            objDetails.DirectoryAccessList = new List<DirectoryAccess>();

            Session["objDetails"] = objDetails;
            Session["DetailsToAdd"] = "";
            Session["DetailsToDelete"] = "";

            return PartialView("_DetailsDirectory", model);
        }

        public PartialViewResult GetSecurityDirectoryForProjectAndDirectory(string SelectedProject, string SelectedDirectoryId, string SelectedDirectory)
        {
            int iSelectedDirectory = 0;
            int iSelectedDirectoryAllowedPermission = 0;
            ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();

            string[] aList = SelectedDirectoryId.Split(';');

            iSelectedDirectory = int.Parse(aList[0]);
            iSelectedDirectoryAllowedPermission = int.Parse(aList[1]);

            if (Session["objDetails"] != null)
            {
                objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
            }

            if (objDetails.DirectoryAccessList.Count == 0)
            {
                ManageDirectoryRightsDetails model = _manageDirectoryRightsService.GetSecurityDirectoryForProjectAndDirectory(SelectedProject, iSelectedDirectory, SelectedDirectory, iSelectedDirectoryAllowedPermission);
                Session["objDetails"] = model;

                return PartialView("_DetailsSecurityDirectoryForProjectAndDirectory", model);
            }
            else
            {
                return PartialView("_DetailsSecurityDirectoryForProjectAndDirectory", objDetails);
            }
        }

        public string[] EmployeesArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.GetGestionnairesNames();

            return rtnlist.ToArray();
        }

        public JsonResult returnPermissionList(string selectedDirectoryRight, string SelectedDirectory, string SelectedProject, string SelectedPermission)
        {            
            List<structureItem> aList = new List<structureItem>();

            int iSelectedDirectory = 0;
            int iSelectedDirectoryAllowedPermission = 0;

            string[] aList2 = SelectedDirectory.Split(';');

            iSelectedDirectory = int.Parse(aList2[0]);
            iSelectedDirectoryAllowedPermission = int.Parse(aList2[1]);

            if (iSelectedDirectoryAllowedPermission == 2)
            {
                structureItem aPermission = new structureItem();
                if (Session["lang"].ToString() == "fr")
                {
                    aPermission.Name = "Écriture";
                }
                else
                {
                    aPermission.Name = "Write";
                }

                aPermission.Value = "2";
                aList.Add(aPermission);
                if (Session["SelectedPermissionInScreen"] == null)
                {
                    Session["SelectedPermissionInScreen"] = "2"; // valeur par défaut si utilisateur ne change pas la valeur
                }

                aPermission = new structureItem();
                if (Session["lang"].ToString() == "fr")
                {
                    aPermission.Name = "Lecture";
                }
                else
                {
                    aPermission.Name = "Read";
                }
                aPermission.Value = "1";
                aList.Add(aPermission);
            }
            else
            {
                structureItem aPermission = new structureItem();
                if (Session["lang"].ToString() == "fr")
                {
                    aPermission.Name = "Lecture";
                }
                else
                {
                    aPermission.Name = "Read";
                }
                aPermission.Value = "1";
                aList.Add(aPermission);
                Session["SelectedPermissionInScreen"] = "1"; // valeur par défaut si utilisateur ne change pas la valeur
            }

            return Json(aList, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult AddEmployeeToDirectoryRight(string strProjectNumber, string strSelectedEmployeeFullName, string strSelectedEmployeeNumber)            
        {
            string success = "true";
            string message = "";

            try
            {
                ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();
                OperationStatus operationStatus = new OperationStatus { Status = false };
                string strTTAccount = "";                
                string strDetailsToAdd = "";
                int SelectedFolderInScreen = 0;
                string SelectedPermissionInScreen = "2";                
                        
                if (strSelectedEmployeeNumber.Length == 0)
                {
                    strSelectedEmployeeNumber = _manageDirectoryRightsService.ObtenirEmployeeNumber(strSelectedEmployeeFullName);
                }

                strTTAccount = _manageDirectoryRightsService.ObtenirCompteTT(strSelectedEmployeeNumber).Replace("TT\\", "").ToUpper();

                if (Session["SelectedPermissionInScreen"] != null)
                {
                    SelectedPermissionInScreen = Session["SelectedPermissionInScreen"].ToString();
                }

                if (Session["objDetails"] != null)
                {
                    objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
                }

                DirectoryAccess aDA2 = new DirectoryAccess();
                aDA2 = objDetails.DirectoryAccessList.Find(x => x.Value.Contains(strTTAccount.ToUpper()) && x.SecurityFolderPermissionId.ToString() == SelectedPermissionInScreen);

                if (aDA2 == null)
                {
                    DirectoryAccess aDA = new DirectoryAccess();
                    aDA.Value = strTTAccount;
                    aDA.AccesTetraLinx = "";
                    if (Session["lang"].ToString() == "fr")
                    {
                        aDA.Permission = (SelectedPermissionInScreen == "1" ? "Lecture" : "Écriture");
                    }
                    else
                    {
                        aDA.Permission = (SelectedPermissionInScreen == "1" ? "Read" : "Write");
                    }
                    aDA.SecurityFolderPermissionId = System.Int32.Parse(SelectedPermissionInScreen);

                    if (Session["SelectedFolderInScreen"] != null)
                    {
                        SelectedFolderInScreen = (int)Session["SelectedFolderInScreen"];
                    }
                    aDA.RepId = SelectedFolderInScreen;
                    aDA.SecurityAuthentificationTypeID = 3;
                    objDetails.DirectoryAccessList.Add(aDA);
                    objDetails.DirectoryAccessList.OrderBy(s => s.Value);
                    operationStatus.Status = true;
                    operationStatus.Message = string.Format("", strTTAccount.Replace("TT", ""));

                    if (Session["DetailsToAdd"] != null)
                    {
                        strDetailsToAdd = Session["DetailsToAdd"].ToString();
                    }
                    if (strDetailsToAdd.Length > 0)
                    {
                        strDetailsToAdd = strDetailsToAdd + ";";
                    }
                    strDetailsToAdd = strDetailsToAdd + SelectedPermissionInScreen + "," + strTTAccount;
                    Session["DetailsToAdd"] = strDetailsToAdd;
                    Session["objDetails"] = objDetails;
                    message = ManageDirectoryRightsRessources.AccessAdded;
                }
                else
                {
                    success = "false";
                    message = ManageDirectoryRightsRessources.AccessAlreadyInDatabase;
                }


                return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                success = "false";
                return Json(new { success = success, message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemoveEmployeeFromDirectoryRight(EmployeeDirectoryRight data)
        {
            ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();
            OperationStatus operationStatus = new OperationStatus { Status = false };
            string strProjectNumber = "";

            try
            {
                DirectoryAccess aDA = new DirectoryAccess();
                string strDetailsToDelete = "";
                string strDetailsToDeleteForThisCall = "";
                strProjectNumber = data.SelectedProject;

                if (Session["objDetails"] != null)
                {
                    objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
                }

                aDA = objDetails.DirectoryAccessList.Find(x => x.Value.Contains(data.SelectedEmployeeName) && x.Permission.ToUpper() == data.SelectedPermissionFromEmployeeObj.ToUpper());
                if (aDA != null)
                {
                    objDetails.DirectoryAccessList.Remove(aDA);
                    Session["objDetails"] = objDetails;

                    if (Session["DetailsToDelete"] != null)
                    {
                        strDetailsToDelete = Session["DetailsToDelete"].ToString();
                    }

                    if (strDetailsToDelete.Length > 0)
                    {
                        strDetailsToDelete = strDetailsToDelete + ";";
                    }

                    if (data.SelectedPermissionFromEmployeeObj.ToUpper() == "LECTURE")
                    {
                        strDetailsToDeleteForThisCall = "1," + data.SelectedEmployeeName;
                    }
                    else
                    {
                        strDetailsToDeleteForThisCall = "2," + data.SelectedEmployeeName;
                    }
                    strDetailsToDelete = strDetailsToDelete + strDetailsToDeleteForThisCall;

                    Session["DetailsToDelete"] = strDetailsToDelete;
                }
            
                operationStatus.Status = true;
                operationStatus.Message = string.Format("", "");

                if (!operationStatus.Status)
                    ModelState.AddModelError("Error", Resources.TTProjetPlusResource.ErrorMessageOther);
            }
            catch (Exception ex)
            {
                _HomeService.addErrorLog(ex, "ManageDirectoryRightsController", "RemoveEmployeeFromDirectoryRight", strProjectNumber);
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new
            {
                selectedGroup = data.SelectedDirectoryRight,
                success = operationStatus.Status,
                successmessage = operationStatus.Message,
                errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRightLevel(string directory)
        {
            string aRightLevel = "";
            string sRepCodePrec = "";
            bool trouve = false;
            List<TTProjetDirectory> OriginalListeOfDirectory = new List<TTProjetDirectory>();

            if (Session["OriginalListeOfDirectory"] != null)
            {
                OriginalListeOfDirectory = (List<TTProjetDirectory>)Session["OriginalListeOfDirectory"];
            }

            foreach (TTProjetDirectory item in OriginalListeOfDirectory)
            {
                if (trouve & item.RepCode != sRepCodePrec)
                {
                    break;
                }

                if (item.RepCode == directory)
                {
                    if (aRightLevel.Length > 0)
                    {
                        aRightLevel = aRightLevel + " et " + item.TTAccountRole;
                    }
                    else
                    {
                        aRightLevel = item.TTAccountRole;
                    }
                    trouve = true;
                }
                sRepCodePrec = item.RepCode;
            }

            return Json(new { stringReturn = aRightLevel }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult keepPermissionValue(string permission, string selectedFolder)
        {
            int iSelectedDirectory = 0;
            int iSelectedDirectoryAllowedPermission = 0;

            string[] aList2 = selectedFolder.Split(';');

            iSelectedDirectory = int.Parse(aList2[0]);
            iSelectedDirectoryAllowedPermission = int.Parse(aList2[1]);

            Session["SelectedFolderInScreen"] = iSelectedDirectory;
            Session["SelectedPermissionInScreen"] = permission;

            return Json(new { stringReturn = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult resetObjDetails()
        {
            ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();

            if (Session["objDetails"] != null)
            {
                objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
                objDetails.DirectoryAccessList = new List<DirectoryAccess>();
            }

            Session["objDetails"] = objDetails;
            Session["DetailsToAdd"] = "";
            Session["DetailsToDelete"] = "";

            return Json(new { stringReturn = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult saveToDatabase(string SelectedProject, string SelectedDirectory)
        {
            ManageDirectoryRightsDetails objDetails = new ManageDirectoryRightsDetails();
            Boolean result = false;
            int iSelectedDirectory = 0;
            string[] aList = SelectedDirectory.Split(';');
            string sMessage = "";
            string strDetailsToAdd = "";
            string strDetailsToDelete = "";

            try
            {
                iSelectedDirectory = int.Parse(aList[0]);

                if (Session["DetailsToAdd"] != null)
                {
                    strDetailsToAdd = Session["DetailsToAdd"].ToString();
                }
                if (Session["DetailsToDelete"] != null)
                {
                    strDetailsToDelete = Session["DetailsToDelete"].ToString();
                }

                result = _manageDirectoryRightsService.AddAndRemoveEmployeeToDirectoryRight(SelectedProject, iSelectedDirectory, strDetailsToAdd, strDetailsToDelete);

                Session["DetailsToAdd"] = "";
                Session["DetailsToDelete"] = "";

                if (Session["objDetails"] != null)
                {
                    objDetails = (ManageDirectoryRightsDetails)Session["objDetails"];
                    objDetails.DirectoryAccessList = new List<DirectoryAccess>();
                }
                Session["objDetails"] = objDetails;

                result = true;
            }
            catch (Exception ex)
            {
                sMessage = "Erreur lors de la sauvegarde";
                _HomeService.addErrorLog(ex, "ManageDirectoryRightsController", "saveToDatabase", SelectedProject);
                throw;
            }

            return Json(new { success = result, message = sMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}