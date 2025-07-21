using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class ManageDirectoryRightsService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public List<SelectListItem> GetDirectoryRightsList()
        {    
            List<SelectListItem> aList = new List<SelectListItem>();

            SelectListItem aItem;

            aItem = new SelectListItem();
            aItem.Text = "Gestions des droits du modèle TQE";
            aItem.Value= "Gestions des droits du modèle TQE";
            aList.Add(aItem);
            //aItem = new SelectListItem();
            //aItem.Text = "Gestions des droits du modèle QIB - 10CONFIDENTIEL";
            //aItem.Value = "Gestions des droits du modèle QIB - 10CONFIDENTIEL";
            //aList.Add(aItem);
            //aItem = new SelectListItem();
            //aItem.Text = "Gestions des droits du modèle QIB - 60DXT";
            //aItem.Value = "Gestions des droits du modèle QIB - 60DXT";
            //aList.Add(aItem);

            return aList;
        }

        public ManageDirectoryRightsDetails GetProjectList(string CurrentUser)
        {
            List<SelectListItem> aList = new List<SelectListItem>();
            
            ManageDirectoryRightsDetails result = new ManageDirectoryRightsDetails { SelectedDirectoryRight = CurrentUser, ProjectList = new List<SelectListItem>(), SelectedProject = "", DirectoryList = new List<TTProjetDirectory>() };

            if (!string.IsNullOrEmpty(CurrentUser))
            {
                result.ProjectList = _entitiesBPR.usp_GetSecurityDirectoryProjectList(CurrentUser).Select(i => new SelectListItem { Text = i.PRO_ID, Value = i.PRO_ID }).ToList();
            }
            return result;
        }

        public ManageDirectoryRightsDetails GetDirectoryList(string CurrentUser, string SelectedProject)
        {
            List<TTProjetDirectory> aList = new List<TTProjetDirectory>();
            List<TTProjetDirectory> aListToReturn = new List<TTProjetDirectory>();            
            string repCodePrec = "";
            int? securityFolderPermissionIDPrec = 0;            

            ManageDirectoryRightsDetails result = new ManageDirectoryRightsDetails { SelectedDirectoryRight = "", ProjectList = new List<SelectListItem>(), SelectedProject = SelectedProject, DirectoryList = new List<TTProjetDirectory>() };            

            if (!string.IsNullOrEmpty(SelectedProject))
            {
                aList = _entitiesBPR.usp_GetSecurityDirectoryFoldersList(CurrentUser, SelectedProject).Select(i => new TTProjetDirectory { RepId = i.REP_ID, RepCode = i.REP_Code + " - " + i.REP_Description, RepDescription = i.REP_Description, SecurityFolderPermissionId = i.Security_Folder_Permission_ID, RepIdAndSecurityFolderPermissionId = i.REP_ID.ToString() + ";" + i.Security_Folder_Permission_ID.ToString(), TTAccountRole= i.TTAccountRole }).ToList();

                foreach (TTProjetDirectory item in aList)
                {
                    if (repCodePrec != "")
                    {
                        if (repCodePrec != item.RepCode)
                        {
                            aListToReturn.Add(item);
                        }                                                
                    }
                    else
                    {
                        // 1er de la list
                        aListToReturn.Add(item);
                    }
                    repCodePrec = item.RepCode;
                    securityFolderPermissionIDPrec = item.SecurityFolderPermissionId;                    
                }

                result.OriginalDirectoryList = aList;
                result.DirectoryList = aListToReturn;                
            }
            return result;
        }

        public ManageDirectoryRightsDetails GetSecurityDirectoryForProjectAndDirectory(string selectedProject, int selectedDirectoryId, string selectedDirectory, int SelectedDirectoryAllowedPermission)
        {
            try
            {               
                ManageDirectoryRightsDetails result = new ManageDirectoryRightsDetails { SelectedDirectoryRight = "", ProjectList = new List<SelectListItem>(), SelectedProject = selectedProject, DirectoryList = new List<TTProjetDirectory>(), DirectoryAccessList = new List<DirectoryAccess>() };             

                var query = from p in _entitiesBPR.usp_GetSecurityDirectoryForProject(selectedDirectoryId, selectedProject)
                            select new DirectoryAccess { RepId = p.rep_id, AccesTetraLinx = p.AccesTetraLinx, Permission = p.Permission, ProjectSecurity = int.Parse(p.sec_prj), RepCode = p.REP_Code, SecurityFolderPermissionId = p.Security_Folder_Permission_ID, Value = (p.TheValue != null ? p.TheValue.ToUpper(): ""), SecurityAuthentificationTypeID = p.Security_Authentification_Type_ID, Rn = p.rn };

                result.SelectedDirectory = selectedDirectory;
                result.SelectedDirectoryId = selectedDirectoryId;
                result.DirectoryAccessList = query.ToList();

                return result;
            }
            catch (Exception e)
            {
                string anError = e.InnerException.ToString();
                return null;
            }
        }

        public Boolean AddAndRemoveEmployeeToDirectoryRight(string projectNumber,int repId, string strDetailsToAdd, string strDetailsToDelete)
        {
            //string strValue = "";
            int strValue = 0;
            bool success = false;
            bool updateFolderSecurity = false;

            try
            {                
                //Nullable<byte> DoDelBatFile = 1;                                            

                if (strDetailsToAdd.Length > 0)
                {
                    _entitiesBPR.usp_Add_SecurityDirectoryRightsList(projectNumber, repId, strDetailsToAdd);
                    addLog(UserService.AuthenticatedUser.TTAccount, "Ajout accès répertoire " + ObtenirRepCode(repId) + " pour " + strDetailsToAdd, projectNumber);
                }

                if (strDetailsToDelete.Length > 0)
                {
                    _entitiesBPR.usp_Delete_SecurityDirectoryRightsList(projectNumber, repId, strDetailsToDelete);
                    addLog(UserService.AuthenticatedUser.TTAccount, "Suppression accès répertoire " + ObtenirRepCode(repId) + " pour " + strDetailsToDelete, projectNumber);
                }

                if (UserService.isPRODEnvironment()==1)
                {
                    updateFolderSecurity = true;                   
                }
                else
                {
                    // Si c'est un projet sur le serveur test, on maj la sécurité des répertoires du projet
                    if (_entitiesBPR.Projets.Where(x => x.PRO_Id == projectNumber).Select(p => p.SER_Id).FirstOrDefault() == 68)
                    {
                        updateFolderSecurity = true;
                    }
                }

                if (updateFolderSecurity)
                {
                    _entitiesBPR.Database.CommandTimeout = 480; //8 min
                    strValue = 1;//_entitiesBPR.usp_NEW_TT_Security_INPROGRESS_TTProjetPlus(projectNumber, DoDelBatFile);
                }

                return success;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("denied"))
                    throw new Exception(Resources.TTProjetPlusResource.AccessDenied);
                else
                    throw (ex);
            }
        }

        private string ObtenirRepCode(int repId)
        {
            string retour = "";
            switch (repId)
            {
                case 2000:
                    retour = "10";
                    break;
                case 2001:
                    retour = "11";
                    break;
                case 2002:
                    retour = "12";
                    break;
                case 2003:
                    retour = "INV";
                    break;
                case 2004:
                    retour = "13";
                    break;
                case 2005:
                    retour = "20";
                    break;
                case 2006:
                    retour = "20EME";
                    break;
                case 2007:
                    retour = "20EMR";
                    break;
                case 2032:
                    retour = "70";
                    break;
                case 2033:
                    retour = "71";
                    break;
                case 2034:
                    retour = "72";
                    break;
                case 2035:
                    retour = "73";
                    break;
                case 2036:
                    retour = "74";
                    break;
                case 2037:
                    retour = "75";
                    break;
                case 2038:
                    retour = "76";
                    break;
                default:
                    retour = repId.ToString();
                    break;
            }

            return retour;
        }

        public string ObtenirCompteTT(string employeeid)
        {
            var directoryService = new DirectoryService();
            var strTTAccount = "";
            var principal = directoryService.FindUserByEmployeeId(employeeid);

            if (principal != null)
            {
                strTTAccount = principal.TTAccount;
            }
            return strTTAccount;
        }



        public string ObtenirEmployeeNumber(string fullName)
        {
            var now = DateTime.Now;
            var employeNumber = _entities.EmployeeActif_Inactive.Where(p => p.FULL_NAME == fullName && p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).
                        Select(p => p.EMPLOYEE_NUMBER).FirstOrDefault();
            return employeNumber;
        }



        public void addLog(string strSubmitBy, string strInfo, string projectId)
        {
            var entity = new Security_Directory_Logs();
            entity.SubmitBy = strSubmitBy;
            entity.Err_UTCDateTime = DateTime.Now;
            entity.Information = strInfo;
            entity.ProjectNo = projectId;
            _entitiesBPR.Security_Directory_Logs.Add(entity);
            _entitiesBPR.SaveChanges();
        }

    }
}