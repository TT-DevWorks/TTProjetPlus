using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Exceptions;
using TetraTech.TTProjetPlus.Models;
using ProjectToCloseRessources = TetraTech.TTProjetPlus.Views.ProjectToClose.ProjectToCloseRessources;

namespace TetraTech.TTProjetPlus.Services
{
    public class ProjectToCloseService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        private readonly EmployeeService _employeesService = new EmployeeService();


        public ProjectToCloseModel QueryProjectToClose(string employeeid, int osProjectID, string menuVisible)
        {
            if (osProjectID == -1)
                return new ProjectToCloseModel(employeeid, osProjectID, menuVisible);
            try
            {
                var now = DateTime.Now;
                var result = _entities.usp_GetProject2CloseInfos(osProjectID)
               .Select(p => new ProjectToCloseModel(employeeid, osProjectID, menuVisible)
               {
                   ProjectInformations = new ProjectInformationsModel
                   {

                       ProjectDescription = p.ProjectDescription,

                       ProjectManager_Num = p.Project_Mgr_Number,
                       ProjectManager_Name = p.MGRName,
                       ProjectManager_Email = p.MGREmail,

                       ProgramManager_Num = p.Program_Mgr_Number,
                       ProgramManager_Name = p.ProgMGRName,
                       ProgramManager_Email = p.ProgMGREmail,

                       BillingSpecialist_Num = p.Bill_Specialist_Number,
                       BillingSpecialist_Name = p.BSName,
                       BillingSpecialist_Email = p.BSEmail,

                       ContractAdministrator_Num = p.CANumber,
                       ContractAdministrator_Name = p.CAName,
                       ContractAdministrator_Email = p.CAEmail,

                       AcctCenterName = p.Acct_Center_Name,
                       CustomerName = p.Customer_Name,
                       ProjectStartDate = p.project_start_date,
                       ProjectEndDate = p.project_end_date,                       
                       Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                       Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager,
                       //fred

                   },

                   OSProjectNumber = p.OS_ProjectNumber,
                   ProjectToCloseId = p.ProjectToCloseId ?? -1,
                   TlinxProjectId = p.TlinxProjectId ?? -1,
                   TlinxProjectNumber = p.TlinxProjectNumber,

                   StatusID = p.Status_ID ?? 0,
                   SubmittedDate = p.SubmittedDate,
                   EstimateEndDate = p.EstimateEndDate ?? p.project_end_date,
                   EstimateEndDateDefault = p.project_end_date?.ToString("yyyy-MM-dd"),

                   DocStatusID = p.Doc_Status_ID ?? 0,
                   StatusDocName = p.Description_FR,
                   DocPostponedDateTime = p.Doc_PostponedDate ?? DateTime.Now.Date,
                   DocPostponedDateDefault = DateTime.Now.Date.ToString("yyyy-MM-dd"),

                   ProjectToCloseProjectManager = p.project_mgr_num,

                   IsClosedOrPostponed = p.IsClosedOrPostponed == 1 ? true : false,
                   ToBeClosed = p.IsClosedOrPostponed == 1 ? true : false,
                   IsArchivedOrPostponed = p.IsArchivedOrPostponed == 1 ? true : false,
                   ToBeArchived = p.IsArchivedOrPostponed == 1 ? true : false,

                   CommentFermeture_Projet = p.CommentFermeture_Projet,
                   submitBy = p.SubmitBy,
                   submitByName = p.SubmitBy == null ? "" : " par " + _entities.EmployeeActif_Inactive.Where(c => c.EMPLOYEE_NUMBER == p.SubmitBy &&  c.EFFECTIVE_START_DATE <= now && c.EFFECTIVE_END_DATE >= now).Select(c => c.FULL_NAME).FirstOrDefault().Split('(')[0]

               })
               .FirstOrDefault();

                if (result == null)
                {
                    return new ProjectToCloseModel(employeeid, osProjectID, menuVisible) { AccessType = TypesEnum.NotFound };
                }
                return result;
            }
            catch (Exception e)
            {
                return new ProjectToCloseModel(employeeid, osProjectID, menuVisible) { AccessType = TypesEnum.NotFound };
            }

        }

        public ProjectToCloseViewModel GetProjectToCloseViewModel(BaseViewModel currentbaseviewmodel, string employeeid, string tlxprojectnumber, string menuVisible)
        {
            int osProjectID = HelperService.GetOSProjectIdFromTlxNumber(tlxprojectnumber);

            if (osProjectID == 0)
                return new ProjectToCloseViewModel(currentbaseviewmodel)
                {
                    Details = new ProjectToCloseModel(employeeid, osProjectID, menuVisible) { TlinxProjectNumber = tlxprojectnumber, AccessType = TypesEnum.NotFoundTlx }
                };
            else
                return new ProjectToCloseViewModel(currentbaseviewmodel)
                {
                    Details = GetProjectToCloseDetais(employeeid, osProjectID, menuVisible)
                };
        }


        public ProjectToCloseModel GetProjectToCloseDetais(string employeeid, int osProjectID, string menuVisible)
        {
            ProjectToCloseModel result = QueryProjectToClose(employeeid, osProjectID, menuVisible);

            result.AccessType = VerifyAccess(result);

            return result;

        }
        public string returnCurrentEstimatedEndDate(int ProjectToCloseId)
        {
            try
            {
                var currentEstimatedEndDate = ((DateTime)_entities.ProjectsToCloses.Where(p => p.ID == ProjectToCloseId).Select(p => p.EstimateEndDate).FirstOrDefault()).ToString("yyyy-MM-dd");
                return currentEstimatedEndDate;

            }
            catch
            {
                return "-1";
            }
        }

        public void SaveProjectToClose(string employeeid, string employeeName, ProjectToCloseModel data, out bool sendRequestCloseProject)
        {
            sendRequestCloseProject = false;

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.AccessType != TypesEnum.ProjectToClose)
                throw new Exception(data.AccessMessage);

            if (data.TlinxProjectId == -1)
                throw new Exception(ProjectToCloseRessources.TlinxProjectNull);

            data.CurrentUser = employeeid;

            try
            {
                var os_projectToSave = _entities.OS_Project.First(pr => pr.OS_Project_ID == data.OSProjectID);


                if (!data.IsClosedOrPostponed && data.ToBeClosed)
                {
                    sendRequestCloseProject = true;
                    data.SubmittedDate = DateTime.Now.Date;
                }

                //OS_Project entity: archive data and project statusid
                //if (data.ToBeClosed)
                //{
                    if (sendRequestCloseProject) os_projectToSave.Status_ID = 8;

                    if (!data.NoDocumentation)
                    {
                        os_projectToSave.Doc_Status_ID = (byte)((data.ToBeArchived) ? 4 : 3);
                        os_projectToSave.Doc_PostponedDate = data.DocPostponedDateTimeIsRequired == true ? data.DocPostponedDateTime : null;
                        os_projectToSave.Client_ProjectManager = data.ProjectInformations.Client_ProjectManager ;
                        os_projectToSave.Client_Email_ProjectManager = data.ProjectInformations.Client_Email_ProjectManager;
                    }
                //}

                //ProjectToClose entity: project status dates
                if (data.ToBeClosed == false || sendRequestCloseProject)
                {
                    CreateOrUpdateProjectToClose(
                        os_projectToSave.OS_Project_ID, os_projectToSave.Project_Mgr_Number, os_projectToSave.Project_Number
                        , os_projectToSave.Status_ID, data.EstimateEndDate, data.SubmittedDate, employeeid, true
                     );
                }

                //SuiviMandat PROJECT_COMMENT entity
                if (sendRequestCloseProject && !string.IsNullOrEmpty(data.CommentFermeture_Projet))
                {
                    DateTime periodweekdate = HelperService.GetCurrentPeriodWeekDate();
                    string employeerole = (os_projectToSave.Project_Mgr_Number == employeeid) ? "PM" : (os_projectToSave.Bill_Specialist_Number == employeeid) ? "AF" : "";
                    int versionid = 0;
                    HelperService.CreateCommentFermeture_Projet(data.TlinxProjectId, periodweekdate, versionid, data.CommentFermeture_Projet, employeeid, employeerole, employeeName);
                }

                _entities.SaveChanges();
                return;
            }
            catch (DuplicateItemException)
            {
                throw new Exception(ProjectToCloseRessources.DuplicateComment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SendProject2CloseEmail(int osProjectID)
        {
            try
            {
                var sendmail = _entities.usp_SendProject2CloseEmail();

                return _entities.ProjectsToCloses.Where(ptc => ptc.ID == osProjectID && ptc.EmailSent == true).Select(ptc => ptc.EmailID).FirstOrDefault() ?? -1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int SendProjectEstimatedEndDate(int osProjectID)
        {
            try
            {
                var sendmail = _entities.usp_SendProjectEstimatedEndDate(osProjectID);

                return 1;// _entities.ProjectsToCloses.Where(ptc => ptc.ID == osProjectID && ptc.EmailSent == true).Select(ptc => ptc.EmailID).FirstOrDefault() ?? -1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateOrUpdateProjectToClose(int projectId, string project_mgr_num, string project_number, byte? StatusID, DateTime? EstimateEndDate, DateTime? SubmittedDate, string submitBy, bool deferredSave = false)
        {
            ProjectsToClose projecttoclose = _entities.ProjectsToCloses.Where(pr => pr.ID == projectId).FirstOrDefault();

            if (StatusID == 8 && EstimateEndDate != null)
            {
                EstimateEndDate = null;
            }

            if (projecttoclose == null)
            {
                var newProjectToClose = new ProjectsToClose
                {
                    ID = projectId,
                    project_mgr_num = project_mgr_num,
                    project_number = project_number,
                    StatusID = StatusID,
                    EstimateEndDate = EstimateEndDate,
                    SubmittedDate = SubmittedDate,
                    SubmitBy = submitBy
                };
                _entities.ProjectsToCloses.Add(newProjectToClose);
            }
            else
            {
                projecttoclose.project_mgr_num = project_mgr_num;
                projecttoclose.StatusID = StatusID;
                projecttoclose.EstimateEndDate = EstimateEndDate;
                projecttoclose.SubmittedDate = SubmittedDate;
            }
            if (!deferredSave) _entities.SaveChanges();
        }

        public TypesEnum VerifyAccess(ProjectToCloseModel projecttoclose)
        {
            if (projecttoclose.OSProjectID == 0)
                return TypesEnum.NotFound;

            if (projecttoclose.OSProjectID == -1)
                return TypesEnum.NA;

            if (!projecttoclose.IsProject)
                return TypesEnum.Other;

            // if (projecttoclose.CurrentUserIsPM_AF)// ici il faut verifer si le user est keyMember
            if (isKeyMember(projecttoclose.CurrentUser, projecttoclose.TlinxProjectNumber))
            {
                if (projecttoclose.IsClosedOrPostponed &&
                    (projecttoclose.IsArchivedOrPostponed || projecttoclose.NoDocumentation))
                    return TypesEnum.ReadOnly;
                else
                    return TypesEnum.ProjectToClose;
            }
            else
            {//not PM_AF
                if (projecttoclose.IsActiveProject)
                    return TypesEnum.AccessDenied;
                else
                    return TypesEnum.ReadOnly;
            }

        }
        //fred 
        public bool isKeyMember(string currentUser, string projectIdOrNumber, string type = "")
        {
            var count = _entitiesSuiviMandat.XX_PROJECT_KEYMEMBERS
                .Where(p => p.project_number == projectIdOrNumber && p.employee_number == currentUser && p.end_date_active >= DateTime.Today)
                .Select(p => p.role).Count();
            if (count > 0) return true;
            else return false;


        }
        public List<SelectItem> GetProjectToCloseForUser(string employeeid, string active, string filter = "", string category = "", int topRecords = 0)
        {

            List<SelectItem> result = new List<SelectItem>();
            List<string> listProjectTocloseForEmployee = new List<string>();
            listProjectTocloseForEmployee = _entities.usp_PotentialProjectsToClose(employeeid, "TTPROJETPLUS").Select(p => p.TLinxProjectNumber).ToList();

            var includeClosed = active == "no" ? "1" : "0";

            IEnumerable<SelectItem> data = (from p in _entities.usp_GetProjectForKeyMembers(employeeid, includeClosed)
                                            select new SelectItem
                                            {
                                                Value = p.OS_Project_ID.ToString(),
                                                Text = p.project_number + " - " + p.Company + " - " + (p.Project_Description.Length >= 100 ? p.Project_Description.Substring(0, 100) : p.Project_Description),
                                                Status = p.Status_ID.ToString(),
                                                estimatedDate = p.EstimateEndDate,
                                                TLinxProjectNumber = p.TLinxProjectNumber
                                            }
             );
            if (topRecords > 0)
                result = data.OrderBy(p => p.Text).Take(topRecords).ToList();
            else
                result = data.OrderBy(p => p.Text).ToList();

            foreach (SelectItem item in result)
            {

                if (item.Status == "4")//listProjectTocloseForEmployee.Contains(item.TLinxProjectNumber
                {
                    if (listProjectTocloseForEmployee.Contains(item.TLinxProjectNumber))
                    {
                        if (item.estimatedDate != null)
                        {
                            item.Color = "#FFA500"; //orange en report
                            item.displayClass = "jaune";
                        }
                        else
                        {
                            item.Color = "#ff6666";
                            item.displayClass = "rouge";

                        }
                    }
                    else

                    {

                        if (item.estimatedDate != null)
                        {
                            item.Color = "#FFA500"; //orange en report
                            item.displayClass = "jaune";
                        }
                        else
                        {
                            item.Color = "#ffffff"; //changer pour test: remettre"none" blanc
                            item.displayClass = "blanc";

                        }
                    }
                }




                //else if (item.estimatedDate != null)
                //{
                //    if (item.Status == "4")
                //    {
                //        item.Color = "#FFA500"; //jaune
                //        item.displayClass = "jaune";
                //    }

                //}

                //else if (item.Status == "4")
                //{
                //    item.Color = "#ffffff"; //changer pour test: remettre"none" blanc
                //    item.displayClass = "blanc";
                //}

                else if (item.Status == "8")
                {
                    item.Color = "#32CD32";//vert
                    item.displayClass = "vert";
                }

                else if (item.Status == "6")
                {
                    item.Color = "#999999"; //gris
                    item.displayClass = "gris";
                }

            }

            //on veut afficher les projet par ordre de couleur suivant: rouge, vert, jaune, blanc, gris
            var list_rouge = result.Where(p => p.Color == "#ff6666").ToList();
            var list_vert = result.Where(p => p.Color == "#32CD32").ToList();
            var list_jaune = result.Where(p => p.Color == "#FFA500").ToList();
            var list_blanc = result.Where(p => p.Color == "#ffffff").ToList();
            var list_gris = result.Where(p => p.Color == "#999999").ToList();
            List<SelectItem> resultF = new List<SelectItem>();

            if (category == "")
            {
                resultF.AddRange(list_rouge);
                resultF.AddRange(list_vert);
                resultF.AddRange(list_jaune);
                resultF.AddRange(list_blanc);
                resultF.AddRange(list_gris);
            }

            else
            {
                switch (category)
                {
                    case "blanc":
                        resultF.AddRange(list_blanc);
                        break;
                    case "vert":
                        resultF.AddRange(list_vert);
                        break;
                    case "jaune":
                        resultF.AddRange(list_jaune);
                        break;
                    case "gris":
                        resultF.AddRange(list_gris);
                        break;
                    case "rouge":
                        resultF.AddRange(list_rouge);
                        break;
                }
            }
            return resultF;
        }

        public int returnIdForProjectFromTTLinx(string TlinxProjectNumber)
        {
            return _entities.OS_Project.Where(p => p.TLinxProjectNumber == TlinxProjectNumber).Select(p => p.OS_Project_ID).FirstOrDefault();
        }
    }
}
