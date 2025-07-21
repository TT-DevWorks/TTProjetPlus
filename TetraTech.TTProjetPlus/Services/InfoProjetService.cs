using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using System.Globalization;

namespace TetraTech.TTProjetPlus.Services
{
    public class InfoProjetService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public SearchCriteriasModel returnSearchCriterias()
        {
            var db = _entities;
            var con = new SqlConnection(db.Database.Connection.ConnectionString);
            var com = new SqlCommand();
            string anItem = "";

            try
            {

                DataSet ds = new DataSet();
                com.Connection = con;
                //com.CommandTimeout = 360;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "usp_RenderSearchCriterias";


                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                adapt.Fill(ds);

                SearchCriteriasModel searchModel = new SearchCriteriasModel();
                searchModel.listeProjectNumbers = new List<structureItem>();
                searchModel.listeProjectDescription = new List<structureItem>();
                searchModel.listeAcctCenterID = new List<structureItem>();
                searchModel.listeArchivedate = new List<structureItem>();
                searchModel.ListeBillSpecialistNumber = new List<structureItem>();
                searchModel.listeCustomersName = new List<structureItem>();
                searchModel.ListeCustomersNumbers = new List<structureItem>();
                searchModel.listeDocPostponedDate = new List<structureItem>();
                searchModel.listeInitiatedBy = new List<structureItem>();
                searchModel.listeProgramMgrNumber = new List<structureItem>();
                searchModel.listeStatusID = new List<structureItem>();
                searchModel.listeSubmittedDate = new List<structureItem>();
                searchModel.listeDocStatutID = new List<structureItem>();
                searchModel.listeFinalClient = new List<structureItem>();


                //foreach (DataRow row in ds.Tables[3].Rows) //a garder
                //{
                //    structureItem item = new structureItem();
                //    item.Name = row["Acct_Center"].ToString();
                //    item.Value = row["Acct_Center_ID"].ToString();
                //    searchModel.listeAcctCenterID.Add(item);
                //}

                // Format à envoyer: AcctGroupId1,AcctCenterId1

                foreach (DataRow row in ds.Tables[3].Rows) //a garder 
                {
                    structureItem item = new structureItem();                 
                    if (row["c_IsActive"].ToString() == "False")
                    {
                        anItem = row["Acct_Center"].ToString() + " *INACTIF (";
                    }
                    else
                    {
                        anItem = row["Acct_Center"].ToString() + " (";
                    }
                    if (row["g_IsActive"].ToString() == "False")
                    {
                        anItem = anItem + row["Acct_Group"].ToString() + ") *INACTIF";
                    }
                    else
                    {
                        anItem = anItem + row["Acct_Group"].ToString() + ")";
                    }

                    item.Name = anItem;
                    item.Value = row["Acct_Group_ID"].ToString() + "," + row["Acct_Center_ID"].ToString();
                    searchModel.listeAcctCenterID.Add(item);
                }

                foreach (DataRow row in ds.Tables[7].Rows) //a garder
                {
                    structureItem item = new structureItem();
                    item.Name = row["Description_FR"].ToString();
                    item.Value = row["Status_ID"].ToString();
                    searchModel.listeStatusID.Add(item);
                }

                foreach (DataRow row in ds.Tables[11].Rows) //a garder
                {
                    structureItem item = new structureItem();
                    item.Name = row["Description_FR"].ToString();
                    item.Value = row["Doc_Status_ID"].ToString();
                    searchModel.listeDocStatutID.Add(item);
                }

                con.Close();
                com.Dispose();

                return searchModel;
            }

            catch (Exception e)
            {
                addErrorLog(e, "InfoProjetService", "returnSearchCriterias");
                con.Close();
                com.Dispose();
                return null;
            }

        }

        public List<searchModel> returnListProjets(
                            string ProjectNumbers,
                            string ProjectDescription,
                            string CustomersNumber,
                            string CustomersName,
                            string FinalClient,
                            string AcctCenterId,
                            string InitiatedBy,
                            string ProgramMgrNumber,
                            string ProjectMgrNumber,
                            string BillSpecialistNumber,
                            string StatusID,
                            string SubmittedDate,
                            string Archivedate,
                            string DocPostponedDate,
                            string DocStatutID,
                            string archiveParam,
                            string comments)
        {

            try
            {
                //les dates
                var SubmittedDate_start = "";
                var SubmittedDate_end = "";
                var Archivedate_start = "";
                var Archivedate_end = "";
                var DocPostponedDate_start = "";
                var DocPostponedDate_end = "";


                if (SubmittedDate != "")
                {
                    string[] dates = SubmittedDate.TrimStart().TrimEnd().Split(' ');
                    SubmittedDate_start = dates[0];//.Remove(10);                  
                    SubmittedDate_end = dates[2];//.Remove(0, 1);                  
                }

                if (Archivedate != "")
                {
                    string[] dates2 = Archivedate.TrimStart().TrimStart().TrimEnd().Split(' ');
                    Archivedate_start = dates2[0];//.Remove(10);
                    Archivedate_end = dates2[2];//.Remove(0, 1);                                              
                }

                if (DocPostponedDate != "")
                {
                    string[] dates3 = DocPostponedDate.TrimStart().TrimStart().TrimEnd().Split(' ');
                    DocPostponedDate_start = dates3[0];//.Remove(10);
                    DocPostponedDate_end = dates3[2];//.Remove(0, 1);
                }
                if (ProjectNumbers.Trim() == "")
                {
                    ProjectNumbers = "";
                }
                if (ProjectDescription.Trim() == "")
                {
                    ProjectDescription = "";
                }
                if (CustomersNumber.Trim() == "")
                {
                    CustomersNumber = "";
                }
                if (CustomersName.Trim() == "")
                {
                    CustomersName = "";
                }
                if (FinalClient.Trim() == "")
                {
                    FinalClient = "";
                }
                if (InitiatedBy.Trim() == "")
                {
                    InitiatedBy = "";
                }
                if (ProgramMgrNumber.Trim() == "")
                {
                    ProgramMgrNumber = "";
                }
                if (comments.Trim() == "")
                {
                    comments = "";
                }
                if (ProjectMgrNumber.Trim() == "")
                {
                    ProjectMgrNumber = "";
                }
                if (BillSpecialistNumber.Trim() == "")
                {
                    BillSpecialistNumber = "";
                }

                var ObjectsList = _entities.usp_InfoRequest2(
                              ProjectNumbers.TrimStart().TrimEnd(),
                              ProjectDescription.TrimStart().TrimEnd(),
                              CustomersNumber.TrimStart().TrimEnd(),
                              CustomersName.TrimStart().TrimEnd(),
                              FinalClient.TrimStart().TrimEnd(),
                              AcctCenterId,
                              InitiatedBy.TrimStart().TrimEnd(),
                              ProgramMgrNumber.TrimStart().TrimEnd(),
                              ProjectMgrNumber.TrimStart().TrimEnd(),
                              BillSpecialistNumber.TrimStart().TrimEnd(),
                              SubmittedDate_start,
                              SubmittedDate_end,
                              Archivedate_start,
                              Archivedate_end,
                              StatusID,
                              DocStatutID,
                              DocPostponedDate_start,
                              DocPostponedDate_end,
                              archiveParam,
                              comments).ToList();

                List<searchModel> finalObjectsList = new List<searchModel>();

                foreach (usp_InfoRequest2_Result item in ObjectsList)
                {
                    searchModel search_item = new searchModel();
                    search_item.ProjectNumbers = item.Project;

                    if (item.Project_Description != null && item.Project_Description != "")
                    {
                        search_item.ProjectDescription = item.Project_Description;
                    }
                    else search_item.ProjectDescription = "";

                    search_item.CustomersNumber = item.Customer_Number;
                    search_item.CustomersName = item.Customer_Name;
                    search_item.FinalClient = item.End_Client_Name;
                    search_item.OldCustomersGPNumber = item.Customer_Number_GP;
                    search_item.OldCustomersGPName = item.Customer_Name_GP;
                    search_item.AcctCenterName = item.Organisation;
                    search_item.InitiatedBy = item.Initiated_Par;
                    search_item.ProgramMgrNumber = item.Program_Manager;
                    search_item.ProjectMgrNumber = item.Project_Manager;
                    search_item.BillSpecialistNumber = item.Billing_specialist;
                    search_item.StatusID = item.statut;
                    search_item.SubmittedDateq = item.SubmittedDate == null ? "" : item.SubmittedDate.Value.Year.ToString("0000") + "-" + item.SubmittedDate.Value.Month.ToString("00") + "-" + item.SubmittedDate.Value.Day.ToString("00");
                    search_item.Archivedateq = item.Archive_date == null ? "" : item.Archive_date.Value.Year.ToString("0000") + "-" + item.Archive_date.Value.Month.ToString("00") + "-" + item.Archive_date.Value.Day.ToString("00");
                    search_item.DocPostponedDateq = item.Doc_PostponedDate == null ? "" : item.Doc_PostponedDate.Value.Year.ToString("0000") + "-" + item.Doc_PostponedDate.Value.Month.ToString("00") + "-" + item.Doc_PostponedDate.Value.Day.ToString("00");
                    search_item.DocStatutID = item.Doc_status;
                    search_item.project_start_date = item.project_start_date == null ? "" : item.project_start_date.Value.Year.ToString("0000") + "-" + item.project_start_date.Value.Month.ToString("00") + "-" + item.project_start_date.Value.Day.ToString("00");
                    search_item.project_end_date = item.project_end_date == null ? "" : item.project_end_date.Value.Year.ToString("0000") + "-" + item.project_end_date.Value.Month.ToString("00") + "-" + item.project_end_date.Value.Day.ToString("00");

                    search_item.sharePath = "";
                    if (item.cheminPartage != null && item.cheminPartage != "")
                    {
                        if (Directory.Exists(item.cheminPartage + "\\" + item.Project))
                        {
                            search_item.sharePath = (item.ETA_PRO_ID == 2) ? "" : item.cheminPartage + "\\" + item.Project;
                        }
                    }

                    search_item.Model = item.MOD_Nom;

                    // search_item.comments = item.Comments;

                  //  search_item.endDate = _entitiesSuiviMandat.XX_PROJECT_DATA.Where(p => p.project_number.Split('-')[1] == item.Project).Select(p => p.project_end_date).ToString();

                    finalObjectsList.Add(search_item);
                }

                return finalObjectsList;
            }
            catch (Exception e)
            {
                addErrorLog(e, "InfoProjetService", "returnListProjets");
                return null;

            }
        }
        public string returnEmployee(string number)
        {           
            var now = DateTime.Now;
            return _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now && p.EMPLOYEE_NUMBER == number).Select(p => p.FULL_NAME).FirstOrDefault();
        }

        public string returnAccCenterName(int? Acct_Center_ID)
        {
            return _entities.Acct_Center.Where(p => p.Acct_Center_ID == Acct_Center_ID).Select(p => p.Acct_Center1).FirstOrDefault();
        }

        public string returnStatusName(byte? Status_ID)
        {
            return _entities.Status.Where(p => p.Status_ID == Status_ID).Select(p => p.Description_FR).FirstOrDefault();
        }

        public string returnDocStatusName(byte? Doc_Status_ID)
        {
            return _entities.Doc_Status.Where(p => p.Doc_Status_ID == Doc_Status_ID).Select(p => p.Description_FR).FirstOrDefault();
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

        public fiche returnFiche(int ficheId)
        {
            var fiche = _entities.fiches.Where(x => x.id_fiche == ficheId).FirstOrDefault();
            return fiche;
        }
    }


}
