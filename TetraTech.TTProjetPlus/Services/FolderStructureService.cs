using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using System.Data.SqlClient;

using System.Security.AccessControl;
using System.Data.SqlClient;

using System.Runtime.InteropServices; // DllImport
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions; // PermissionSetAttribute

namespace TetraTech.TTProjetPlus.Services
{
    public class FolderStructureService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();
        private readonly HomeService _HomeService = new HomeService();

        // obtains user token
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public string getServersUsed(string projectNumber)
        {
            //recuperer le path_partage pour le serveur de ce projet
            var id = _entitiesBPR.usp_ObtenirProjet(projectNumber).Select(x => x.SER_Id).FirstOrDefault();
            var serverUsed = _entitiesBPR.usp_ObtenirServeur(id).Select(x => x.SER_Description).FirstOrDefault();
            return serverUsed;
        }

        public ResumeItem getResumeForProject(string projectNumber)
        {
            try
            {
                var resume = _entities.usp_getProjectSummary(projectNumber).FirstOrDefault();

                if (resume != null)
                {
                    ResumeItem item = new ResumeItem();
                    item.Description_projet = resume.Project_Description;
                    item.Organisation = resume.acct_center;
                    item.Statut_projet = resume.project_status_code;
                    item.Client = resume.customer_name;
                    item.Gestionnaire_projet = resume.project_mgr_name;
                    item.Gestionnaire_programme = resume.program_mgr_name;
                    item.Agent_facturation = resume.bill_specialist_name;
                    return item;
                }
                    else return null;
                
            }
            catch (Exception e)
            {
                return null;

            }

        }

        public string ReturnModId(string projectId)
        {
            var modId = _entitiesBPR.Projets.Where(x => x.PRO_Id == projectId)
                                  .Select(x => x.MOD_SEC_Id).FirstOrDefault().ToString();
            return modId;
        }

        public string ReturnModNom(string modId)
        {
            var modNom = _entitiesBPR.Modeles.Where(x => x.MOD_Id.ToString() == modId)
                                  .Select(x => x.MOD_Nom).FirstOrDefault().ToString();
            return modNom;
        }

        public NodeItem ReturnParentFolder(NodeItem item, string modId)
        {
            NodeItem itemToReturn = new NodeItem();
            var queryID = _entitiesBPR.Repertoires
                    .Where(p => p.REP_Id.ToString() == item.nodeID  && p.MOD_Id.ToString() == modId).FirstOrDefault();

            itemToReturn.nodeID = queryID.REP_Parent.ToString();

            var queryDesc = _entitiesBPR.Repertoires
                    .Where(p => p.REP_Id.ToString() == itemToReturn.nodeID && p.MOD_Id.ToString() == modId).FirstOrDefault();

            itemToReturn.nodeDescription = queryDesc.REP_Description;

            return itemToReturn;
        }

        public void AddFoldersToProjectOnDB(List<NodeItem> listOfFolderToAdd, string projectID)
        {
            foreach (NodeItem item in listOfFolderToAdd)
            {

                var entity = new Projet_Repertoire();

                entity.PRO_Id = projectID;
                entity.REP_Id = Int32.Parse(item.nodeID);
                _entitiesBPR.Projet_Repertoire.Add(entity);

            }
            _entitiesBPR.SaveChanges();
        }

        public string ReturnRepCodeForRepID(string repID, string modId)
        {
            var parentId = _entitiesBPR.Repertoires.Where(x => x.REP_Id.ToString() == repID && x.MOD_Id.ToString() == modId)
                                  .Select(x => x.REP_Code).FirstOrDefault().ToString();
            return parentId;
        }

        public string checkFoldersForProject(string projectId)
        {            
            var projectModelSelec = _entitiesBPR.usp_ObtenirModeleParProjet(projectId).Select(p => p.MOD_Id).FirstOrDefault();         
            Dictionary<int?, int> DicTQEDocs = new Dictionary<int?, int>();
            List<TQEDocsToInsertInSubForlders_Sly> listForTQE = new List<TQEDocsToInsertInSubForlders_Sly>();
            if (projectModelSelec == 30)//TQE
            {
                listForTQE = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.ToList();

            }

            //1. recuperer tous les paths en fonctions des repertoires pour ce projet dans la bd:   
            
            int seqID = _entitiesBPR.Database.SqlQuery<int>("EXEC usp_ObtenirRepertoireParProjetTTProjetPlusNasuni @ProjetID, @modId",
                                           new SqlParameter("ProjetID", projectId),
                                           new SqlParameter("modId", projectModelSelec.ToString()))
                                           .SingleOrDefault();


            var listPaths = _entitiesBPR.tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni.Where(p => p.seqID == seqID).ToList();

            //2.  si ces paths n'existent pas sur le serveur on les crée:
            try
            {                
                foreach (tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni item in listPaths)
                //foreach (usp_ObtenirRepertoireParProjetTTProjetPlus_Result item in listPaths)
                {
                    
                    bool exists = System.IO.Directory.Exists(item.Folder);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(item.Folder);                        
                    }                    
                    
                    if (projectModelSelec == 30)
                    {
                        foreach (TQEDocsToInsertInSubForlders_Sly item2 in listForTQE)
                        {
                            if (item.REP_Id == item2.SousRep_id)
                            {
                                //injection; par exemple:
                                try
                                {
                                    System.IO.File.Copy(item2.NumeroDoc, item.Folder +"\\"+ item2.NumeroDoc.Split('\\')[6]);

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    _HomeService.SetFolderPermission_TTProjet(item.Folder, item.compte, item.Security);
                }

                //_HomeService.addSecurityOn10CONFIDENTIELFolder(projectId);
                _entitiesBPR.usp_Delete_tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni(seqID);
                return "success";
            }
            catch (Exception e)
            {
            _HomeService.addErrorLog(e, "FolderStructureService", "checkFoldersForProject", projectId);
            return e.InnerException.ToString();
             }

        }

        public bool ModifyServer(string projectNumber, string newServer)
        {
            try
            {                

               var itemproject = _entitiesBPR.Projets.Where(p => p.PRO_Id == projectNumber).FirstOrDefault();
                itemproject.SER_Id  = Int32.Parse(newServer); 
                _entitiesBPR.SaveChanges();
             
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        

        public bool AddEmp60Conf(string TTAccount, string ProjectID)
        {
            try
            {
                _entitiesBPR.usp_AddEmp60Conf(TTAccount, ProjectID);               
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public string checkFoldersForProjectEnvDiligence(string projectId)
        {
            var projectModelSelec = _entitiesBPR.usp_ObtenirModeleParProjet(projectId).Select(p => p.MOD_Id).FirstOrDefault();
            Dictionary<int?, int> DicTQEDocs = new Dictionary<int?, int>();
            List<TQEDocsToInsertInSubForlders_Sly> listForTQE = new List<TQEDocsToInsertInSubForlders_Sly>();
            if (projectModelSelec == 30)//TQE
            {
                listForTQE = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.ToList();

            }

            //1. recuperer tous les paths en fonctions des repertoires pour ce projet dans la bd:   

            int seqID = _entitiesBPR.Database.SqlQuery<int>("EXEC usp_ObtenirRepertoireParProjetTTProjetPlusNasuni @ProjetID, @modId",
                                           new SqlParameter("ProjetID", projectId),
                                           new SqlParameter("modId", projectModelSelec.ToString()))
                                           .SingleOrDefault();


            var listPaths = _entitiesBPR.tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni.Where(p => p.seqID == seqID).ToList();

            //2.  si ces paths n'existent pas sur le serveur on les crée:
            try
            {
                foreach (tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni item in listPaths)
                //foreach (usp_ObtenirRepertoireParProjetTTProjetPlus_Result item in listPaths)
                {

                    bool exists = System.IO.Directory.Exists(item.Folder);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(item.Folder);
                    }

                    if (projectModelSelec == 30)
                    {
                        foreach (TQEDocsToInsertInSubForlders_Sly item2 in listForTQE)
                        {
                            if (item.REP_Id == item2.SousRep_id)
                            {
                                //injection; par exemple:
                                try
                                {
                                    System.IO.File.Copy(item2.NumeroDoc, item.Folder + "\\" + item2.NumeroDoc.Split('\\')[6]);

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    //_HomeService.SetFolderPermission_TTProjet(item.Folder, item.compte, item.Security);
                    _HomeService.SetFolderPermission_TTProjetEnvDiligence(item.Folder, item.compte, item.Security);
                    
                }

                //_HomeService.addSecurityOn10CONFIDENTIELFolder(projectId);
                _entitiesBPR.usp_Delete_tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni(seqID);
                return "success";
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "FolderStructureService", "checkFoldersForProject", projectId);
                return e.InnerException.ToString();
            }

        }

       
    }
}