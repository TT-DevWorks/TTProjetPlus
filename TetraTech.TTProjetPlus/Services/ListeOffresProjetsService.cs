using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class ListeOffresProjetsService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public List<OSProjectModel> returnListOfOffersForUser(string userId, string type, string active)
        {
                List<OSProjectModel> liste = new List<OSProjectModel>();
            try

            {
                var OffersList = _entities.usp_getOffersOrProjectsForUser(userId, type, active).ToList();
                foreach (usp_getOffersOrProjectsForUser_Result item in OffersList)
                {
                    OSProjectModel itemOSProject = new OSProjectModel();
                    itemOSProject.OS_Project_ID = item.OS_Project_ID;
                    itemOSProject.Project_Number = item.number;
                    itemOSProject.Project_Description = item.title;
                    itemOSProject.Organisation = item.organisation;
                    itemOSProject.GestionnaireProjet = item.gestionnaireProjet;
                    itemOSProject.Program_Mgr_Name = item.Program_Mgr;
                    itemOSProject.Initiated_By_Name = item.InitiatedBy;
                    itemOSProject.StatusName = item.statutProjet;
                    itemOSProject.StatusDocName = item.statutDoc;
                    itemOSProject.Acct_Group_Name = item.UA;
                    itemOSProject.Doc_PostponedDate = (item.Doc_PostponedDate.ToString() == "" || item.Doc_PostponedDate == null) ? "" : String.Format("{0:yyyy-MM-dd}", item.Doc_PostponedDate);
                    itemOSProject.Customer_Name = item.client;
                    itemOSProject.path = item.Chemin_Projet;
                    itemOSProject.Comments = item.Comments;
                    itemOSProject.submittedDate = (item.SubmittedDate.ToString() == "" || item.SubmittedDate == null) ? "" : String.Format("{0:yyyy-MM-dd}", item.SubmittedDate);
                    itemOSProject.submittedDateForSorting = item.SubmittedDate;
                    itemOSProject.Project_Mgr_Number = item.Project_Mgr_Number;
                    itemOSProject.Client_ProjectManager = (item.Client_ProjectManager == null || item.Client_ProjectManager == "" || item.Client_ProjectManager == "NULL") ? "" : item.Client_ProjectManager;
                    itemOSProject.Client_Email_ProjectManager = (item.Client_Email_ProjectManager == null || item.Client_Email_ProjectManager == "" || item.Client_Email_ProjectManager == "NULL") ? "" : item.Client_Email_ProjectManager;

                    var pattern = @"
                    Informations standards (compléter les informations et expédier aux personnes concernées)%0D%0A
                    ------------------------------ %0D%0A
                    Numéro de projet : {0} %0D%0A
                    Titre du projet : {1}%0D%0A
                    Client # : {2}%0D%0A
                    Compagnie: {3}%0D%0A
                    Unité d'affaires : {4}%0D%0A
                    Organisation : {5}%0D%0A
                    Initié par : {6}%0D%0A
                    Références : {7}%0D%0A
                    Gestionnaire de projet :  {8}%0D%0A
                    Gestionnaire de programme  : {9}%0D%0A
                    Attribué le : {10}%0D%0A
                    %0D%0A
                    Informations supplémentaires facultatives selon la politique de chaque unité d’affaires %0D%0A
                    ------------------------------- %0D%0A
                    Résultat de l’analyse de risque :%0D%0A
                    Si le niveau de risque est jaune ou supérieur ou si le client est nouveau ou si le montant des honoraires estimé risque d’être supérieur à 100 000 $ les informations supplémentaires suivantes sont requises :%0D%0A
                    Mandat :%0D%0A
                    Type de service (horaire, forfait, etc.) :%0D%0A
                    Montant des honoraires estimés approximatif :%0D%0A 
                    Potentiel de marge brute :%0D%0A
                    Date probable de l’octroi du contrat :%0D%0A
                    Niveau de probabilité d’obtention du contrat :%0D%0A 
                    Description des risques majeurs du projet :%0D%0A 
                    Stratégie d’entreprise (motifs pour lesquels on devrait faire cette offre) :%0D%0A
                    Notes et recommandations : 
                    ";


                    itemOSProject.emailContent = string.Format(pattern,
                        item.number,
                        item.title,
                        item.clientNumber + " (" + item.client + ")",
                        item.Company,
                        item.UA,
                        //item.organisation = item.organisation.Contains("&") == true ? item.organisation.Replace("&", "%26") : item.organisation,
                        (item.organisation == "" || item.organisation == null) ? "" : item.organisation.Contains("&") == true ? item.organisation.Replace("&", "%26") : item.organisation,
                        (item.InitiatedBy == "" || item.InitiatedBy == null) ? "" : item.InitiatedBy,
                        (item.Comments == "" || item.Comments == null) ? "" : item.Comments,
                        (item.gestionnaireProjet == "" || item.gestionnaireProjet == null) ? "" : item.gestionnaireProjet,
                        (item.Program_Mgr == "" || item.Program_Mgr == null) ? "" : item.Program_Mgr,
                        (item.SubmittedDate.ToString() == "" || item.SubmittedDate == null) ? "" : String.Format("{0:yyyy-MM-dd}", item.SubmittedDate)

                        );
                    liste.Add(itemOSProject);
                }
            }

            catch (Exception e)
            {
                addErrorLog(e, "ListeOffresProjetsService", "returnListOfOffersForUser");
                return null;
            }

                return liste;
        }

        public OSProjectModel returnOfferProject(string id)
        {
            var itemForModal = from p in _entities.usp_GetProjectOrOSDetails(id)
                               select new OSProjectModel
                               {
                                   OS_Project_ID = p.OS_Project_ID,
                                   Project_Number = p.Project,
                                   Project_Description = p.Project_Description,
                                   Project_Master_ID = p.Project_Master,
                                   PhaseCode = p.PhaseCode,
                                   Customer_Number = p.Customer_Number,
                                   Customer_Name = p.Customer_Name,
                                   Company = p.Company,
                                   Acct_Group_ID = p.UA_ID.ToString(),
                                   Acct_Group_Name = p.UniteAffaire,
                                   Acct_Center_ID = p.ORG_ID.ToString(),
                                   Acct_Center_Name = p.Organisation,
                                   Project_Mgr_Number = p.Project_Mgr_Number,
                                   Project_Mgr_Name = p.Project_Mgr,
                                   Program_Mgr_Number = p.Program_Mgr_Number,
                                   Program_Mgr_Name = p.Program_Mgr,
                                   End_Client_Name = p.End_Client_Name,
                                   Initiated_By = p.InitiatedByNumber,
                                   Approx_Amount = p.Approx_Amount,
                                   Comments = p.Comments,
                                   StatusName = p.statut,
                                   StatusDocName = p.Doc_status,
                                   dDoc_PostponedDate = p.Doc_PostponedDate,
                                   Doc_PostponedDate = (p.Doc_PostponedDate.ToString() == "" || p.Doc_PostponedDate == null) ? "" : String.Format("{0:yyyy-MM-dd}", p.Doc_PostponedDate),
                                   Last_Modification_Date = (p.Last_Modification_Date.ToString() == "" || p.Last_Modification_Date == null) ? "" : String.Format("{0:yyyy-MM-dd}", p.Last_Modification_Date),
                                   Last_Modification_User = p.LastModifiedBy,
                                   //submittedDate = (p.SubmittedDate.ToString() =="" || p.SubmittedDate == null)? "(donnée indisponible)" :DateTime.ParseExact(p.SubmittedDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString(),
                                   submittedDate = (p.SubmittedDate.ToString() == "" || p.SubmittedDate == null) ? "" : String.Format("{0:yyyy-MM-dd}", p.SubmittedDate),
                                   Initiated_By_Name = p.InitiatedBy,
                                   Organisation = p.Organisation,
                                   ProjectModId = p.Modele,
                                   Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                                   Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager
                               };

            var modelTosend = itemForModal.Where(p => p.OS_Project_ID.ToString() == id).FirstOrDefault();
            //var datePst = "";
            //if (modelTosend.Doc_PostponedDate.ToString() != "")
            //{
            //    DateTime fullDate = (DateTime)modelTosend.dDoc_PostponedDate;
            //    var jour = fullDate.Day;
            //    var mois = fullDate.Month;
            //    var moisF = "";
            //    var annee = fullDate.Year;

            //    if (mois == 1) { moisF = "Janvier"; }
            //    if (mois == 2) { moisF = "Février"; }
            //    if (mois == 3) { moisF = "Mars"; }
            //    if (mois == 4) { moisF = "Avril"; }
            //    if (mois == 5) { moisF = "Mai"; }
            //    if (mois == 6) { moisF = "Juin"; }
            //    if (mois == 7) { moisF = "Juillet"; }
            //    if (mois == 8) { moisF = "Août"; }
            //    if (mois == 9) { moisF = "Septembre"; }
            //    if (mois == 10) { moisF = "Octobre"; }
            //    if (mois == 11) { moisF = "Novembre"; }
            //    if (mois == 12) { moisF = "Décembre"; }

            //    datePst = jour + " " + moisF + " " + annee;
            //    modelTosend.FDoc_PostponedDate = datePst;
            //}

            var companyId = _entities.Companies.Where(p => p.company1 == modelTosend.Company).Select(p => p.Company_id).FirstOrDefault();
            var listeUAForCompany = from p in _entities.Acct_Group
                                    where (p.Company_id.ToString() == companyId.ToString())
                                    select new structureItem
                                    {
                                        Name = p.Acct_Group1,
                                        Value = p.Acct_Group_ID.ToString()
                                    };
            modelTosend.ListeUAForCompany = listeUAForCompany.ToList();

            var listeORGForUA = from p in _entities.Acct_Center
                                where (p.Acct_Group_ID.ToString() == modelTosend.Acct_Group_ID)
                                select new structureItem
                                {
                                    Name = p.Acct_Center1,
                                    Value = p.Acct_Center_ID.ToString()
                                };
            modelTosend.ListeORGForUA = listeORGForUA.ToList();

            //modelTosend.ProjectModId = "un modèle";

            //modelTosend.ProjectModId = from p in _entitiesBPR.Projet   
            //                           join m in _entitiesBPR.Modele on p.MOD_SEC_Id = m.
            //                           where (p.PRO_Id.ToString() == id)
            //                           select n

            return modelTosend;
        }

        public string returnOSProjectId(string Project_Number)
        {            
            OS_Project aproject = _entities.OS_Project.Where(pr => pr.Project_Number == Project_Number).FirstOrDefault();

            return aproject.OS_Project_ID.ToString();
        }


            public List<structureItem> returnStatusList()
        {
            var statusList = from p in _entities.Status
                             where (p.Status_ID == 1 || p.Status_ID == 2 || p.Status_ID == 3)
                             select new structureItem
                             {
                                 Name = p.Description_FR,
                                 Value = p.Status_ID.ToString()
                             };
            return statusList.ToList();
        }

        public List<structureItem> returnDocStatusList()
        {
            var docStatusList = from p in _entities.Doc_Status
                                select new structureItem
                                {
                                    Name = p.Description_FR,
                                    Value = p.Doc_Status_ID.ToString()
                                };
            return docStatusList.ToList();
        }

        public string updateOSProjectTable(string type, string projectId, string title, string customer, string company, string businessUnit,
            string org, string projectMgr, string programMgr, string finalClient, string approximateAmount,
            string status, string statusDoc, string dateArchivage, string dateRepport, string initiatedBy, string comments, string NamePM, string EmailPM)
        {
            try
            {
                var projectToUpdate = _entities.OS_Project
               .First(p => p.OS_Project_ID.ToString() == projectId);

                if (type == "OS")
                {
                    var amount = approximateAmount.Split(' ')[0];
                    string correctedAmount = new string(amount.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
                    //update du projet

                    projectToUpdate.Customer_Number = customer;
                    projectToUpdate.Customer_Name = _entities.Customers.Where(p => p.Customer_Number == customer).Select(p => p.Customer_Name).FirstOrDefault();
                    projectToUpdate.Project_Description = title;
                    projectToUpdate.Company = company;
                    projectToUpdate.Acct_Group_ID = Int32.Parse(businessUnit);
                    projectToUpdate.Acct_Center_ID = Int32.Parse(org);
                    projectToUpdate.Project_Mgr_Number = projectMgr;
                    projectToUpdate.End_Client_Name = finalClient;
                    projectToUpdate.Approx_Amount = Int32.Parse(correctedAmount);
                    //projectToUpdate.Comments = refAndComments;
                    projectToUpdate.Status_ID = status == "" ? (Byte?)null : Convert.ToByte(status);
                    projectToUpdate.Doc_Status_ID = statusDoc == "" ? (Byte?)null : Convert.ToByte(statusDoc);
                    //projectToUpdate.Archive_date = dateArchivage == "" ? projectToUpdate.Archive_date : Convert.ToDateTime(dateArchivage);
                    //projectToUpdate.Doc_PostponedDate = dateRepport == "" ? projectToUpdate.Doc_PostponedDate : Convert.ToDateTime(dateRepport);
                    projectToUpdate.Doc_PostponedDate = dateRepport == "" ? (DateTime?)null : Convert.ToDateTime(dateRepport);
                    projectToUpdate.Last_Modification_Date = DateTime.Now;
                    projectToUpdate.Last_Modification_User = UserService.CurrentUser.EmployeeId;
                    projectToUpdate.Initiated_By = initiatedBy;
                    projectToUpdate.Program_Mgr_Number = programMgr;
                    projectToUpdate.Comments = comments;
                    projectToUpdate.Client_ProjectManager = NamePM;
                    projectToUpdate.Client_Email_ProjectManager = EmailPM;
                }
                if (type == "PR")
                {
                    //projectToUpdate.Comments = refAndComments;
                    //projectToUpdate.Archive_date = dateArchivage == "" ? projectToUpdate.Archive_date : Convert.ToDateTime(dateArchivage);
                    //projectToUpdate.Doc_PostponedDate = dateRepport == "" ? projectToUpdate.Doc_PostponedDate : Convert.ToDateTime(dateRepport);
                    projectToUpdate.Doc_PostponedDate = dateRepport == "" ? (DateTime?)null : Convert.ToDateTime(dateRepport);
                    projectToUpdate.Last_Modification_Date = DateTime.Now;
                    projectToUpdate.Last_Modification_User = UserService.CurrentUser.EmployeeId;
                    projectToUpdate.Doc_Status_ID = statusDoc == "" ? (Byte?)null : Convert.ToByte(statusDoc);
                    projectToUpdate.Comments = comments;
                    projectToUpdate.Client_ProjectManager = NamePM;
                    projectToUpdate.Client_Email_ProjectManager = EmailPM;
                }
                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "success";
        }

        public string ReturnUserEmail(string EmployeeId)
        {           
            var now = DateTime.Now;
            return _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now && p.EMPLOYEE_NUMBER == EmployeeId).Select(p => p.EMAIL_ADDRESS).FirstOrDefault();
        }

        public Boolean isProjectSecurityNeedsUpdate(string proId)
        {
            Boolean projectNeedsUpdate = false;

            var result = _entitiesBPR.usp_isProjectSecurityNeedsUpdate(proId).FirstOrDefault();

            if (result == 1)
            {
                projectNeedsUpdate = true;
            }            

            return projectNeedsUpdate;
        }

        public void addErrorLog(Exception outputResult, string controller, string controllerFunction, string projectId = "")
        {
            var error = "";
            if (outputResult.InnerException != null)
            {
                error = outputResult.InnerException.ToString();
            }
            else if (outputResult.Message != null)
            {
                error = outputResult.Message.ToString();
            }
            else
            {
                error = "Erreur inconnue";
            }
            var entity = new TTProjetPlus_Error_Log();
            entity.Err_UTCDatetime = DateTime.Now;
            entity.TTAccount = UserService.AuthenticatedUser.TTAccount;
            entity.ImpersonationName = UserService.ImpersonatedUser == null ? entity.TTAccount : UserService.ImpersonatedUser.TTAccount;
            entity.ControllerName = controller;
            entity.ControllerFunction = controllerFunction;
            entity.InnerException = error;
            entity.Url = "URL: " + HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0] + " / projectId: " + projectId;
            _entities.TTProjetPlus_Error_Log.Add(entity);
            _entities.SaveChanges();
        }

    }
}
