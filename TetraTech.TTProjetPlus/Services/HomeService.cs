using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;
using SpreadsheetLight;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using System.Net;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;
using ExcelDataReader;
using System.Security.AccessControl;
using System.Data.SqlClient;

using System.Runtime.InteropServices; // DllImport
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions; // PermissionSetAttribute


namespace TetraTech.TTProjetPlus.Services
{
    public class HomeService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();        
        public List<string> listeOfActiveModelsIds = new List<string>();
        private readonly AdministrationEntities _entitiesAdministration = new AdministrationEntities();

        // obtains user token
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public List<OSProjectModel> getOffersList(bool notIncludeInactiveOffers)
        {

            if (notIncludeInactiveOffers == true)//on masque les offres inactives 2, 3
            {
                var OffersList = from p in _entities.OS_Project.Where(x => x.Status_ID == 1 || x.Status_ID == 5)
                                 select new OSProjectModel
                                 {
                                     Acct_Center_ID = p.Acct_Center_ID.ToString(),
                                     Acct_Group_ID = p.Acct_Group_ID.ToString(),
                                     Approx_Amount = p.Approx_Amount,
                                     Billing_Address = p.Billing_Address,
                                     Comments = p.Comments,
                                     Company = p.Company,
                                     Contact_Client = p.Contact_Client,
                                     Contract_Number = p.Contract_Number,
                                     Contract_Type_ID = p.Contract_Type_ID,
                                     Currency_ID = p.Currency_ID,
                                     Customer_Name = p.Customer_Name,
                                     Customer_Number = p.Customer_Number,
                                     End_Client_Name = p.End_Client_Name,
                                     Initiated_By = p.Initiated_By,
                                     Invoice_Email_Address = p.Invoice_Email_Address,
                                     Is_Master = p.Is_Master,
                                     Last_Modification_Date = (p.Last_Modification_Date.ToString() == "" || p.Last_Modification_Date == null) ? "" : DateTime.ParseExact(p.Last_Modification_Date.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString(),
                                     Last_Modification_User = p.Last_Modification_User,
                                     OS_Project_ID = p.OS_Project_ID,
                                     PhaseCode = p.PhaseCode,
                                     PhaseCodeType_ID = p.PhaseCodeType_ID,
                                     Program_ID = p.Program_ID,
                                     Program_Mgr_Number = p.Program_Mgr_Number,
                                     Project_Description = p.Project_Description,
                                     Project_Master_ID = p.Project_Master,
                                     // Project_Master_Origine_ID = p.Project_Master_Origine_ID,
                                     Project_Mgr_Number = p.Project_Mgr_Number,
                                     Project_Number = p.Project_Number,
                                     Required_Opening_Date = p.Required_Opening_Date,
                                     Shipping_Address = p.Shipping_Address,
                                     Status_ID = p.Status_ID,
                                     Work_Type_ID = p.Work_Type_ID,
                                     Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                                     Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager                                     
                                 };
                return OffersList.ToList();
            }
            else //on voit toutes les offres, actives et incatives
            {
                var OffersList = from p in _entities.OS_Project.Where(x => x.Status_ID == 1 || x.Status_ID == 2 || x.Status_ID == 3 || x.Status_ID == 5)
                                 select new OSProjectModel
                                 {
                                     Acct_Center_ID = p.Acct_Center_ID.ToString(),
                                     Acct_Group_ID = p.Acct_Group_ID.ToString(),
                                     Approx_Amount = p.Approx_Amount,
                                     Billing_Address = p.Billing_Address,
                                     Comments = p.Comments,
                                     Company = p.Company,
                                     Contact_Client = p.Contact_Client,
                                     Contract_Number = p.Contract_Number,
                                     Contract_Type_ID = p.Contract_Type_ID,
                                     Currency_ID = p.Currency_ID,
                                     Customer_Name = p.Customer_Name,
                                     Customer_Number = p.Customer_Number,
                                     End_Client_Name = p.End_Client_Name,
                                     Initiated_By = p.Initiated_By,
                                     Invoice_Email_Address = p.Invoice_Email_Address,
                                     Is_Master = p.Is_Master,
                                     Last_Modification_Date = (p.Last_Modification_Date.ToString() == "" || p.Last_Modification_Date == null) ? "" : DateTime.ParseExact(p.Last_Modification_Date.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString(),
                                     Last_Modification_User = p.Last_Modification_User,
                                     OS_Project_ID = p.OS_Project_ID,
                                     PhaseCode = p.PhaseCode,
                                     PhaseCodeType_ID = p.PhaseCodeType_ID,
                                     Program_ID = p.Program_ID,
                                     Program_Mgr_Number = p.Program_Mgr_Number,
                                     Project_Description = p.Project_Description,
                                     Project_Master_ID = p.Project_Master,
                                     //  Project_Master_Origine_ID = p.Project_Master_Origine_ID,
                                     Project_Mgr_Number = p.Project_Mgr_Number,
                                     Project_Number = p.Project_Number,
                                     Required_Opening_Date = p.Required_Opening_Date,
                                     Shipping_Address = p.Shipping_Address,
                                     Status_ID = p.Status_ID,
                                     Work_Type_ID = p.Work_Type_ID,
                                     Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                                     Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager

                                 };
                return OffersList.ToList();
            }

        }

        public List<OSProjectModel> getProjectList(bool notIncludeInactiveProjects)
        {
            if (notIncludeInactiveProjects == true)//on masque lesprojets inactifs 6
            {
                var ProjectsList = from p in _entities.OS_Project.Where(x => x.Status_ID == 4)
                                   select new OSProjectModel
                                   {
                                       Acct_Center_ID = p.Acct_Center_ID.ToString(),
                                       Acct_Group_ID = p.Acct_Group_ID.ToString(),
                                       Approx_Amount = p.Approx_Amount,
                                       Billing_Address = p.Billing_Address,
                                       Comments = p.Comments,
                                       Company = p.Company,
                                       Contact_Client = p.Contact_Client,
                                       Contract_Number = p.Contract_Number,
                                       Contract_Type_ID = p.Contract_Type_ID,
                                       Currency_ID = p.Currency_ID,
                                       Customer_Name = p.Customer_Name,
                                       Customer_Number = p.Customer_Number,
                                       End_Client_Name = p.End_Client_Name,
                                       Initiated_By = p.Initiated_By,
                                       Invoice_Email_Address = p.Invoice_Email_Address,
                                       Is_Master = p.Is_Master,
                                       Last_Modification_Date = (p.Last_Modification_Date.ToString() == "" || p.Last_Modification_Date == null) ? "" : DateTime.ParseExact(p.Last_Modification_Date.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString(),
                                       Last_Modification_User = p.Last_Modification_User,
                                       OS_Project_ID = p.OS_Project_ID,
                                       PhaseCode = p.PhaseCode,
                                       PhaseCodeType_ID = p.PhaseCodeType_ID,
                                       Program_ID = p.Program_ID,
                                       Program_Mgr_Number = p.Program_Mgr_Number,
                                       Project_Description = p.Project_Description,
                                       Project_Master_ID = p.Project_Master,
                                       //  Project_Master_Origine_ID = p.Project_Master_Origine_ID,
                                       Project_Mgr_Number = p.Project_Mgr_Number,
                                       Project_Number = p.Project_Number,
                                       Required_Opening_Date = p.Required_Opening_Date,
                                       Shipping_Address = p.Shipping_Address,
                                       Status_ID = p.Status_ID,
                                       Work_Type_ID = p.Work_Type_ID,
                                       Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                                       Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager
                                   };
                return ProjectsList.ToList();
            }
            else //on voit toutes les projets, actifs et inactifs
            {
                var ProjectsList = from p in _entities.OS_Project.Where(x => x.Status_ID == 4 || x.Status_ID == 6)
                                   select new OSProjectModel
                                   {
                                       Acct_Center_ID = p.Acct_Center_ID.ToString(),
                                       Acct_Group_ID = p.Acct_Group_ID.ToString(),
                                       Approx_Amount = p.Approx_Amount,
                                       Billing_Address = p.Billing_Address,
                                       Comments = p.Comments,
                                       Company = p.Company,
                                       Contact_Client = p.Contact_Client,
                                       Contract_Number = p.Contract_Number,
                                       Contract_Type_ID = p.Contract_Type_ID,
                                       Currency_ID = p.Currency_ID,
                                       Customer_Name = p.Customer_Name,
                                       Customer_Number = p.Customer_Number,
                                       End_Client_Name = p.End_Client_Name,
                                       Initiated_By = p.Initiated_By,
                                       Invoice_Email_Address = p.Invoice_Email_Address,
                                       Is_Master = p.Is_Master,
                                       Last_Modification_Date = (p.Last_Modification_Date.ToString() == "" || p.Last_Modification_Date == null) ? "" : DateTime.ParseExact(p.Last_Modification_Date.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString(),
                                       Last_Modification_User = p.Last_Modification_User,
                                       OS_Project_ID = p.OS_Project_ID,
                                       PhaseCode = p.PhaseCode,
                                       PhaseCodeType_ID = p.PhaseCodeType_ID,
                                       Program_ID = p.Program_ID,
                                       Program_Mgr_Number = p.Program_Mgr_Number,
                                       Project_Description = p.Project_Description,
                                       Project_Master_ID = p.Project_Master,
                                       //   Project_Master_Origine_ID = p.Project_Master_Origine_ID,
                                       Project_Mgr_Number = p.Project_Mgr_Number,
                                       Project_Number = p.Project_Number,
                                       Required_Opening_Date = p.Required_Opening_Date,
                                       Shipping_Address = p.Shipping_Address,
                                       Status_ID = p.Status_ID,
                                       Work_Type_ID = p.Work_Type_ID,
                                       Client_ProjectManager = (p.Client_ProjectManager == null || p.Client_ProjectManager == "" || p.Client_ProjectManager == "NULL") ? "" : p.Client_ProjectManager,
                                       Client_Email_ProjectManager = (p.Client_Email_ProjectManager == null || p.Client_Email_ProjectManager == "" || p.Client_Email_ProjectManager == "NULL") ? "" : p.Client_Email_ProjectManager
                                   };
                return ProjectsList.ToList();
            }

        }



        public List<string> GetCustomersNumbers()
        {
            var numberList = _entities.Customers.OrderBy(x => x.Customer_Number).Select(p => p.Customer_Number).ToList();
            return numberList;
        }

        public List<string> GetGestionnairesNames()
        {            
            DateTime dt = DateTime.Now.Date;
            var listGest = _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= dt && p.EFFECTIVE_END_DATE >= dt).Select(p => p.FULL_NAME);// + " / " + p.EMPLOYEE_NUMBER);
            return listGest.Distinct().ToList();

        }

        public List<string> ProjectNumberListArrayFromEntity()
        {
            var ProjectNumberList = _entities.OS_Project.OrderBy(x => x.Project_Number).Select(p => p.Project_Number).Distinct().ToList();
            return ProjectNumberList;

        }


        public List<string> GetCustomersNames()
        {
            var nameList = _entities.Customers.OrderBy(x => x.Customer_Name).Select(p => p.Customer_Name).ToList();

            return nameList;
        }


        public List<Customer> GetCustomersNumberName()
        {
            return _entities.Customers.OrderBy(p => p.Customer_Name).ToList();
        }


        //gestionnaire de projets decommenter lorsque le serveur test est a nouveau operationnel 
        //gestionnaire de projets decommenter lorsque le serveur test est a nouveau operationnel
        public List<ProjectManagerItem> GetProjectManagers()
        {
            try
            {                
                var now = DateTime.Now;
                var query = from p in _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now)
                            select new ProjectManagerItem { Full_Name = p.FULL_NAME, Employee_Number = p.EMPLOYEE_NUMBER.ToString(), Org = p.ORG, Office = p.LOCATION_CODE, email = p.EMAIL_ADDRESS };
                return query.Distinct().ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<structureItem> GetProjectManagers2()
        {
            var query = from p in _entities.OS_Project
                        select new structureItem { Name = p.Project_Mgr_Number, Value = p.Project_Mgr_Number.ToString() };
            return query.Distinct().ToList();

        }

        public string Get_CP_mail(string full_name)
        {
            var name = full_name.Split('(')[0];
            var employeMail = _entities.EmployeeActif_Inactive.Where(p => p.FULL_NAME.Contains(name)).
                        Select(p => p.EMAIL_ADDRESS).FirstOrDefault();
            return employeMail;

        }



        public string GetProjectManagersNumber(string ProjectMgr)
        {
            var now = DateTime.Now;
            var employeNumber = _entities.EmployeeActif_Inactive.Where(p => p.FULL_NAME == ProjectMgr && p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).
                        Select(p => p.EMPLOYEE_NUMBER).FirstOrDefault();
            return employeNumber;

        }
        public List<structureItem> GetProjectFiltered(string numeroProjet, string titreProjet, string gestionProjet,
             string compagnie, string uniteAffaire, string organisation)
        {
            var query = from p in _entities.OS_Project select p;
            if (!String.IsNullOrEmpty(numeroProjet))
            {
                query = query.Where(p => p.Project_Number == numeroProjet);
            }
            if (!String.IsNullOrEmpty(titreProjet))
            {
                query = query.Where(p => p.Project_Description == titreProjet);
            }

            if (!String.IsNullOrEmpty(gestionProjet))
            {
                query = query.Where(p => p.Project_Mgr_Number == gestionProjet);
            }

            if (!String.IsNullOrEmpty(compagnie))
            {
                query = query.Where(p => p.Company == compagnie);
            }
            if (!String.IsNullOrEmpty(uniteAffaire))
            {
                query = query.Where(p => p.Acct_Group_ID.ToString() == uniteAffaire);
            }

            if (!String.IsNullOrEmpty(organisation))
            {
                query = query.Where(p => p.Acct_Center_ID.ToString() == organisation);
            }


            var listProjects = from p in query select new structureItem { Name = p.Project_Description, Value = p.Project_Number };


            return listProjects.Distinct().ToList();

        }

        public List<structureItem> GetProjectFiltered2()
        {
            _entities.Database.CommandTimeout = 480; //8 min
            var query = _entities.usp_Search_MasterProjectNumbers("", null, null);
            var list = from p in query
                       where (p.Project_Description != "" && p.Project_Description != null)
                       select new structureItem
                       {
                           Name = p.Project_Number + " / " +
                       (p.Project_Description.Length > 50 ? p.Project_Description.Substring(0, 50) : p.Project_Description),
                           Value = p.Project_Number
                       };


            return list.Distinct().ToList();

        }

        //liste des projets: doit etre validée:
        public List<structureItem> GetProjects()
        {

            var listProjects = _entities.usp_getProjectNoStruct().ToList();
            var listProjectsNotNull = listProjects.Where(x => x != null).ToList();
            var query = from p in _entities.OS_Project
                        where listProjectsNotNull.Contains(p.Project_Number)
                        select new structureItem { Name = p.Project_Number, Description = p.Project_Description == null ? "" : p.Project_Description, Value = p.Project_Number.ToString(), email = "" };
            return query.Distinct().ToList();

            //
        }

        public List<structureItem> GetProjectsForArb()
        {

            var query = from p in _entitiesBPR.Projets
                        select new structureItem { Name = p.PRO_Id, Value = p.PRO_Id.ToString() }
                        ;
            return query.Distinct().ToList();

        }

        public List<structureItem> getListWSDR()
        {
            //liste de projet problematique sans structure de repertoire
            var query = from p in _entitiesBPR.Projets
                        where p.ETA_PRO_ID == 1 
                        select new structureItem { Name = p.PRO_Id, Value = p.PRO_Id.ToString() , PRO_Date_Creation = p.PRO_Date_Creation};
            var list = query.Distinct().OrderByDescending(x => x.PRO_Date_Creation).ToList();
            return list;

        }

        public List<structureItem> getListWSDR2()
        {

            var query = from p in _entities.usp_ssrsGetProjetAFermer()
                        where p.ETA_PRO_ID == 1
                        select new structureItem { Name = p.Serveur + " / " + p.Project_Number   , Value = p.Project_Number };
            var list = query.Distinct().ToList();
            return list;

        }



        public string GetCustomerNameByNumber(string customerNumber)
        {
            var name = _entities.Customers.Where(p => p.Customer_Number == customerNumber).Select(p => p.Customer_Name).FirstOrDefault();
            return name;
        }

        public string GetProjectTitleByNumber(string projectNumber)
        {
            var name = _entitiesSuiviMandat.XX_PROJECT_DATA.Where(p => p.project_number == projectNumber).Select(p => p.project_desc).FirstOrDefault();
            return name;
        }

        public string GetCustomerNumberByName(string customerName)
        {
            var name = _entities.Customers.Where(p => p.Customer_Name == customerName).Select(p => p.Customer_Number).FirstOrDefault();
            return name;
        }


        //unites d'affaire: table Acc_group
        public List<structureItem> GetUAs()
        {
            var query = from p in _entities.Acct_Group.Where(p => p.isActive).OrderBy(p => p.Acct_Group1)
                        select new structureItem { Name = p.Acct_Group1, Value = p.Acct_Group_ID.ToString() };

            return query.ToList();
        }

        public List<structureItem> GetUAsForSelectedCompanies(string company)
        {
            var id = _entities.Companies.Where(p => p.company1 == company).Select(p => p.Company_id).FirstOrDefault();
            var query = from p in _entities.Acct_Group.Where(p => p.Company_id == id && p.isActive).OrderBy(p => p.Acct_Group_ID)
                        select new structureItem { Name = p.Acct_Group1, Value = p.Acct_Group_ID.ToString() };

            return query.OrderBy(x => x.Name).ToList();
        }

        public List<structureItem> GetModeleSelectionForModelId(string user, int modelId)
        {
            //var projectsList = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user || x.MOD_SEL_Utilisateur == "COMMUN") && x.MOD_Id==modelId)
            //                   select new structureItem { Name = p.MOD_SEL_Description , Value = p.MOD_SEL_Id.ToString() };
            //return projectsList.ToList();
            var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN") && x.MOD_Id == modelId).OrderBy(p => p.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };
            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user) && x.MOD_Id == modelId).OrderBy(p => p.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var l1 = projectsList1.ToList();
            var l2 = projectsList2.ToList();
            l1.AddRange(l2);
            return l1;

        }

        public List<structureItem> GetModeleSelectionAll(string user)
        {
            var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN")).OrderBy(x => x.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user)).OrderBy(x => x.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var l1 = projectsList1.ToList();
            var l2 = projectsList2.ToList();
            l1.AddRange(l2);
            return l1;
        }

        public List<structureItem> GetModeleSelectionAll2(string user, string projectNumber)
        {
            var projectNumberModId = _entitiesBPR.Projets.Where(x => x.PRO_Id == projectNumber).Select(x => x.MOD_SEC_Id).FirstOrDefault();
            var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN" && x.MOD_Id == projectNumberModId)).OrderBy(x => x.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user && x.MOD_Id == projectNumberModId)).OrderBy(x => x.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var l1 = projectsList1.ToList();
            var l2 = projectsList2.ToList();
            l1.AddRange(l2);
            return l1;
        }





        /*
        select* from Modele_Selection where mod_id in
        (
        select mod_id from Modele where mod_id = 10
        )
        and MOD_SEL_Utilisateur = 'COMMUN'--Pour obtenir les modèles communs à tous

        */
        public string GetCompanyName(string companyId)
        {
            var Id = Int32.Parse(companyId);
            var query = _entities.Companies.Where(p => p.Company_id == Id).Select(p => p.company1).FirstOrDefault();

            return query;
        }

        public string GetUAName(string BusinessUnitId)
        {
            var Id = Int32.Parse(BusinessUnitId);
            var query = _entities.Acct_Group.Where(p => p.Acct_Group_ID == Id).Select(p => p.Acct_Group1).FirstOrDefault();

            return query;
        }

        public string GetORGName(string ORGId)
        {
            var Id = Int32.Parse(ORGId);
            var query = _entities.Acct_Center.Where(p => p.Acct_Center_ID == Id).Select(p => p.Acct_Center1).FirstOrDefault();

            return query;
        }


        //organisation: table Acc_center
        public List<structureItem> GetORGs()
        {
            var query = from p in _entities.Acct_Center.Where(p => p.isActive).OrderBy(p => p.Acct_Center1)
                        select new structureItem { Name = p.Acct_Center1, Value = p.Acct_Center_ID.ToString() };

            return query.ToList();
        }


        public List<structureItem> GetUAsForSelectedORG(int UAId)
        {
            var query = from p in _entities.Acct_Center.Where(p => p.Acct_Group_ID == UAId && p.isActive).OrderBy(p => p.Acct_Center1)
                        select new structureItem { Name = p.Acct_Center1, Value = p.Acct_Center_ID.ToString() };

            return query.ToList();
        }

        //compagnies: table Company
        public List<structureItem> GetCompanies()
        {
            var query = from p in _entities.Companies.Where(p => p.isActive).OrderBy(p => p.company1)
                        select new structureItem { Name = p.company1, Value = p.Company_id.ToString() };

            return query.ToList();
        }


        public List<structureItem> GetServerName()
        {
            var query = from p in _entitiesBPR.usp_ObtenirServeur(null).GroupBy(x => x.SER_Nom).Select(x => x.FirstOrDefault()).OrderBy(p => p.SER_Description)
                        select new structureItem { Name = p.SER_Description, Value = p.SER_Id.ToString() };
            return query.ToList();
        }

        public List<structureItem> GetTestServerName()
        {
            var query = from p in _entitiesBPR.Serveurs.Where(p => p.SER_Id == 68 || p.SER_Id == 94 || p.SER_Id == 95).OrderBy(p => p.SER_Description)
                        select new structureItem { Name = p.SER_Description, Value = p.SER_Id.ToString() };
            return query.ToList();
        }

        public List<structureItem> GetTQEExceptionServerName()
        {
            var query = from p in _entitiesBPR.Serveurs.Where(p => p.SER_Id == 59).OrderBy(p => p.SER_Description)
                        select new structureItem { Name = p.SER_Description, Value = p.SER_Id.ToString() };
            return query.ToList();
        }

        public string GetNameForServer(string server)
        {
            var query = _entitiesBPR.Serveurs.Where(p => p.SER_Id.ToString() == server).Select(p => p.SER_Description).FirstOrDefault();

            return query;


        }

        public string returnCheminPartage(string server)
        {
            var query = _entitiesBPR.Serveurs.Where(p => p.SER_Id.ToString() == server).Select(p => p.SER_Chemin_Partage).FirstOrDefault();

            return query;


        }


        public List<structureItem> GetModeleName()
        {
            var query = from p in _entitiesBPR.Modeles.Where(x => x.MOD_Actif == true).GroupBy(x => x.MOD_Id).Select(x => x.FirstOrDefault()).OrderBy(p => p.MOD_Nom)
                        select new structureItem { Name = p.MOD_Nom, Value = p.MOD_Id.ToString() };
            listeOfActiveModelsIds = query.Select(x => x.Value).ToList();
            return query.ToList();

        }

        public List<structureItem> GetTQEExceptionModeleName()
        {
            var query = from p in _entitiesBPR.Modeles.Where(x => (x.MOD_Id == 30 || x.MOD_Id == 12)).GroupBy(x => x.MOD_Id).Select(x => x.FirstOrDefault()).OrderBy(p => p.MOD_Nom)
                        select new structureItem { Name = p.MOD_Nom, Value = p.MOD_Id.ToString() };
            listeOfActiveModelsIds = query.Select(x => x.Value).ToList();
            return query.ToList();

        }

        public string GetServerModelName(string serverModel)
        {
            var query = _entitiesBPR.Modeles.Where(x => x.MOD_Id.ToString() == serverModel).Select(x => x.MOD_Description).FirstOrDefault();
            return query;

        }


        public List<structureItem> GetModeleSecurite()
        {
            var query = from p in _entitiesBPR.Modele_Securite
                        where (listeOfActiveModelsIds.Contains(p.MOD_Id.ToString()))
                        select new structureItem { Name = p.MOD_SEC_Description, Value = p.MOD_SEC_Id.ToString() };

            return query.ToList();
        }


        public string GetSecurityModelName(string SecurityModel)
        {
            var query = _entitiesBPR.Modele_Securite.Where(p => p.MOD_SEC_Id.ToString() == SecurityModel).Select(p => p.MOD_SEC_Description).FirstOrDefault();
            return query;
        }



        public List<structureItem> GetModeleSelection()
        {
            //var query = from p in _entitiesBPR.Modele_Selection.GroupBy(x=>x.MOD_SEL_Id).Select(x => x.FirstOrDefault()).OrderBy(p => p.MOD_SEL_Description)
            //            select new structureItem { Name = p.MOD_SEL_Description +" * "+p.MOD_SEL_Utilisateur, Value = p.MOD_SEL_Id.ToString() };

            //return query.ToList();

            var user = UserService.CurrentUser.TTAccount;
            user = Regex.Replace(user.Normalize(NormalizationForm.FormD), @"[^A-Za-z 0-9 \.,\?'""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*", string.Empty).Trim();
            var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN")).OrderByDescending(p => p.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Description + " * " + p.MOD_SEL_Utilisateur, Value = p.MOD_SEL_Id.ToString() };

            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user)).OrderByDescending(p => p.MOD_SEL_Nom).ToList()
                                select new structureItem { Name = p.MOD_SEL_Description + " * " + p.MOD_SEL_Utilisateur, Value = p.MOD_SEL_Id.ToString() };
            var l1 = projectsList1.ToList();
            var l2 = projectsList2.ToList();
            l1.AddRange(l2);
            return l1;

        }

        public string SelectionModelName(string SelectionModel)
        {
            var query = _entitiesBPR.Modele_Selection.Where(p => p.MOD_SEL_Id.ToString() == SelectionModel).Select(p => p.MOD_SEL_Description + " * " + p.MOD_SEL_Utilisateur).FirstOrDefault();
            return query;
        }


        // strucuture de repertoire pour la page de modification
        public List<node> returnListOfNodes(string striprojectNumber, bool disableAll, bool folderCheckDisabled)
        {

            List<node> nodeList = new List<node>();
            var modID2 = _entitiesBPR.usp_ObtenirModeleParProjet(striprojectNumber).Select(x => x.MOD_Id).FirstOrDefault();
            var nodeList2 = _entitiesBPR.usp_ObtenirArborescenceRepertoireModeleTTProjetPlus(modID2.ToString()).ToList();

            foreach (usp_ObtenirArborescenceRepertoireModeleTTProjetPlus_Result item in nodeList2)
            {
                var disabled = (item.REP_Id == 1 || item.REP_Id == 2 || item.REP_Id == 3) ? true : false;
                node model = new node("<span style='color: #000080'>" + item.REP_Description.Split('*')[0] + "</span>" + " - " + item.REP_Description.Split('*')[1],
                                     item.REP_Id.ToString(),
                                     item.REP_Parent.ToString(),
                                     false,
                                     item.REP_Id.ToString(),
                                     disabled,
                                     disableAll,
                                     item.REP_Description.Split('*')[0].TrimEnd().TrimStart(),
                                     folderCheckDisabled,
                                     ""
                               );
                nodeList.Add(model);
            }

            return nodeList;

        }
        public List<node> FolderUsedInModeleByModId(string modeleSelection, bool disableAll, bool folderCheckDisabled, string color = "")
        {
            List<node> nodeList = new List<node>();
            var modId = _entitiesBPR.Modele_Selection.Where(p => p.MOD_SEL_Id.ToString() == modeleSelection).Select(p => p.MOD_Id).FirstOrDefault();
            var nodeList2 = _entitiesBPR.usp_ObtenirRepertoireParModele(modId).ToList();//.OrderBy(pr => pr.REP_Description).ToList();

            foreach (usp_ObtenirRepertoireParModele_Result item in nodeList2)
            {
                var disabled = (item.REP_Id == 1 || item.REP_Id == 2 || item.REP_Id == 3 || item.REP_Id == 559) ? true : false;
                node model = new node("<span style='color: #000080'>" + item.REP_Code + "</span>" + " - " + item.REP_Description,
                                     item.REP_Id.ToString(),
                                     item.REP_Parent.ToString(),
                                     false,
                                     item.REP_Code,
                                     disabled,
                                     disableAll,
                                     item.REP_Code,
                                     folderCheckDisabled,
                                     ""
                               );
                nodeList.Add(model);
            }
            //nodeList.OrderBy(i => i.Code);
            return nodeList;

        }

        public int CompareThings(node x, node y)
        {
            int intX, intY;
            if (int.TryParse(x.Code, out intX) && int.TryParse(y.Code, out intY))
                return intX.CompareTo(intY);

            return x.Code.CompareTo(y.Code);
        }
        // strucuture de repertoire pour la page de creation
        public List<node> returnListOfNodesForCreation(int modId)
        {
            List<node> nodeList = new List<node>();
            nodeList = _entitiesBPR.usp_ObtenirArborescenceRepertoireModeleTTProjetPlus(modId.ToString()).OrderBy(pr => pr.REP_Description)
                                    .Select(pr => new node(pr.REP_Description, pr.REP_Id.ToString(), pr.REP_Parent.ToString(), false, pr.REP_Id.ToString(), false, false, pr.REP_Description.Split('*')[0], false, "")).ToList();
            nodeList.OrderBy(i => i.name);
            return nodeList;
        }

        public List<string> FolderUsedInProjectBySelection_Node(string modeleSelection)
        {
            List<string> nodeList = new List<string>();
            nodeList = _entitiesBPR.usp_ObtenirRepertoireParModeleSelection(Int32.Parse(modeleSelection)).OrderBy(pr => pr.REP_Description)
                                    .Select(pr => pr.REP_Id.ToString()).ToList();

            return nodeList;

        }


        public List<string> ReturnUsedFolder(string selectionModel)
        {
            var listOfUsedFolders = FolderUsedInProjectBySelection_Node(selectionModel);
            return listOfUsedFolders;
        }
        public List<node> returnListOfNodesTotal(string striprojectNumber, string modeleSelection, bool disableAll, bool folderCheckDisabled)
        {

            List<node> nodeList = new List<node>();
            var modID2 = _entitiesBPR.usp_ObtenirModeleParProjet(striprojectNumber).Select(x => x.MOD_Id).FirstOrDefault();
            var nodeList2 = _entitiesBPR.usp_ObtenirArborescenceRepertoireModeleTTProjetPlus(modID2.ToString()).ToList();
            var nodeList3 = _entitiesBPR.usp_ObtenirRepertoireParModele(Int32.Parse(modeleSelection)).ToList();//.OrderBy(pr => pr.REP_Description).ToList();

            foreach (usp_ObtenirArborescenceRepertoireModeleTTProjetPlus_Result item in nodeList2)
            {
                var disabled = (item.REP_Id == 1 || item.REP_Id == 2 || item.REP_Id == 3) ? true : false;
                node model = new node("<span style='color: #000080'>" + item.REP_Description.Split('*')[0] + "</span>" + " - " + item.REP_Description.Split('*')[1],
                                     item.REP_Id.ToString(),
                                     item.REP_Parent.ToString(),
                                     false,
                                     item.REP_Id.ToString(),
                                     disabled,
                                     disableAll,
                                     item.REP_Description.Split('*')[0].TrimEnd().TrimStart(),
                                     folderCheckDisabled,
                                     ""
                               );
                nodeList.Add(model);
            }

            return nodeList;

        }


        public IList<node> BuildTreeForProjectAndMSLG(string controller, string striprojectNumber, string modeleSelection = "", bool disableAll = false, bool folderCheckDisabled = false)

        {
            var source = _entitiesBPR.usp_ObtenirRepertoireParProjetEtModSelectionTTProjetPlus(striprojectNumber, Int32.Parse(modeleSelection)).ToList();
            var nodeList = new List<node>();
            foreach (usp_ObtenirRepertoireParProjetEtModSelectionTTProjetPlus_Result item in source)
            {
                var disabled = (item.REP_Id == 1 || item.REP_Id == 2 || item.REP_Id == 3 || item.REP_Id == 559) ? true : false;
                node model = new node("<span style='color: #000080'>" + item.REP_Code + "</span>" + " - " + item.REP_Description,
                                     item.REP_Id.ToString(),
                                     item.REP_Parent.ToString(),
                                     false,
                                     item.REP_Code,
                                     disabled,
                                     disableAll,
                                     item.REP_Code,
                                     folderCheckDisabled,
                                     item.ProjetALeRept.ToString()
                               );
                nodeList.Add(model);
            }

            foreach (node item in nodeList)
            {
                if (item.HasThisRep == "1")
                {
                    item.Checked = true;
                    item.FolderCheckDisabled = true;

                }
                else if (item.HasThisRep == "0")
                {
                    item.Checked = true;
                    item.FolderCheckDisabled = false;

                }

                else
                {
                    item.Checked = false;
                    item.FolderCheckDisabled = false;

                }
            }

            //OrderBy(i=> Int32.Parse(i.RepId))
            var groups = nodeList.GroupBy(i => i.parentKey);
            var groupsDocProj = nodeList.Where(i => i.parentKey == "1");

            var roots = groups.FirstOrDefault(g => g.Key == "0").ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }


        public IList<node> BuildTreeForProjectAndMS(string controller, string striprojectNumber, string modeleSelection = "", bool disableAll = false, bool folderCheckDisabled = false)

        {
            //dans cette fonction, on va afficher un arbre qui correspond au projet avec
            //les repertoires du modele de selection choisi. 
            //les repertoires du projet seron en rouge; ceux du modeles auront une case pré-cochées

            //1. recuperer l ensemble des rep du  modId du projet
            var sourceProjet = returnListOfNodes(striprojectNumber, disableAll, folderCheckDisabled);
            //2. recuper la liste des rep utilisés par le projet
            // sourceProjet.OrderBy(x => x.name);
            var listRepProjet = FolderUsedInProject(striprojectNumber);
            //3. recuperer la liste des repertoires du modid de base du  mds choisi:
            //mds = modele de selection
            var sourceMDS = FolderUsedInModeleByModId(modeleSelection, disableAll, folderCheckDisabled, "green");
            // sourceMDS.OrderBy(x => x.name);
            // 4.  recuper la liste des rep utilisés par le modele choisi
            var listRepModeleChoisi = FolderUsedInProjectBySelection_Node(modeleSelection);

            //5. parmi les rep de base du modelechoisi (en fait, les node), on ne garde que ceux qui ont un repid dans listRepModeleChoisi:
            var sourceMDS2 = new List<node>();
            foreach (node item in sourceMDS)
            {
                if (listRepModeleChoisi.Contains(item.RepId))
                {
                    sourceMDS2.Add(item);
                }
            }
            //5. a present on va ajouter les rep du modeledeselection avec ceux du projet  
            //en prenant soin d eviter les doublons
            var newlistRepModeleChoisi = new List<string>();
            foreach (string item in listRepModeleChoisi)
            {
                if (!listRepProjet.Contains(item))
                {
                    newlistRepModeleChoisi.Add(item);
                }
            }
            //ici on fait l union..
            var sourceFinal1 = sourceProjet.Union(sourceMDS2).Distinct().ToList();
            //...et ceci pour enlever les doublons sur la base de leur repId
            sourceFinal1 = sourceFinal1.GroupBy(x => x.RepId).Select(x => x.First()).ToList();
            var sourceFinal2 = sourceFinal1.OrderBy(x => x.RepId);
            var testlist = sourceFinal2.Select(x => x.RepId).ToList();
            //6.a present on va mettre en rouge les rep deja utilisés par le projet  et non modifiable 
            //etant donné qu'ils appartiennent deja au projet
            foreach (node item in sourceFinal2)
            {

                foreach (string idp in listRepProjet)
                {

                    if (item != null && item.RepId == idp)
                    {
                        item.Checked = true;
                        item.FolderCheckDisabled = true;
                    }

                }



            }
            foreach (node item in sourceFinal2)
            {

                foreach (string idm in newlistRepModeleChoisi)
                {
                    if (item != null && item.RepId == idm)
                    {

                        item.Checked = true;
                        item.FolderCheckDisabled = false;
                    }

                }


            }

            //OrderBy(i=> Int32.Parse(i.RepId))
            var groups = sourceFinal2.GroupBy(i => i.parentKey);
            var groupsDocProj = sourceFinal2.Where(i => i.parentKey == "1");

            var roots = groups.FirstOrDefault(g => g.Key == "0").ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        public IList<node> BuildTree(string controller, string striprojectNumber, string modeleSelection = "", bool disableAll = false, bool folderCheckDisabled = false)
        {
            //si on a choisi un modele de selection, on va renvoyer un arbre des repertoires correspondant a ce modele de selection
            // dans lequel les repertoires deja existant seront pre-cochés
            //sinon on va renvoyer un larbre des repertoires tel qu il existe sur la base de données: 

            //la source represent tous les repertoires pour un mod_id donné (celui du striprojectNumber) ou
            //ceux pour un mode de selection donné (celui de modeleSelection)
            var source = (modeleSelection == "" || modeleSelection == null) ? returnListOfNodes(striprojectNumber, disableAll, folderCheckDisabled) : FolderUsedInModeleByModId(modeleSelection, disableAll, folderCheckDisabled, "#000080");//
            var usedFolder = (modeleSelection == "" || modeleSelection == null) ? FolderUsedInProject(striprojectNumber) : FolderUsedInProjectBySelection_Node(modeleSelection);


            source.OrderBy(x => x.RepId);
            if (modeleSelection == "" || modeleSelection == null)
            {
                foreach (node item in source)
                {
                    foreach (string id in usedFolder)
                    {
                        if (item.key == id)
                        {

                            item.Checked = true;
                            if (controller != "ModeleSelection")
                            {
                                item.FolderCheckDisabled = true;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string id in usedFolder)
                {
                    foreach (node item in source)
                    {
                        if (item.key == id)
                        {

                            item.Checked = true;
                            if (controller != "ModeleSelection")
                            {
                                item.FolderCheckDisabled = true;
                            }

                        }
                    }
                }

            }
            //OrderBy(i=> Int32.Parse(i.RepId))
            var groups = source.GroupBy(i => i.parentKey);
            var groupsDocProj = source.Where(i => i.parentKey == "1");

            var roots = groups.FirstOrDefault(g => g.Key == "0").ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }


        //tree pour creation
        public IList<node> BuildTreeCreation(int modID, int modSelectionID)
        {
            var source = returnListOfNodesForCreation(modID);
            var usedFolderInSelectionID = FolderUsedInProjectBySelection(modSelectionID);
            foreach (node item in source)
            {
                foreach (string id in usedFolderInSelectionID)
                {
                    if (item.key == id)
                    {
                        item.Checked = true;
                    }
                }
            }

            var groups = source.GroupBy(i => i.parentKey);

            var roots = groups.FirstOrDefault(g => g.Key == "0").ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        private static void AddChildren(node node, IDictionary<string, List<node>> source)
        {
            if (source.ContainsKey(node.key))
            {
                node.children = source[node.key];
                for (int i = 0; i < node.children.Count; i++)
                    AddChildren(node.children[i], source);
            }
            else
            {
                node.children = new List<node>();
            }
        }

        public List<string> FolderUsedInProject(string projectNumber)
        {
            var folderList = _entitiesBPR.usp_ObtenirRepertoireParProjetTTProjetPlus(projectNumber).OrderBy(pr => pr.REP_id)
                                    .Select(pr => pr.REP_id.ToString()).ToList();
            return folderList;
        }

        public List<string> FolderUsedInModeleSelection(string modeleSelection)
        {
            var folderList = _entitiesBPR.Modele_Selection_Repertoire.Where(pr => pr.MOD_SEL_Id.ToString() == modeleSelection)
                                    .Select(pr => pr.REP_Id.ToString()).ToList();
            return folderList;
        }



        public List<string> FolderUsedInProject2(string projectNumber)
        {
            var folderList = _entitiesBPR.usp_ObtenirRepertoireParProjetTTProjetPlus(projectNumber).OrderBy(pr => pr.REP_id)
                                    .Select(pr => pr.REP_id.ToString() + "/" + "*" + pr.REP_Code + "*" + pr.REP_Description).ToList();
            return folderList;
        }
        public List<string> FolderUsedInProjectBySelection(int modSelectionID)
        {
            var folderList = _entitiesBPR.usp_ObtenirRepertoireParModeleSelection(modSelectionID).OrderBy(pr => pr.REP_Id)
                                    .Select(pr => pr.REP_Code + " * " + pr.REP_Id.ToString()).ToList();
            return folderList;
        }



        public List<string> getProjectsIDList()
        {
            var projectsList = _entitiesBPR.Projets.Where(x => x.ETA_PRO_ID == 1)
                                    .Select(x => x.PRO_Id).ToList();
            return projectsList;
        }

        public List<string> getModelsList()
        {
            var user = UserService.CurrentUser.TTAccount;
            var projectsList = _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user || x.MOD_SEL_Utilisateur == "COMMUN"))
                                    .Select(x => x.MOD_SEL_Nom).ToList();
            return projectsList;

        }


        public int getProjectNumber()
        {
            int? result = _entities.usp_GetProjectNumberCounter().FirstOrDefault();
            return result.GetValueOrDefault();
        }








        public List<string> ReturnListOfChildNodes(string nodeId)
        {
            var listOfChildNodes = _entitiesBPR.Repertoires.Where(p => p.REP_Parent.ToString() == nodeId)
                              .Select(p => p.REP_Id.ToString()).ToList();

            return listOfChildNodes;
        }

        public List<string> ReturnListOfParentdNodes(string nodeId)
        {
            var listOfParentNodes = _entitiesBPR.Repertoires.Where(p => p.REP_Id.ToString() == nodeId)
                              .Select(p => p.REP_Parent.ToString()).ToList();

            return listOfParentNodes;
        }


        public List<string> ReturnListOfAllDescendance(string ModId, string selectionModele)
        {
            var modid = Int32.Parse(ModId);
            var selectmod = Int32.Parse(selectionModele);

            var listDescendance = _entitiesBPR.usp_GetChildrenByParentIDandModID(modid, selectmod).Select(p => p.REP_Id.ToString()).ToList();
            return listDescendance;
        }


        public string registerProjectInOSProject(string
            AbreviatedTitle,
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
            string ProgramMgr,
            string ClientFinal,
            string ClientContact,
            string ApproximateAmount,
            string RefAndComments,
            string currentUser,
            string projectNumber,
            byte? status_id,
            bool HasprojectStruture,
            string ClientProjectManager,
            string ClientEmailProjectManager
)
        {
            try
            {
                var entity = new OS_Project();
                entity.Is_Master = HasAMasterProject == "1" ? false : true;
                entity.Project_Description = AbreviatedTitle;
                entity.Project_Master = MasterProjectNumber;
                entity.PhaseCode = PhaseCode;
                entity.PhaseCodeType_ID = (byte)Int32.Parse(PhaseCodeID);
                entity.Contact_Client = ClientContact;
                entity.Customer_Number = ClientNumber;
                entity.Customer_Name = ClientName;
                entity.Company = CompanyId;
                entity.Acct_Group_ID = Int32.Parse(BusinessUnit);
                entity.Acct_Center_ID = Int32.Parse(ORG);
                entity.Project_Mgr_Number = ProjectMgr;
                entity.Program_Mgr_Number = ProgramMgr;
                entity.End_Client_Name = ClientFinal;
                entity.Contact_Client = ClientContact;
                entity.Approx_Amount = ApproximateAmount == "" ? 0 : Int32.Parse(ApproximateAmount);
                entity.Comments = RefAndComments;
                entity.Initiated_By = currentUser;
                entity.Project_Number = projectNumber;
                entity.SubmittedDate = DateTime.Now;
                entity.Status_ID = status_id;
                entity.Doc_Status_ID = HasprojectStruture == true ? (byte)1 : (byte)5;

                if (ClientProjectManager == null || ClientProjectManager == "") 
                {
                    entity.Client_ProjectManager = "NULL";
                }
                else
                {
                    entity.Client_ProjectManager = ClientProjectManager;
                }

                if (ClientEmailProjectManager == null || ClientEmailProjectManager == "")
                {
                    entity.Client_Email_ProjectManager = "NULL";
                }
                else
                {
                    entity.Client_Email_ProjectManager = ClientEmailProjectManager;
                }
                                
                _entities.OS_Project.Add(entity);
                _entities.SaveChanges();

                return "successInsertionInTT";
            }

            catch (Exception e)
            {
                string info = "AbreviatedTitle: " + AbreviatedTitle + "; " +
                    "HasAMasterProject: " + HasAMasterProject + "; " +
                    "MasterProjectNumber: " + MasterProjectNumber + "; " +
                    "PhaseCode: " + PhaseCode + "; " +
                    "PhaseCodeID: " + PhaseCodeID + "; " +
                    "ClientNumber: " + ClientNumber + "; " +
                   "ClientName: " + ClientName + "; " +
                    "CompanyId: " + CompanyId + "; " +
                    "BusinessUnit: " + BusinessUnit + "; " +
                    "ORG: " + ORG + "; " +
                    "ProjectMgr: " + ProjectMgr + "; " +
                    "ProgramMgr: " + ProgramMgr + "; " +
                    "ClientFinal: " + ClientFinal + "; " +
                    "ClientContact: " + ClientContact + "; " +
                    "ApproximateAmount: " + ApproximateAmount + "; " +
                    "RefAndComments: " + RefAndComments + "; " +
                    "currentUser: " + currentUser + "; " +
                    "projectNumber: " + projectNumber + "; " +
                    "status_id: " + status_id.ToString() + "; " +                  
                    "HasprojectStruture: " + HasprojectStruture.ToString();

                addErrorLog(e, "HomeService", "registerProjectInOSProject", projectNumber, info);
                return "failedInsertionInTT";

            }

        }


        public string returnProjectNumber()
        {
            var newNumber = _entities.usp_GetProjectNumberCounter().FirstOrDefault();
            return newNumber.ToString();
        }

        public structureItem addSecurityOnFolders(string projectid, string modID, Nullable<byte> DoDelBatFile, string function, string controller)
        {
            int value = 0;
            structureItem result = new structureItem();
            int trialNb = 1;

            result.Value = "success";

            do
            {
                try
                {
                    //_entitiesBPR.Database.CommandTimeout = 480; //8 min
                    value = 1;// _entitiesBPR.usp_NEW_TT_Security_INPROGRESS_TTProjetPlus(projectid, DoDelBatFile);
                    result.Name = value.ToString();

                    if (value == 0)
                    {
                        //SI Len(@FileOutput) = 0 , il faut soulever une erreur et loguer l'erreur dans une table… l'application de la sécurité n'a pas fonctionné
                        addErrorLogSpecial(result.Name, controller + "dans if à partir de HomeService", function, projectid);
                        result.Value = "successAttention";
                    }
                    else
                    {
                        //2) SI le projet a un modèle (10,11 ou 20) ALORS
                        // Cette fonction est appelée si modèle du projet est 10,11 ou 20
                        if (modID == "10" || modID == "11" || modID == "20")
                        {
                            result.Value = UpdateConfidentialFolderSecurity(projectid);
                            break;
                        }
                        else
                        {
                            result.Value = "success";
                            break;
                        }
                    }

                }
                catch (Exception e)
                {
                    addErrorLog(e, controller + " à partir de HomeService", function, projectid, "trialNb = " + trialNb.ToString());
                    result.Name = e.InnerException.ToString();
                    result.Value = "fail";

                    // LG 20210224: Présentement le code arrive ici lorsque la SP soulève cette erreur: 
                    // System.Data.SqlClient.SqlException: Transaction (Process ID 52) was deadlocked on lock resources with another process and has been chosen as the deadlock victim.
                    // Dans ce cas, on va refaire un appel une 2ième fois
                }

                trialNb = trialNb + 1;

            } while (trialNb <= 2);

            return result;
        }

        public void addSecurityOn10CONFIDENTIELFolder(string projectid)
        {
            try
            {

                var con = new SqlConnection(UserService.TTProjetPlusconnectionStringBPRProjetConf());
                DataSet ds = new System.Data.DataSet();

                SqlParameter parameter = new SqlParameter();
                var cmd = new SqlCommand("[dbo].[usp_Nasuni_Projet10Conf_Table]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                parameter = new SqlParameter();
                parameter.ParameterName = "@Projet";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 100;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = projectid;
                cmd.Parameters.Add(parameter);

                // en secondes donc 20 minutes
                cmd.CommandTimeout = 1200;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (Exception e)
            {
                var test = e.Message;
            }
        }

        public void addErrorLogSpecial(string error, string controller, string controllerFunction, string projectId = "")
        {

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

        //code fourni par Line! merci pour son aide!!!
        // Cette fonction est appelée si modèle du projet est 10,11 ou 20
        public string UpdateConfidentialFolderSecurity(string strProjectNo)
        {
            String strPathServer = "";
            String strPathProject = "";
            String strProjetSecurise = "";

            try
            {
                //1)
                //Récupérer le path partage du serveur où se retrouve le projet
                strPathServer = returnPathPartage(strProjectNo);

                // strPathServer = @"\\tts349test03\Prj_Reg\";
                // strPathProject = strPathServer + strProjectNo;
                strPathProject = strPathServer + "\\" + strProjectNo;

                //2)
                //On vérifie si le répertoire 10CONFIDENTIEL existe sur le serveur pour le projet
                if (Directory.Exists(strPathProject + @"\DOC-PROJ\10\10CONFIDENTIEL"))
                {
                    //3
                    //On appelle la SP qui va sécuriser le répertoire 10CONFIDENTIEL
                    strProjetSecurise = SecureConfidentialFolder(strProjectNo);

                    //4
                    //On confirme que la sécurité est ok si la SP a retourné un path (ce qui veut dire que le 10CONFIDENTIEL doit être sécurisé)
                    if (strProjetSecurise.Length > 0)
                    {
                        ConfirmConfidentialFolderSecurity(strPathProject, strProjectNo);
                        // return "successSecurityOn10Conf";
                    }

                }
                return "success";
            }
            catch (Exception e)
            {
                addErrorLog(e, "HomeService", "UpdateConfidentialFolderSecurity", strProjectNo);
                return "successAttention";

            }

        }

        public String SecureConfidentialFolder(string strProjectNo)
        {
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamString = new System.Data.Entity.Core.Objects.ObjectParameter("FileOutput", typeof(string));

                //var result = _entitiesBPR.usp_MAJ_SecuriteTTProjet("", strProjectNo, 0, myOutputParamString);
                string strProjetSecurise = "";// Convert.ToString(myOutputParamString.Value);

                return strProjetSecurise;
            }
            catch (Exception e)
            {
                return "Erreur";
            }
        }

        public void ConfirmConfidentialFolderSecurity(string strPathProject, string strProjectNo)
        {
            String strNoProjetDansTetraLinx = "";
            String strGroupeAD = "";

            try
            {
                strNoProjetDansTetraLinx = GetProjectNoInTetraLinx(strProjectNo);

                if (strNoProjetDansTetraLinx.Length > 0)
                {
                    strGroupeAD = @"TT\BPR.GL_FIC_10_10CONFIDENTIEL_P";
                }
                else
                {
                    strGroupeAD = @"TT\BPR.GL_FIC_10_10CONFIDENTIEL_OS";
                }

                CheckConfidentialFolderSecurity(strPathProject, strProjectNo, strGroupeAD);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetProjectNoInTetraLinx(String strProjectNo)
        {
            string strProjetASecuriser = "";

            try
            {
                strProjetASecuriser = _entitiesBPR.usp_isProject_TTPlus(strProjectNo).FirstOrDefault();
                if (strProjetASecuriser == null) { strProjetASecuriser = ""; }
            }
            catch (Exception)
            {

                throw;
            }

            return strProjetASecuriser;
        }

        public void CheckConfidentialFolderSecurity(string strPathProject, string strProjectNo, String sGroupeAD)
        {
            Boolean blnTrouve = false;
            var existingACL = Directory.GetAccessControl(strPathProject + @"\DOC-PROJ\10\10CONFIDENTIEL");

            try
            {
                foreach (FileSystemAccessRule rule in existingACL.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                {
                    if (rule.IdentityReference.ToString() == sGroupeAD)
                    {
                        blnTrouve = true;
                        break;
                    }
                }

                if (!blnTrouve)
                {
                    foreach (FileSystemAccessRule rule in existingACL.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                        existingACL.RemoveAccessRuleAll(rule);

                    // On ajoute le groupe Administrateurs en contrôle total
                    SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
                    DirectorySecurity securityRules = new DirectorySecurity();
                    FileSystemAccessRule fsRule =
                                new FileSystemAccessRule(sid, FileSystemRights.FullControl,
                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                PropagationFlags.None, AccessControlType.Allow);
                    existingACL.AddAccessRule(fsRule);
                    Directory.SetAccessControl(strPathProject + @"\DOC-PROJ\10\10CONFIDENTIEL", existingACL);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string returnPathPartage(string projectId)
        {
            var serveurId = _entitiesBPR.Projets.Where(p => p.PRO_Id == projectId).Select(p => p.SER_Id).FirstOrDefault();
            var pathPartage = _entitiesBPR.usp_ObtenirServeur(serveurId).Select(p => p.SER_Chemin_Partage).FirstOrDefault();
            return pathPartage;
        }

        public List<structureItem> ObtainAvailablePhaseCodes2(string masterProjectID)
        {
            var project = _entities.OS_Project.Where(p => p.Project_Number == masterProjectID && p.Is_Master == true).FirstOrDefault();
            if (project != null)
            {
                List<structureItem> returnList = new List<structureItem>();

                string level = "";

                // 1-Boucle qui construit la liste « returnValue » qui va contenir 676 phases de A à Z puis double lettres jusqu’à ZZ
                for (int iterate = 0; iterate < 6; iterate++)
                {
                    for (int count = 65; count < 91; count++)
                    {
                        structureItem item = new structureItem();
                        item.Name = level + char.ConvertFromUtf32(count);
                        item.Value = "2";
                        returnList.Add(item);
                    }

                    level = char.ConvertFromUtf32(65 + iterate);
                }
                var listOfPhasesUsedSoFar = _entities.sp_GetUsedPhase(masterProjectID).Select(p => p.PhaseCode).ToList();
                returnList.RemoveAll(x => listOfPhasesUsedSoFar.Contains(x.Name));
                // 3-Boucle qui conserve les 26 prochaines phases à utiliser
                if (returnList.Count > 26)
                {
                    for (int count = returnList.Count - 1; count > 25; count--)
                    {
                        returnList.RemoveAt(count);
                    }
                }

                return (returnList);
            }
            return null;
        }

        public List<structureItem> ObtainAvailablePhaseCodes1(string masterProjectID)
        {

            var project = _entities.OS_Project.Where(p => p.Project_Number == masterProjectID && p.Is_Master == true).FirstOrDefault();
            if (project != null)
            {
                var listOfPhasesUsedSoFar = _entities.sp_GetUsedPhase(masterProjectID).Select(p => p.PhaseCode).ToList();
                var countOflistOfPhasesUsedSoFar = listOfPhasesUsedSoFar.Count;

                List<structureItem> returnList = new List<structureItem>();

                //string level = "";

                // 1-Boucle qui construit la liste « returnValue » qui va contenir 676 phases de A à Z puis double lettres jusqu’à ZZ
                for (int count = 1; count < countOflistOfPhasesUsedSoFar + 50; count++)
                {
                    structureItem item = new structureItem();
                    item.Name = $"{count:D3}";
                    item.Value = "1";
                    returnList.Add(item);
                }
                foreach (structureItem item in returnList)
                {
                    if (listOfPhasesUsedSoFar.Contains(item.Name))
                    {
                        item.Name = "";
                        item.Value = "";
                    }
                }
                // returnList.RemoveAll(x => listOfPhasesUsedSoFar.Contains(x.Name ));
                List<structureItem> returnList2 = new List<structureItem>();
                foreach (structureItem item in returnList)
                {
                    if (item.Name != "" && item.Value != "")
                    {
                        returnList2.Add(item);
                    }
                }

                // 3-Boucle qui conserve les 26 prochaines phases à utiliser

                if (returnList2.Count > 26)
                {
                    for (int count = returnList2.Count - 1; count > 25; count--)
                    {
                        returnList2.RemoveAt(count);
                    }
                }

                return (returnList2.OrderBy(X => X.Value).ToList());
            }
            return null;
        }

        public string AddProjectInTTProjet(
          string ProjetID, int ModeleSecuriteId, int ServeurID, string ServerModel, int TypeProjet, string UtilisateurId, string customername, string Parent = null, int EtatProjetID = 1, bool isAdmin = false, string SelectionModel = "")
        {

            //1) Ajouter le projet dans la table PROJET
            try
            {
                var user = UserService.CurrentUser.TTAccount;

                // 2020-10-22 On conserve le modèle de sélection dans la colonne PRO_Parent qui n'est jamais utilisé ... utile lorsque problème ou besoin de recréer la structure de répertoires
                Parent = SelectionModel;

                _entitiesBPR.usp_AjouterProjet(ProjetID, ModeleSecuriteId, ServeurID, TypeProjet, Parent, user, EtatProjetID, isAdmin);//ServeurID
            }
            catch (Exception e)
            {
                string info = "ProjetID: " + ProjetID + "; " +
                    "ModeleSecuriteId: " + ModeleSecuriteId.ToString() + "; " +
                    "ServeurID: " + ServeurID.ToString() + "; " +
                    "ServerModel: " + ServerModel + "; " +
                    "TypeProjet: " + TypeProjet.ToString() + "; " +
                    "UtilisateurId: " + UtilisateurId + "; " +
                   "EtatProjetID: " + EtatProjetID.ToString() + "; " +
                    "SelectionModel: " + SelectionModel;
                addErrorLog(e, "HomeService", "AddProjectInTTProjet", ProjetID, info);
                if (e.InnerException.ToString().Contains("Un projet existe deja dans la table projet"))
                {
                    return "alreadyExistingProject";
                }
                else return "failedIsertionInBPR";
            }

            // 2) Ajouter les répertoires pour le projet selon le modèle de sélection choisi
            try
            {
                if (SelectionModel == "")
                {
                    _entitiesBPR.usp_AjouterRepertoireProjet_TTProjetPlus(ProjetID, null);
                }
                else
                {
                    var selecMdod = Int32.Parse(SelectionModel);
                    _entitiesBPR.usp_AjouterRepertoireProjet_TTProjetPlus(ProjetID, selecMdod);
                }


            }
            catch (Exception e)
            {
                addErrorLog(e, "HomeService", "AddProjectInTTProjet", ProjetID);
                return "failedAdditionFolder";
            }


            //  3) Créer les répertoires sur le serveur

            var step3 = checkFoldersForProject(ProjetID, customername);

            //addSecurityOn10CONFIDENTIELFolder(ProjetID);
            ////if (step3 == "success")
            ////{
            ////    //4) Appliquer la sécurité sur le projet

            ////    var outputResult = addSecurityOnFolders(ProjetID, ServerModel, 1, "AddProjectInTTProjet", "FolderStructure");
            ////    if (outputResult.Value == "success")
            ////    {
            ////        return "successfullProcess";
            ////    }

            ////    else return "failedApplySecurity";
            ////}
            ////else return "failedCreateFolderOnServer";
            return "successfullProcess";
        }

        public string checkFoldersForProject(string projectId, string customername)
        {
            //ici il faut eventuellement injecter des formulaires dans les folders - demande de TQE 
            /*
             * voici  la methode utilisé pôpur trouver les rep-id des folders dans lesquels on doit ajouter des formulaires:
             * 1. on a recu de JP veillette une listedu genre:
             * repertoire   sous repertoire   no de deco                titre et lien
             * 40              CCO            000000-60-REG-002_F       Registre des changements
             * etc
             * 
             * on a cherché les rep_id de tout les repertoire (40, par exemple est le rep_code)
             * requete sql:
             *  select rep_code, rep_id from [Repertoire_2021_08_20] --where rep_code ='61'
                 where rep_code in ('10'	, '11',	 '12',	 '13',	 '20',	 '30',	 '30',	 '30',	 '31',	 '32',	 '33',	 '40',	 '40',	 '41',	 '42',	 '43',	 '44',	 '50',	 '60',	 '61',	 '63',	 '64',	 '65',	 '66',	 '67',	 '68',	 '69',	 '69',	 '70',	 '71',	 '72',	 '73',	 '74',	 '75',	 '76',	 '80',	 '80',	 '81',	 '82',	 '83',	 '83',	 '83',	 '84',	 '85',	 '86',	 '87',	 '88',	 '89',	 '90',	 '91',	 '92',	 '92',	 '93',	 '93',	 '95',	 '96',	 '97',	 '98',	 '99',	 '00',	 '6A',	 '6C',	 '6E',	 '6M')
                order by rep_code

            ces rep_id represent les rep_id des folder parents qui contiennent les sous repertoire dans lesquels on doit injecter les formulaire:
                ex:  le rep_id du repertoire dont le code est 40 -comme dans l'exemple- est 2012
                on doit injecter un formulaire dans le sous-repertoire CCO du repertoire qui a un rep_id=2012 (donc avec rep_code = 40)
                le rep_id de ce sous repertoire est: 2084

            voici la requete sql:

 select distinct convert(nvarchar(20),rep_parent)+'-'+convert(nvarchar(20),rep_code) as 'fred: rep_parent-sousrepertoire', rep_id as 'rep_id of sub folder' from [Repertoire] 
 where 
(
(rep_parent =  2000 and rep_code= ' ') or
(rep_parent =  2001 and rep_code= ' ') or
(rep_parent =  2002 and rep_code= ' ') or
(rep_parent =  2004 and rep_code= ' ') or 
(rep_parent =  2005 and rep_code= ' ') or
...

)
on a donc a present la liste des rep_id dans lesquels on doit injecter des formluaires.
fred:rep_parent-sousrepertoire	rep_id of sub folder
                2008-KOM	       2073
                2008-MOM	       2074
                2012-CCO	       2084
...            
  voici le code qui génére la table en sql (sylvain 2022-04-28): il faudra executé cette requete en prod pour produire une table toute neuve
                          select a.*,b.[NumeroDoc],IDENTITY(int, 1,1) AS itemId
                        into [TQEDocsToInsertInSubForlders_Sly]
                        from
                        (
                        select p.REP_Code,p.REP_Id,e.REP_Code as [SousRep_code],e.REP_Id as [SousRep_id]
                        from Repertoire P
                        left join Repertoire e
                        on p.REP_Id = e.REP_Parent
                        and p.MOD_Id = e.MOD_Id
                        where p.MOD_Id =30
                        ) as a
                        full join
                        [dbo].[TQEDocsToInsertInSubForlders] b
                        on a.rep_id = b.rep_id
                        and a.[SousRep_Id] = b.[SousRep_Id]


             * */
            //pour obtenir les rep descrition et rep_id pour un modele de selection: bpr[usp_ObtenirArborescenceRepertoireModeleTTProjetPlus]
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

            //2. si ces paths n'existent pas sur le serveur on les crée:
            List<string> listDescription = new List<string>();
            try
            {
                IntPtr userHandle = IntPtr.Zero;
                WindowsImpersonationContext impersonationContext = null;

                foreach (tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni item in listPaths)
                    
                {
                    bool exists = System.IO.Directory.Exists(item.Folder);
                    if (!exists)
                    {

                        try
                        {
                            //A DECOMMENTER POUR LA DEV!!!!! NE PAS OUBLIER!!!!!!!!!!!
                            //var valueTTProjetPlusUserNasuni = UserService.TTProjetPlusUserNasuni(); 
                            //var valueTTProjetPlusPwdNasuni = UserService.TTProjetPlusPwdNasuni();
                            //var valueTTProjetPlusEnvrNasuni = UserService.TTProjetPlusEnvrNasuni();

                            //bool loggedOn = LogonUser(valueTTProjetPlusUserNasuni,
                            //           valueTTProjetPlusEnvrNasuni,
                            //           valueTTProjetPlusPwdNasuni,
                            //           2,
                            //           0,
                            //           ref userHandle);

                            //if (!loggedOn)
                            //{
                            //    Console.WriteLine("Exception impersonating user, error code: " + Marshal.GetLastWin32Error());

                            //}
                            //else
                            //{
                                // Begin impersonating the user

                                //A DECOMMENTER POUR DEV!!! NE PAS OUBLIER!!!!!!!!!!!!
                                //impersonationContext = WindowsIdentity.Impersonate(userHandle);

                                //run the program with elevated privileges (like file copying from a domain server)
                                Directory.CreateDirectory(item.Folder);//item.Folder                                
                                                                       // }

                            if (System.Web.HttpContext.Current.Request.IsLocal)
                            {
                                Directory.CreateDirectory(item.Folder);
                            }
                            else
                            {
                                //A DECOMMENTER POUR LA DEV!!!!! NE PAS OUBLIER!!!!!!!!!!!
                                var valueTTProjetPlusUserNasuni = UserService.TTProjetPlusUserNasuni();
                                var valueTTProjetPlusPwdNasuni = UserService.TTProjetPlusPwdNasuni();
                                var valueTTProjetPlusEnvrNasuni = UserService.TTProjetPlusEnvrNasuni();

                                bool loggedOn = LogonUser(valueTTProjetPlusUserNasuni,
                                           valueTTProjetPlusEnvrNasuni,
                                           valueTTProjetPlusPwdNasuni,
                                           2,
                                           0,
                                           ref userHandle);

                                if (!loggedOn)
                                {
                                    Console.WriteLine("Exception impersonating user, error code: " + Marshal.GetLastWin32Error());

                                }
                                else
                                {
                                //    Begin impersonating the user

                             //   A DECOMMENTER POUR DEV!!!NE PAS OUBLIER!!!!!!!!!!!!
                               impersonationContext = WindowsIdentity.Impersonate(userHandle);

                                   // run the program with elevated privileges(like file copying from a domain server)
                                Directory.CreateDirectory(item.Folder);                                
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception impersonating user: " + ex.Message);
                        }
                        finally
                        {
                            // Clean up
                            if (impersonationContext != null)
                            {
                                impersonationContext.Undo();
                            }

                            if (userHandle != IntPtr.Zero)
                            {
                                CloseHandle(userHandle);
                            }
                        }
                        //System.IO.Directory.CreateDirectory(item.Folder);//item.Folder
                        //pour l'injection                       
                       
                    }

                    if (projectModelSelec == 30)
                    {
                        foreach (TQEDocsToInsertInSubForlders_Sly item2 in listForTQE)
                        {
                            if (item.REP_Id == item2.SousRep_id)
                            {
                                if (item2.Customer_Name == null)
                                {
                                    //injection; par exemple:
                                    try
                                    {
                                        System.IO.File.Copy(item2.NumeroDoc, item.Folder);

                                    }
                                    catch (Exception e)
                                    {
                                        try
                                        {
                                            System.IO.File.Copy(item2.NumeroDoc, item.Folder + "\\" + item2.NumeroDoc.Split('\\')[6]);
                                        }
                                        catch (Exception e1)
                                        {
                                            continue;
                                        }

                                    }
                                }
                                else
                                {
                                    if (item2.Customer_Name == customername)
                                    {
                                        //injection; par exemple:
                                        try
                                        {
                                            System.IO.File.Copy(item2.NumeroDoc, item.Folder);

                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                System.IO.File.Copy(item2.NumeroDoc, item.Folder + "\\" + item2.NumeroDoc.Split('\\')[6]);
                                            }
                                            catch (Exception e1)
                                            {
                                                continue;
                                            }

                                        }
                                    }
                                }

                            }

                        }
                    }                    
                    SetFolderPermission_TTProjet(item.Folder, item.compte, item.Security);
                }

                //addSecurityOn10CONFIDENTIELFolder(projectId);
                _entitiesBPR.usp_Delete_tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni(seqID);
                return "success";
            }
            catch (Exception e)
            {
                addErrorLog(e, "HomeService", "checkFoldersForProject", projectId);
                return e.InnerException.ToString();
            }

        }

        //public string checkFoldersForProject(string projectId)
        //{
        //    //1. recuperer tous les paths en fonctions des repertoires pour ce projet dans la bd:
        //    var listPaths = _entitiesBPR.usp_ObtenirRepertoireParProjetTTProjetPlus(projectId).Select(p => p.Folder).ToList();
        //    //2.  si ces paths n'existent pas sur le serveur on les crée:
        //    try
        //    {
        //        foreach (string item in listPaths)
        //        {
        //            bool exists = System.IO.Directory.Exists(item);
        //            if (!exists)
        //                System.IO.Directory.CreateDirectory(@"\\TTS367FS1.tt.local\prj_reg\newTest2099");

        //        }
        //        return "success";
        //    }
        //    catch (Exception e)
        //    {
        //        addErrorLog(e, "HomeService", "checkFoldersForProject", projectId);
        //        return e.InnerException.ToString();
        //    }

        //}


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

        public void addErrorLog(Exception outputResult, string controller, string controllerFunction, string projectId = "", string info = "")
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
            entity.InnerException = "Project info: " + info + Environment.NewLine + " Error: " + error;
            entity.Url = "URL: " + HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0] + " / projectId: " + projectId;
            _entities.TTProjetPlus_Error_Log.Add(entity);
            _entities.SaveChanges();
        }

        public List<masterProjectItem> displayNextResults(string searchTerm, int start, int end)
        {
            List<masterProjectItem> listOfProject = new List<masterProjectItem>();
            var listToReturn = from p in _entities.usp_Search_MasterProjectNumbers(searchTerm, start, end)
                               select new masterProjectItem
                               {
                                   Name = p.Project_Description.Length > 50 ? p.Project_Description.Substring(0, 50) : p.Project_Description,
                                   ProjectNumber = p.Project_Number,
                                   ClientName = p.Customer_Name,
                                   Org = p.organisation
                               };
            listOfProject = listToReturn.ToList();
            return listOfProject;

        }

        public int? returnNumberOfMasterProjects()
        {
            var number = _entities.usp_ReturnNumberOfMasterProjects().FirstOrDefault();
            if (number == 0)
                return 0;
            else return number;


        }


        public string returnPath(string serverNumber)
        {
            var serId = Int32.Parse(serverNumber);
            var path = _entitiesBPR.Serveurs.Where(p => p.SER_Id == serId).Select(p => p.SER_Chemin_Partage).FirstOrDefault();
            return path;

        }

        public string HasAccessToNewProjectNumber(string employeeId)
        {
            var HasAccess = _entities.TTProjetSecurities.Where(p => p.Employee_Number == employeeId && (p.Security_Role == "14" || p.Security_Role == "1" || p.Security_Role == "2")).FirstOrDefault();
            return HasAccess == null ? "false" : "true";
        }

        public string HasAccessToSecurite(string employeeId)
        {
            var HasAccess = _entities.TTProjetSecurities.Where(p => p.Employee_Number == employeeId && (p.Security_Role == "1" || p.Security_Role == "2")).FirstOrDefault();
            return HasAccess == null ? "false" : "true";
        }

        public List<structureItem> GetAdministratorsList(string type)
        {                        
            List<structureItem> si = new List<structureItem>();
            var now = DateTime.Now;
            var items = (from s in _entities.TTProjetSecurities
                join e in _entities.EmployeeActif_Inactive on s.Employee_Number equals e.EMPLOYEE_NUMBER
                where s.Security_Role.ToString() == type && e.EFFECTIVE_START_DATE <= now && e.EFFECTIVE_END_DATE >= now
                         select new
                {
                    Name = s.FULL_NAME,
                    Description = s.Company,
                    Value = s.Employee_Number,
                    Email = e.EMAIL_ADDRESS
                });

            List<structureItem> listFinal = items.Select(i =>
            new structureItem
            {
                Name = i.Name,
                Description = i.Description,
                Value = i.Value,
                email = i.Email
            }).ToList();

            return listFinal;
        }

        public string checkNameAvailability(string name)
        {
            var count = _entities.OS_Project.Where(p => p.Project_Description == name).Count();
            return count > 0 ? "notAvailable" : "available";
        }

        public string checkForDoublons(string master, string phase, string company)
        {
            var count = _entities.OS_Project.Where(p => p.Project_Master == master && p.PhaseCode == phase && p.Company == company).Count();
            return count > 0 ? "cannotContinue" : "canContinue";
        }

        public string checkIfMasterProjectNumberExists(string master)
        {
            var count = _entities.OS_Project.Where(p => p.Project_Number == master && p.Is_Master == true).Count();
            return count > 0 ? "canContinue" : "cannotContinue";
        }

        public OS_Project returnDataForMasterProject(string masterProjectNumber)
        {
            var osDataForMaster = _entities.OS_Project.Where(p => p.Project_Number == masterProjectNumber).FirstOrDefault();
            if (osDataForMaster != null)
            {               
                var now = DateTime.Now;
                var projectManagerName = (osDataForMaster.Project_Mgr_Number == null || osDataForMaster.Project_Mgr_Number == "") ? "" : _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now && p.EMPLOYEE_NUMBER == osDataForMaster.Project_Mgr_Number).Select(p => p.FULL_NAME).FirstOrDefault();
                var programManagerName = (osDataForMaster.Program_Mgr_Number == null || osDataForMaster.Program_Mgr_Number == "") ? "" : _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now && p.EMPLOYEE_NUMBER == osDataForMaster.Program_Mgr_Number).Select(p => p.FULL_NAME).FirstOrDefault();
                osDataForMaster.Project_Mgr_Number = projectManagerName;
                osDataForMaster.Program_Mgr_Number = programManagerName;
                return osDataForMaster != null ? osDataForMaster : null;

            }
            return null;
        }

        public string SendOSEmail(string projectNumber, string TQELang)
        {
            //var db = _entities;
            int OSProjectId;
            string EmailAddresses = "";

            try
            {

                OSProjectId = _entities.OS_Project.Where(x => x.Project_Number == projectNumber).Select(x => x.OS_Project_ID).FirstOrDefault();

                //var con = new SqlConnection(db.Database.Connection.ConnectionString);
                var con = new SqlConnection(UserService.TTProjetPlusconnectionStringBPRProjetUser());
                DataSet ds = new System.Data.DataSet();

                SqlParameter parameter = new SqlParameter();
                var cmd = new SqlCommand("[dbo].[usp_SendOSEmail]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                parameter = new SqlParameter();
                parameter.ParameterName = "@osprojectid";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = OSProjectId;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@language";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 2;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = TQELang;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@EmailAddressesOutput";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 8000;
                parameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parameter);

                // en secondes donc 5 minutes
                cmd.CommandTimeout = 300;

                con.Open();
                cmd.ExecuteNonQuery();
                EmailAddresses = parameter.Value.ToString();
                con.Close();

                return EmailAddresses;

            }
            catch (Exception e)
            {
                return e.InnerException.ToString();
            }

            //EmailAddresses = _entities.OS_Project.Where(x => x.Project_Number == projectNumber).Select(x => x.OS_Project_ID);
            //EmailAddresses = _entities.Acct_Group_Center_Email.Where(x => x.Usage == "" && x.isActive == 1)
            // completer la sp [usp_SendOSEmail], modifier le EF et envoyer projectd et TQELang            
        }

        public Boolean AutomaticEmailAddresses(string BusinessUnit, string ORG)
        {
            int intBusinessUnit = int.Parse(BusinessUnit);
            int intOrg = int.Parse(ORG);

            var count = _entities.Acct_Group_Center_Email
                .Where(p => 
                    (p.Usage == "usp_SendOSEmail" &&
                    p.isActive == true &&
                    ((p.Acct_Group_ID == intBusinessUnit && p.Acct_Center_ID == null) || 
                    (p.Acct_Group_ID == intBusinessUnit && p.Acct_Center_ID == intOrg))
                    )
                    )
                .Count();
            return count > 0 ? true : false;
        }

        public int GetProjectModId(string pro_id)
        {
            int modId = _entitiesBPR.Projets.Where(p => p.PRO_Id == pro_id).Select(p => p.MOD_SEC_Id).FirstOrDefault();

            return modId;

        }

        public Boolean isProjectOnTestServer(string pro_id)
        {
            var count = _entitiesBPR.Projets
                .Where(p => p.PRO_Id == pro_id && p.SER_Id == 68)
                .Count();
            return count > 0 ? true : false;
        }


        public void SetFolderPermission(string folderPath)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(folderPath);
                var directorySecurity = directoryInfo.GetAccessControl();
                //var currentUserIdentity = WindowsIdentity.GetCurrent();
                var fileSystemRule = new FileSystemAccessRule("TT.Projects.Quebec",
                                                              FileSystemRights.Modify,
                                                              InheritanceFlags.ObjectInherit |
                                                              InheritanceFlags.ContainerInherit,
                                                              PropagationFlags.None,
                                                              AccessControlType.Allow);

                directorySecurity.AddAccessRule(fileSystemRule);
                directoryInfo.SetAccessControl(directorySecurity);
            }
            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public void SetFolderPermission_TTProjet(string folderPath, string Compte, string Security)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

                DirectorySecurity directorySecurity = Directory.GetAccessControl(folderPath);

                string parentFolderPath = Path.GetDirectoryName(folderPath);

                // Étape 1 : Désactivez l'héritage sur le répertoire enfant.
                directorySecurity.SetAccessRuleProtection(true, false);

                // Étape 2 : Copiez les règles Full Control du parent vers l'enfant.
                AuthorizationRuleCollection parentRules = Directory.GetAccessControl(directoryInfo.Parent.FullName).GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in parentRules)
                {
                    if (rule is FileSystemAccessRule fsAce)
                    {
                        if ((fsAce.FileSystemRights & FileSystemRights.FullControl) == FileSystemRights.FullControl)
                        {
                            directorySecurity.AddAccessRule(fsAce);
                        }
                    }
                }

                // Étape 3 : Supprimez les règles héritées du parent.
                AuthorizationRuleCollection accessRules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in accessRules)
                {                   
                        if (rule is FileSystemAccessRule fsAce)
                        {
                            if ((fsAce.FileSystemRights & FileSystemRights.FullControl) != FileSystemRights.FullControl)
                            {
                                directorySecurity.RemoveAccessRule(fsAce);
                            }
                        }
                                                        
                }

                directoryInfo.SetAccessControl(directorySecurity);
                           

                var fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;

             
                //plusieurs droits sur le répertoire
                if (Compte.Contains(","))
                {
                    string[] listeCompteInitial = Compte.Trim().TrimEnd('\n').TrimStart('\n').Split(',');
                    string[] listeSecuriteInitial = Security.Trim().TrimEnd('\n').TrimStart('\n').Split(',');

                    if (listeCompteInitial.Length == listeSecuriteInitial.Length)
                    {
                        for (int i = 0; i < listeCompteInitial.Length; i++)
                        {
                            string compte = listeCompteInitial[i].Trim().TrimEnd('\n').TrimStart('\n');
                            string securite = listeSecuriteInitial[i].Trim().TrimEnd('\n').TrimStart('\n');

                            if (securite == "Modify")
                            {
                                fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;
                            }
                            else
                            {
                                fileSystemRightsNew = FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize;
                            }

                            var fileSystemRuleNew = new FileSystemAccessRule(compte,
                                                                            fileSystemRightsNew,
                                                                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                                            PropagationFlags.None,
                                                                            AccessControlType.Allow);

                            directorySecurity.AddAccessRule(fileSystemRuleNew);
                        }
                    }

                }
                else
                {  //un seul droit sur le répertoire
                    //

                    // Ajoutez vos nouvelles règles d'accès
                    string[] listeCompteInitial = Compte.Trim().TrimEnd('\n').TrimStart('\n').Split(',');


                    if (Security == "Modify")
                    {
                        fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;
                    }
                    else
                    {
                        fileSystemRightsNew = FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize;
                    }

                    var fileSystemRuleNew = new FileSystemAccessRule(Compte.Trim().TrimEnd('\n').TrimStart('\n'),
                                                            fileSystemRightsNew,
                                                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                            PropagationFlags.None,
                                                            AccessControlType.Allow);

                    directorySecurity.AddAccessRule(fileSystemRuleNew);

                }
                
                //// Appliquez les modifications
                //directorySecurity.SetAccessRuleProtection(false, false);
                Directory.SetAccessControl(folderPath, directorySecurity);
            }
            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public string registerProjectInOSProjectEnv(
           string projectNumber,
           string AbreviatedTitle,
           string ProjectMgrNumber,           
           string currentUser)
        {
            try
            {
                var entity = new OS_Project_Env();
                entity.Project_Number = projectNumber;
                entity.Project_Description = AbreviatedTitle;
                entity.Project_Mgr_Number = ProjectMgrNumber;
                entity.Initiated_By = currentUser;

                _entities.OS_Project_Env.Add(entity);
                _entities.SaveChanges();

                return "successInsertionInOSProjectEnv";
            }

            catch (Exception e)
            {
                string info = "Project_Description: " + AbreviatedTitle + "; " +
                    "Project_Number: " + projectNumber + "; " +
                    "Project_Mgr_Number: " + ProjectMgrNumber + "; " +
                    "Initiated_By: " + currentUser;
                addErrorLog(e, "HomeService", "registerProjectInOSProjectDev", projectNumber, info);
                return "failedInsertionInOSProjectEnv";

            }

        }
        public string AddProjectInTTProjetWithDiligenteEnv(
                    string ProjetID, int ModeleSecuriteId, int ServeurID, string ServerModel, int TypeProjet, string UtilisateurId, string customername, string Parent = null, int EtatProjetID = 1, bool isAdmin = false, string SelectionModel = "")
        {

            //1) Ajouter le projet dans la table PROJET
            try
            {
                var user = UserService.CurrentUser.TTAccount;

                // 2020-10-22 On conserve le modèle de sélection dans la colonne PRO_Parent qui n'est jamais utilisé ... utile lorsque problème ou besoin de recréer la structure de répertoires
                Parent = SelectionModel;

                _entitiesBPR.usp_AjouterProjet(ProjetID, ModeleSecuriteId, ServeurID, TypeProjet, Parent, user, EtatProjetID, isAdmin);//ServeurID
            }
            catch (Exception e)
            {
                string info = "ProjetID: " + ProjetID + "; " +
                "ModeleSecuriteId: " + ModeleSecuriteId.ToString() + "; " +
                "ServeurID: " + ServeurID.ToString() + "; " +
                "ServerModel: " + ServerModel + "; " +
                "TypeProjet: " + TypeProjet.ToString() + "; " +
                "UtilisateurId: " + UtilisateurId + "; " +
                "EtatProjetID: " + EtatProjetID.ToString() + "; " +
                "SelectionModel: " + SelectionModel;
                addErrorLog(e, "HomeService", "AddProjectInTTProjet", ProjetID, info);
                if (e.InnerException.ToString().Contains("Un projet existe deja dans la table projet"))
                {
                    return "alreadyExistingProject";
                }
                else return "failedIsertionInBPR";
            }

            // 2) Ajouter les répertoires pour le projet selon le modèle de sélection choisi
            try
            {
                if (SelectionModel == "")
                {
                    _entitiesBPR.usp_AjouterRepertoireProjet_TTProjetPlus(ProjetID, null);
                }
                else
                {
                    var selecMdod = Int32.Parse(SelectionModel);
                    _entitiesBPR.usp_AjouterRepertoireProjet_TTProjetPlus(ProjetID, selecMdod);
                }


            }
            catch (Exception e)
            {
                addErrorLog(e, "HomeService", "AddProjectInTTProjet", ProjetID);
                return "failedAdditionFolder";
            }


            //  3) Créer les répertoires sur le serveur

            var step3 = checkFoldersForProjectEnvDiligente(ProjetID, customername);
                      
            return "successfullProcess";
        }

        public string checkFoldersForProjectEnvDiligente(string projectId, string customername)
        {
            
            //pour obtenir les rep descrition et rep_id pour un modele de selection: bpr[usp_ObtenirArborescenceRepertoireModeleTTProjetPlus]
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

            //2. si ces paths n'existent pas sur le serveur on les crée:
            List<string> listDescription = new List<string>();
            try
            {
                IntPtr userHandle = IntPtr.Zero;
                WindowsImpersonationContext impersonationContext = null;

                foreach (tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni item in listPaths)

                {
                    bool exists = System.IO.Directory.Exists(item.Folder);
                    if (!exists)
                    {

                        try
                        {
                            var valueTTProjetPlusUserNasuni = UserService.TTProjetPlusUserNasuni();
                            var valueTTProjetPlusPwdNasuni = UserService.TTProjetPlusPwdNasuni();
                            var valueTTProjetPlusEnvrNasuni = UserService.TTProjetPlusEnvrNasuni();

                            bool loggedOn = LogonUser(valueTTProjetPlusUserNasuni,
                                       valueTTProjetPlusEnvrNasuni,
                                       valueTTProjetPlusPwdNasuni,
                                       2,
                                       0,
                                       ref userHandle);

                            if (!loggedOn)
                            {
                                Console.WriteLine("Exception impersonating user, error code: " + Marshal.GetLastWin32Error());

                            }
                            else
                            {
                                // Begin impersonating the user
                                impersonationContext = WindowsIdentity.Impersonate(userHandle);

                                //run the program with elevated privileges (like file copying from a domain server)
                                Directory.CreateDirectory(item.Folder);//item.Folder                                
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception impersonating user: " + ex.Message);
                        }
                        finally
                        {
                            // Clean up
                            if (impersonationContext != null)
                            {
                                impersonationContext.Undo();
                            }

                            if (userHandle != IntPtr.Zero)
                            {
                                CloseHandle(userHandle);
                            }
                        }
                        //System.IO.Directory.CreateDirectory(item.Folder);//item.Folder
                        //pour l'injection                       

                    }

                    if (projectModelSelec == 30)
                    {
                        foreach (TQEDocsToInsertInSubForlders_Sly item2 in listForTQE)
                        {
                            if (item.REP_Id == item2.SousRep_id)
                            {
                                if (item2.Customer_Name == null)
                                {
                                    //injection; par exemple:
                                    try
                                    {
                                        System.IO.File.Copy(item2.NumeroDoc, item.Folder);

                                    }
                                    catch (Exception e)
                                    {
                                        try
                                        {
                                            System.IO.File.Copy(item2.NumeroDoc, item.Folder + "\\" + item2.NumeroDoc.Split('\\')[6]);
                                        }
                                        catch (Exception e1)
                                        {
                                            continue;
                                        }

                                    }
                                }
                                else
                                {
                                    if (item2.Customer_Name == customername)
                                    {
                                        //injection; par exemple:
                                        try
                                        {
                                            System.IO.File.Copy(item2.NumeroDoc, item.Folder);

                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                System.IO.File.Copy(item2.NumeroDoc, item.Folder + "\\" + item2.NumeroDoc.Split('\\')[6]);
                                            }
                                            catch (Exception e1)
                                            {
                                                continue;
                                            }

                                        }
                                    }
                                }

                            }

                        }
                    }
                    SetFolderPermission_TTProjetEnvDiligence(item.Folder, item.compte, item.Security);
                }

                //addSecurityOn10CONFIDENTIELFolder(projectId);
                _entitiesBPR.usp_Delete_tbl_ObtenirRepertoireParProjetTTProjetPlusNasuni(seqID);
                return "success";
            }
            catch (Exception e)
            {
                addErrorLog(e, "HomeService", "checkFoldersForProject", projectId);
                return e.InnerException.ToString();
            }

        }

        public void SetFolderPermission_TTProjetEnvDiligence(string folderPath, string Compte, string Security)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

                DirectorySecurity directorySecurity = Directory.GetAccessControl(folderPath);

                string parentFolderPath = Path.GetDirectoryName(folderPath);

                // Étape 1 : Désactivez l'héritage sur le répertoire enfant.
                directorySecurity.SetAccessRuleProtection(true, false);

                // Étape 2 : Copiez les règles Full Control du parent vers l'enfant.
                AuthorizationRuleCollection parentRules = Directory.GetAccessControl(directoryInfo.Parent.FullName).GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in parentRules)
                {
                    if (rule is FileSystemAccessRule fsAce)
                    {
                        if ((fsAce.FileSystemRights & FileSystemRights.FullControl) == FileSystemRights.FullControl)
                        {
                            directorySecurity.AddAccessRule(fsAce);
                        }
                    }
                }

                // Étape 3 : Supprimez les règles héritées du parent.
                AuthorizationRuleCollection accessRules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in accessRules)
                {
                    if (rule is FileSystemAccessRule fsAce)
                    {
                        if ((fsAce.FileSystemRights & FileSystemRights.FullControl) != FileSystemRights.FullControl)
                        {
                            directorySecurity.RemoveAccessRule(fsAce);
                        }
                    }

                }

                directoryInfo.SetAccessControl(directorySecurity);


                var fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;


                if (folderPath.EndsWith("10CONFIDENTIEL") || folderPath.EndsWith("10BUD") || folderPath.EndsWith("10FH") || folderPath.EndsWith("10SM") || folderPath.EndsWith("10TAR"))
                {
                    var fileSystemRuleNew = new FileSystemAccessRule("BPR.GL_FIC_Env_VD",
                                                             fileSystemRightsNew,
                                                             InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                             PropagationFlags.None,
                                                             AccessControlType.Allow);
                    
                    directorySecurity.AddAccessRule(fileSystemRuleNew);

                }
                else
                {
                    //plusieurs droits sur le répertoire
                    if (Compte.Contains(","))
                    {
                        string[] listeCompteInitial = Compte.Trim().TrimEnd('\n').TrimStart('\n').Split(',');
                        string[] listeSecuriteInitial = Security.Trim().TrimEnd('\n').TrimStart('\n').Split(',');

                        if (listeCompteInitial.Length == listeSecuriteInitial.Length)
                        {
                            for (int i = 0; i < listeCompteInitial.Length; i++)
                            {
                                string compte = listeCompteInitial[i].Trim().TrimEnd('\n').TrimStart('\n');
                                string securite = listeSecuriteInitial[i].Trim().TrimEnd('\n').TrimStart('\n');

                                if (securite == "Modify")
                                {
                                    fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;
                                }
                                else
                                {
                                    fileSystemRightsNew = FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize;
                                }

                                var fileSystemRuleNew = new FileSystemAccessRule(compte,
                                                                                fileSystemRightsNew,
                                                                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                                                PropagationFlags.None,
                                                                                AccessControlType.Allow);

                                directorySecurity.AddAccessRule(fileSystemRuleNew);
                            }
                        }

                    }
                    else
                    {  //un seul droit sur le répertoire
                       //

                        // Ajoutez vos nouvelles règles d'accès
                        string[] listeCompteInitial = Compte.Trim().TrimEnd('\n').TrimStart('\n').Split(',');


                        if (Security == "Modify")
                        {
                            fileSystemRightsNew = FileSystemRights.Modify | FileSystemRights.Synchronize;
                        }
                        else
                        {
                            fileSystemRightsNew = FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize;
                        }

                        var fileSystemRuleNew = new FileSystemAccessRule(Compte.Trim().TrimEnd('\n').TrimStart('\n'),
                                                                fileSystemRightsNew,
                                                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                                PropagationFlags.None,
                                                                AccessControlType.Allow);

                        directorySecurity.AddAccessRule(fileSystemRuleNew);

                    }
                }

                
                //// Appliquez les modifications                
                Directory.SetAccessControl(folderPath, directorySecurity);
            }
            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public Boolean isProjetEnvDiligence(string ProjectNumber)
        {
            var count = _entities.OS_Project_Env.Where(p => p.Project_Number == ProjectNumber).Count();
            return count > 0 ? true : false;
        }

        public string SendEmailEnvDiligente(string projectNumber)
        {
                        
            string EmailEnvDiligente = "";

            try
            {   
                var con = new SqlConnection(UserService.TTProjetPlusconnectionStringBPRProjetUser());
                DataSet ds = new System.Data.DataSet();

                SqlParameter parameter = new SqlParameter();
                var cmd = new SqlCommand("[dbo].[usp_SendEmailEnvDiligente]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                parameter = new SqlParameter();
                parameter.ParameterName = "@osprojectnumber";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = projectNumber;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@EmailAddressesOutput";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Size = 8000;
                parameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parameter);

                // en secondes donc 5 minutes
                cmd.CommandTimeout = 300;

                con.Open();
                cmd.ExecuteNonQuery();
                EmailEnvDiligente = parameter.Value.ToString();
                con.Close();

                return EmailEnvDiligente;

            }
            catch (Exception e)
            {
                return e.InnerException.ToString();
            }

        }


        public string returnInfo(string nomEmp)
        {
            var info = "";
            var num = "";
            var email = "";
            var org = "";
            var infoFinal = "";
            try
            {
             info = _entities.EmployeeActif_Inactive.Where(p => p.FULL_NAME == nomEmp).Select(p => p.EMPLOYEE_NUMBER + "#" + p.EMAIL_ADDRESS + "#" + p.ORG).FirstOrDefault();// + " / " + p.EMPLOYEE_NUMBER);
             num = info.Split('#')[0];
             email = info.Split('#')[1];
             org = info.Split('#')[2].Split(' ')[1];
             infoFinal = num + "#" + email + "#" + org;

            }
            catch(Exception e)
            {
                infoFinal = "" + "#" + "" + "#" + "";
            }
            return infoFinal;

        }

        public List<string> GetGestionnairesNames715()
        {
            DateTime dt = DateTime.Now.Date;
            var listGest = _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= dt && p.EFFECTIVE_END_DATE >= dt && p.ORG.Contains("715")).Select(p => p.FULL_NAME);// + " / " + p.EMPLOYEE_NUMBER);
            return listGest.Distinct().ToList();

        }

        public List<string> returnTitreRH()
        {
            try
            {
                var listeTitreRH = _entitiesAdministration.ListeNominative_liste.Select(x => x.Titre_d_emploie_RH).Distinct().ToList();
                return listeTitreRH;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string insertIntoTable(string nomEmp, string numEmp, string email, string titreRH, string division, string comment, string type, string nom_app, string divMark, string autorisation)
        {
            try
            {
                switch (type)
                {
                    case "partielle":
                        TDT_Liste_de_taux_partielle model = new TDT_Liste_de_taux_partielle();
                        model.Commentaire = comment;
                        model.Division = division;
                        model.Adresse_courriel = email;
                        // model.Fait = item.Fait;
                        model.Nom_de_l_employé = nomEmp;
                        model.Numéro_employé_ = numEmp;
                        model.Titre = titreRH;
                        _entitiesSuiviMandat.TDT_Liste_de_taux_partielle.Add(model);
                        break;
                    case "moyenne":
                        TDT_Liste_de_taux_moyen model2 = new TDT_Liste_de_taux_moyen();
                        model2.Commentaire = comment;
                        model2.Division = division;
                        model2.Adresse_courriel = email;
                        // model.Fait = item.Fait;
                        model2.Nom_de_l_employé = nomEmp;
                        model2.Numéro_employé_ = numEmp;
                        model2.Titre = titreRH;
                        _entitiesSuiviMandat.TDT_Liste_de_taux_moyen.Add(model2);
                        // model.Retrait = item.Retrait;

                        break;
                    case "complete":
                        TDT_Liste_de_taux_complet model3 = new TDT_Liste_de_taux_complet();
                        model3.Commentaire = comment;
                        model3.Division = division;
                        model3.Adresse_courriel = email;
                        // model.Fait = item.Fait;
                        model3.Nom_de_l_employé = nomEmp;
                        model3.Numéro_employé_ = numEmp;
                        model3.Titre = titreRH;
                        _entitiesSuiviMandat.TDT_Liste_de_taux_complet.Add(model3);

                        break;

                    case "approbateurs":
                        var listeApprobateurs = _entitiesSuiviMandat.TDT_Approbateurs.ToList();
                        TDT_Approbateurs model4 = new TDT_Approbateurs();
                        model4.Nom = nom_app;
                        model4.Division_de_marché = divMark;
                        model4.autorisation = autorisation;
                        _entitiesSuiviMandat.TDT_Approbateurs.Add(model4);

                        break;

                }
                _entitiesSuiviMandat.SaveChanges();

                return "success";

            }
            catch (Exception e)
            {
                return "failed";
            }
        }

        public string removeFromTable(string numEmp, string type)
        {
            try
            {
                switch (type)
                {
                    case "partielle":
                        var itemtDeleteP = _entitiesSuiviMandat.TDT_Liste_de_taux_partielle.Where(p => p.Numéro_employé_ == numEmp).FirstOrDefault();
                        _entitiesSuiviMandat.TDT_Liste_de_taux_partielle.Remove(itemtDeleteP);
                        break;
                    case "moyenne":
                        var itemtDeleteM = _entitiesSuiviMandat.TDT_Liste_de_taux_moyen.Where(p => p.Numéro_employé_ == numEmp).FirstOrDefault();
                        _entitiesSuiviMandat.TDT_Liste_de_taux_moyen.Remove(itemtDeleteM);
                        // model.Retrait = item.Retrait;

                        break;
                    case "complete":
                        var itemtDeleteC = _entitiesSuiviMandat.TDT_Liste_de_taux_complet.Where(p => p.Numéro_employé_ == numEmp).FirstOrDefault();
                        _entitiesSuiviMandat.TDT_Liste_de_taux_complet.Remove(itemtDeleteC);

                        break;

                    case "approbateurs":
                        var nom = numEmp.Split('_')[0];
                        var divMark = numEmp.Split('_')[1];

                        var itemtDeleteA = _entitiesSuiviMandat.TDT_Approbateurs.Where(p => p.Nom == nom && p.Division_de_marché == divMark).FirstOrDefault();
                        _entitiesSuiviMandat.TDT_Approbateurs.Remove(itemtDeleteA);

                        break;

                }
                _entitiesSuiviMandat.SaveChanges();
                return "success";
            }
            catch (Exception e)
            {
                return "failed";
            }


        }

        public bool GetSecuritySatisfactionClient(string EmployeeNumber)
        {
            try
            {
                var UserSatisfactionClient = _entities.TTProjetSecurities.Where(p => p.Employee_Number == EmployeeNumber && p.Security_Desc == "Satisfaction Client").FirstOrDefault();

                if (UserSatisfactionClient != null)
                {
                    return true;
                }
                else
                {                    

                    return false;

                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string UpdateBDWithExcelSatisfactionClient()//DateTime ProjectStartDate, DateTime ProjectEndDate
        {
            int COUNT = 1;

            try
            {
                var filename = UserService.FichierQIBSatisfactionClient();
                try
                {
                    //on vide la table temp; on vide la table satisfaction; on execute le rapport de satisfaction;  on rempli la table temp
              var troc  =  _entities.usp_SatisfactionClient(null, null);

                }
                catch (Exception e)
                {
                    var truc = 2;
                }
                /*Exécuter la procédure du rapport satisfaction client*/

                /*supprimer les projetes de TQE*/
                _entities.usp_Clean_SatisfactionClient();

                /*Exécuter la procédure pour vider la table du fichier Excel*/
                _entities.usp_Clean_tbl_Excel_SatisfactionClient();

                var itemsToAdd = new List<tbl_Excel_SatisfactionClient>();
                
                using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                                          
                        DataTableCollection tables = result.Tables;

                        // Copie indépendante des données pour libérer les ressources associées au fichier Excel
                        DataTable resultTable = tables["SSS"].Copy();

                        // Libérez les ressources associées au DataSet immédiatement
                        result.Dispose();

                        foreach (DataRow row in resultTable.Rows)
                        {                            
                            string projectNumber = row[0].ToString(); // colonne : Numéro de projet 
                            string datestring = row[19].ToString(); //colonne : Date - Info reçue

                            DateTime? dateInfoRecu = null;

                            // Convertir en DateTime et vérifier la plage pour smalldatetime
                            if (DateTime.TryParse(datestring, out DateTime parsedDate))
                            {
                                // Utiliser uniquement la date et vérifier la plage pour smalldatetime
                                if (parsedDate >= new DateTime(1900, 1, 1) && parsedDate <= new DateTime(2079, 6, 6))
                                {
                                    dateInfoRecu = parsedDate.Date;
                                }
                                else
                                {                                    
                                    dateInfoRecu = null;
                                }
                            }
                            else
                            {                                
                                dateInfoRecu = null;
                            }

                            var newItem = new tbl_Excel_SatisfactionClient
                            {
                                TLinxProjectNumber = projectNumber,
                                dateInfoRecu = dateInfoRecu
                            };

                            itemsToAdd.Add(newItem);
                            COUNT++;

                        }

                    }
                }

                // Ajouter les éléments à la base de données et sauvegarder les changements
                _entities.tbl_Excel_SatisfactionClient.AddRange(itemsToAdd);
                _entities.SaveChanges();
                                               
                /*on enléve de satisfactionClient les projets qui ne sont pas dans la table tbl_excel. (mise à jour de la table satisfactionclient avec les données de la table Excel)
                 met a jour le champ dateInfoRecu et supprime ce champ s il n est pas null c.a.d si le champ a une date, le projet est supprimé*/
                _entities.usp_Update_SatisfactionClient();
                
                return "success";
            }
                       
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string UpdateBDWithExcelSatisfactionClient_TQE()// DateTime ProjectStartDate, DateTime ProjectEndDate
        {
            int COUNT = 1;

            try
            {
                var filename = UserService.FichierTQESatisfactionClient();

                /*Exécuter la procédure du rapport satisfaction client*/
                _entities.usp_SatisfactionClient_TQE(null, null);//ProjectStartDate, ProjectEndDate

                /*supprimer les projetes de TQE*/
                _entities.usp_Clean_SatisfactionClient_TQE();

                /*Exécuter la procédure pour vider la table du fichier Excel*/
                _entities.usp_Clean_tbl_Excel_SatisfactionClient_TQE();

                var itemsToAdd = new List<tbl_Excel_SatisfactionClient_TQE>();

                using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        DataTableCollection tables = result.Tables;

                        // Copie indépendante des données pour libérer les ressources associées au fichier Excel
                        DataTable resultTable = tables["SSS"].Copy();

                        // Libérez les ressources associées au DataSet immédiatement
                        result.Dispose();

                        foreach (DataRow row in resultTable.Rows)
                        {
                            string projectNumber = row[0].ToString(); // colonne : Numéro de projet 
                            string datestring = row[20].ToString(); //colonne : Date - Info reçue

                            DateTime? dateInfoRecu = null;

                            // Convertir en DateTime et vérifier la plage pour smalldatetime
                            if (DateTime.TryParse(datestring, out DateTime parsedDate))
                            {
                                // Utiliser uniquement la date et vérifier la plage pour smalldatetime
                                if (parsedDate >= new DateTime(1900, 1, 1) && parsedDate <= new DateTime(2079, 6, 6))
                                {
                                    dateInfoRecu = parsedDate.Date;
                                }
                                else
                                {
                                    dateInfoRecu = null;
                                }
                            }
                            else
                            {
                                dateInfoRecu = null;
                            }

                            var newItem = new tbl_Excel_SatisfactionClient_TQE
                            {
                                TLinxProjectNumber = projectNumber,
                                dateInfoRecu = dateInfoRecu
                            };

                            itemsToAdd.Add(newItem);
                            COUNT++;

                        }

                    }
                }

                // Ajouter les éléments à la base de données et sauvegarder les changements
                _entities.tbl_Excel_SatisfactionClient_TQE.AddRange(itemsToAdd);
                _entities.SaveChanges();

                /*mise à jour de la table satisfactionclient avec les données de la table Excel**/
                _entities.usp_Update_SatisfactionClient_TQE();

                return "success";
            }

            catch (Exception e)
            {
                return e.Message;
            }

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void SendCourrielSatisfactionClient( string TypeEnvoi)//DateTime ProjectStartDate, DateTime ProjectEndDate,
        {
            var CourrielTestSatisfactionClient = UserService.CourrielTestSatisfactionClient();
            var CourrielQIBSatisfactionClientFROM = UserService.CourrielQIBSatisfactionClientFROM();
            var CourrielQIBSatisfactionClientCC = UserService.CourrielQIBSatisfactionClientCC();
            var emailErrors = new List<string>();
            var count = 0;

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            //générer  Tableau avec la colonne Numéro de projet et Chargé de projet
            // ajouter le tableau dans le courriel: Satisfaction client QIB – Résultat d'envoi
            List<string> listeRecapitulative = new List<string>();
         

            try
            {
                var Project_Mgr_SatisfactionClientList = _entities.usp_get_FULL_NAME_Project_Mgr_SatisfactionClient().ToList();

                foreach (var itemProjectManger in Project_Mgr_SatisfactionClientList)
                {
                    
                    var ListProjectMgr_SatisfactionClient = _entities.usp_getListProjectSatisfactionClient(itemProjectManger.FULL_NAME_Project_Mgr).ToList();

                    if (ListProjectMgr_SatisfactionClient.Count != 0)
                    {
                        MailMessage NewEmail = new MailMessage();

                        var email = "";
                        email += "<html><head>";

                        email += "</br>";

                        email += @"<div>Bonjour " + itemProjectManger.FULL_NAME_Project_Mgr + "</div>";
                        email += "</br>";
                        email += @"<div>Voici la liste des projets pour lesquels vous êtes identifié comme gestionnaire, et pour lesquels un sondage de satisfaction sera envoyé <span style='color: red;'> dans deux semaines</span> :</div>";
                        email += "</br>";
                        email += "<div>";
                        email += "<table style='border-style: solid;width: 100%;'";
                        email += "<thead>";
                        email += "<tr  style ='font-family: arial; font-size:13px; background-color: #003478;'>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Numéro de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Description de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> CP Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Courriel Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Commentaires</th>";
                        email += "</tr>";

                        foreach (var line in ListProjectMgr_SatisfactionClient)
                        {
                            listeRecapitulative.Add(itemProjectManger.FULL_NAME_Project_Mgr + "#" + line.Project_number);

                            email += "<tr>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "" : line.Project_number)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "" : line.Project_Description)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "" : line.Customer_Name)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_ProjectManager)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_Email_ProjectManager)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + line.commentaire + "</td>";
                            email += "</tr>";

                        }

                        email += "</tbody>";
                        email += "</table>";
                        email += "</div>";
                        email += "<br>";

                        email += "<div><span style='color: red;'>Merci de valider les adresses courriel ou de les compléter au besoin (cases jaunes).</span></div>";
                        email += "<br>";
                        email += "<div>Si vous avez des commentaires merci de nous en faire part en répondant à ce courriel.</div>";
                        email += "</br>";
                        email += "<div>Bonne journée,</div>";
                        email += "</br>";
                        email += "<div>Votre équipe Qualité</div>";
                        email += "</body>";

                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            if (!string.IsNullOrEmpty(itemProjectManger.EMAIL_ADDRESS_Project_Mgr) && IsValidEmail(itemProjectManger.EMAIL_ADDRESS_Project_Mgr))
                            {

                                NewEmail.To.Add(CourrielTestSatisfactionClient);
                            }
                            else
                            {
                                emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " : Courriel :  " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                continue; // Passer à l'email suivant

                            }
                        }
                        else
                        {
                            
                            if (!string.IsNullOrEmpty(itemProjectManger.EMAIL_ADDRESS_Project_Mgr) && IsValidEmail(itemProjectManger.EMAIL_ADDRESS_Project_Mgr))
                            {
                                NewEmail.To.Add(itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                               NewEmail.CC.Add(CourrielQIBSatisfactionClientCC);

                            }
                            else
                            {                                
                                emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " : Courriel :  " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                continue; // Passer à l'email suivant
                            }
                        }
                        

                        NewEmail.From = new MailAddress(CourrielQIBSatisfactionClientFROM);
                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            NewEmail.Subject = "TEST - INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }
                        else
                        {
                            NewEmail.Subject = "INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }

                        NewEmail.Body = new MessageBody(Microsoft.Exchange.WebServices.Data.BodyType.HTML, email);
                        NewEmail.IsBodyHtml = true;

                        ServicePointManager.ServerCertificateValidationCallback =
                                    delegate (
                                        object s,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors
                                    ) {
                                        return true;
                                    };

                        try
                        {   
                            SmtpServer.Send(NewEmail);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            // Capturer l'erreur et l'ajouter à la liste
                            emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);                            
                        }
                        finally
                        {
                            // Libérer les ressources de l'email
                            NewEmail.Dispose();
                        }
                    }
                }
                SmtpServer.Dispose();

                sendConfirmationEnvoiCourrielSatisfactionClient(emailErrors, count, Project_Mgr_SatisfactionClientList.Count, listeRecapitulative);

            }

            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public void SendCourrielSatisfactionClient_2(string TypeEnvoi,List<string> listeProjetQIB, List<string> listeProjetQIB_CP, List<string>  listeProjetQIB_CP_email)//DateTime ProjectStartDate, DateTime ProjectEndDate,
        {
            var CourrielTestSatisfactionClient = UserService.CourrielTestSatisfactionClient();
            var CourrielQIBSatisfactionClientFROM = UserService.CourrielQIBSatisfactionClientFROM();
            var CourrielQIBSatisfactionClientCC = UserService.CourrielQIBSatisfactionClientCC();
            var emailErrors = new List<string>();
            var count = 0;

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            //générer  Tableau avec la colonne Numéro de projet et Chargé de projet
            // ajouter le tableau dans le courriel: Satisfaction client QIB – Résultat d'envoi
            List<string> listeRecapitulative = new List<string>();


            try
            {
                var Project_Mgr_SatisfactionClientList = _entities.usp_get_FULL_NAME_Project_Mgr_SatisfactionClient().ToList();

                foreach (var itemProjectManger in Project_Mgr_SatisfactionClientList)
                {

                    var ListProjectMgr_SatisfactionClient = _entities.usp_getListProjectSatisfactionClient(itemProjectManger.FULL_NAME_Project_Mgr).ToList();

                    if (ListProjectMgr_SatisfactionClient.Count != 0)
                    {
                        MailMessage NewEmail = new MailMessage();

                        var email = "";
                        email += "<html><head>";

                        email += "</br>";

                        email += @"<div>Bonjour " + itemProjectManger.FULL_NAME_Project_Mgr + "</div>";
                        email += "</br>";
                        email += @"<div>Voici la liste des projets pour lesquels vous êtes identifié comme gestionnaire, et pour lesquels un sondage de satisfaction sera envoyé <span style='color: red;'> dans deux semaines</span> :</div>";
                        email += "</br>";
                        email += "<div>";
                        email += "<table style='border-style: solid;width: 100%;'";
                        email += "<thead>";
                        email += "<tr  style ='font-family: arial; font-size:13px; background-color: #003478;'>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Numéro de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Description de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> CP Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Courriel Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Commentaires</th>";
                        email += "</tr>";

                        foreach (var line in ListProjectMgr_SatisfactionClient)
                        {
                            listeRecapitulative.Add(itemProjectManger.FULL_NAME_Project_Mgr + "#" + line.Project_number);

                            email += "<tr>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "" : line.Project_number)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "" : line.Project_Description)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "" : line.Customer_Name)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_ProjectManager)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_Email_ProjectManager)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + line.commentaire + "</td>";
                            email += "</tr>";

                        }

                        email += "</tbody>";
                        email += "</table>";
                        email += "</div>";
                        email += "<br>";

                        email += "<div><span style='color: red;'>Merci de valider les adresses courriel ou de les compléter au besoin (cases jaunes).</span></div>";
                        email += "<br>";
                        email += "<div>Si vous avez des commentaires merci de nous en faire part en répondant à ce courriel.</div>";
                        email += "</br>";
                        email += "<div>Bonne journée,</div>";
                        email += "</br>";
                        email += "<div>Votre équipe Qualité</div>";
                        email += "</body>";

                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            if (!string.IsNullOrEmpty(itemProjectManger.EMAIL_ADDRESS_Project_Mgr) && IsValidEmail(itemProjectManger.EMAIL_ADDRESS_Project_Mgr))
                            {

                                NewEmail.To.Add(CourrielTestSatisfactionClient);
                            }
                            else
                            {
                                emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " : Courriel :  " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                continue; // Passer à l'email suivant

                            }
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(itemProjectManger.EMAIL_ADDRESS_Project_Mgr) && IsValidEmail(itemProjectManger.EMAIL_ADDRESS_Project_Mgr))
                            {
                                NewEmail.To.Add(itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                NewEmail.CC.Add(CourrielQIBSatisfactionClientCC);

                            }
                            else
                            {
                                emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " : Courriel :  " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                continue; // Passer à l'email suivant
                            }
                        }


                        NewEmail.From = new MailAddress(CourrielQIBSatisfactionClientFROM);
                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            NewEmail.Subject = "TEST - INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }
                        else
                        {
                            NewEmail.Subject = "INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }

                        NewEmail.Body = new MessageBody(Microsoft.Exchange.WebServices.Data.BodyType.HTML, email);
                        NewEmail.IsBodyHtml = true;

                        ServicePointManager.ServerCertificateValidationCallback =
                                    delegate (
                                        object s,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors
                                    ) {
                                        return true;
                                    };

                        try
                        {
                            SmtpServer.Send(NewEmail);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            // Capturer l'erreur et l'ajouter à la liste
                            emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                        }
                        finally
                        {
                            // Libérer les ressources de l'email
                            NewEmail.Dispose();
                        }
                    }
                }
                SmtpServer.Dispose();

                sendConfirmationEnvoiCourrielSatisfactionClient(emailErrors, count, Project_Mgr_SatisfactionClientList.Count, listeRecapitulative);

            }

            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public void SendCourrielSatisfactionClient_TQE( string TypeEnvoi)//DateTime ProjectStartDate, DateTime ProjectEndDate,
        {
            var CourrielTestSatisfactionClientTQE = UserService.CourrielTestSatisfactionClientTQE();
            var CourrielTQESatisfactionClientFROM = UserService.CourrielTQESatisfactionClientFROM();
            var CourrielTQESatisfactionClientCC = UserService.CourrielTQESatisfactionClientCC();
            var emailErrors = new List<string>();
            var count = 0;
            List<string> listeRecapitulative = new List<string>();
            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {               

                var Project_Mgr_SatisfactionClientList = _entities.usp_get_FULL_NAME_Project_Mgr_SatisfactionClient_TQE().ToList();

                foreach (var itemProjectManger in Project_Mgr_SatisfactionClientList)
                {

                    var ListProjectMgr_SatisfactionClient = _entities.usp_getListProjectSatisfactionClient_TQE(itemProjectManger.FULL_NAME_Project_Mgr).ToList();

                    if (ListProjectMgr_SatisfactionClient.Count != 0)
                    {
                        MailMessage NewEmail = new MailMessage();

                        var email = "";
                        email += "<html><head>";

                        email += "</br>";

                        email += @"<div>Bonjour " + itemProjectManger.FULL_NAME_Project_Mgr + "</div>";
                        email += "</br>";
                        email += @"<div>Voici la liste des projets pour lesquels vous êtes identifié comme gestionnaire, et pour lesquels un sondage de satisfaction sera envoyé <span style='color: red;'> dans deux semaines</span> :</div>";
                        email += "</br>";
                        email += "<div>";
                        email += "<table style='border-style: solid;width: 100%;'";
                        email += "<thead>";
                        email += "<tr  style ='font-family: arial; font-size:13px; background-color: #003478;'>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Numéro de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Description de projet</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> CP Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Courriel Client</th>";
                        email += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Commentaires</th>";
                        email += "</tr>";

                        foreach (var line in ListProjectMgr_SatisfactionClient)
                        {
                            listeRecapitulative.Add(itemProjectManger.FULL_NAME_Project_Mgr + "#" + line.Project_number);

                            email += "<tr>";                            
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_number == null || line.Project_number.Trim().ToLower() == "null" ? "" : line.Project_number)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Project_Description == null || line.Project_Description.Trim().ToLower() == "null" ? "" : line.Project_Description)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Customer_Name == null || line.Customer_Name.Trim().ToLower() == "null" ? "" : line.Customer_Name)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_ProjectManager == null || line.Client_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_ProjectManager)
                                       + "</td>";                            
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; background-color: "
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "yellow;" : "white;") + "'>"
                                       + (line.Client_Email_ProjectManager == null || line.Client_Email_ProjectManager.Trim().ToLower() == "null" ? "" : line.Client_Email_ProjectManager)
                                       + "</td>";
                            email += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + line.commentaire + "</td>";
                            email += "</tr>";

                        }

                        email += "</tbody>";
                        email += "</table>";
                        email += "</div>";
                        email += "<br>";

                        email += "<div><span style='color: red;'>Merci de valider les adresses courriel ou de les compléter au besoin (cases jaunes).</span></div>";
                        email += "<br>";
                        email += "<div>Si vous avez des commentaires merci de nous en faire part en répondant à ce courriel.</div>";
                        email += "</br>";
                        email += "<div>Bonne journée,</div>";
                        email += "</br>";
                        email += "<div>Votre équipe Qualité</div>";
                        email += "</body>";

                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            NewEmail.To.Add(CourrielTestSatisfactionClientTQE);
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(itemProjectManger.EMAIL_ADDRESS_Project_Mgr) && IsValidEmail(itemProjectManger.EMAIL_ADDRESS_Project_Mgr))
                            {
                                NewEmail.To.Add(itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                NewEmail.CC.Add(CourrielTQESatisfactionClientCC);
                                
                            }
                            else
                            {
                                emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " : Courriel :  " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                                continue; // Passer à l'email suivant
                            }
                        }


                        NewEmail.From = new MailAddress(CourrielTQESatisfactionClientFROM);
                        if (TypeEnvoi == "Test" || TypeEnvoi == "")
                        {
                            NewEmail.Subject = "TEST - INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }
                        else
                        {
                            NewEmail.Subject = "INFORMATION - Évaluation de la satisfaction client - " + itemProjectManger.FULL_NAME_Project_Mgr;
                        }

                                                
                        NewEmail.Body = new MessageBody(Microsoft.Exchange.WebServices.Data.BodyType.HTML, email);
                        NewEmail.IsBodyHtml = true;

                        ServicePointManager.ServerCertificateValidationCallback =
                                    delegate (
                                        object s,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors
                                    ) {
                                        return true;
                                    };

                        try
                        {
                            SmtpServer.Send(NewEmail);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            // Capturer l'erreur et l'ajouter à la liste
                            emailErrors.Add(itemProjectManger.FULL_NAME_Project_Mgr + " " + itemProjectManger.EMAIL_ADDRESS_Project_Mgr);
                        }
                        finally
                        {
                            // Libérer les ressources de l'email
                            NewEmail.Dispose();
                        }



                    }
                }
                SmtpServer.Dispose();

                sendConfirmationEnvoiCourrielSatisfactionClient_TQE(emailErrors, count, Project_Mgr_SatisfactionClientList.Count, listeRecapitulative);

            }

            catch (Exception e)
            {
                var test = e.Message;
            }

        }

        public void SendCourrielSatisfactionClient_phase2(string TypeEnvoi, string unit,  List<string>  listeProjetTQE, List<string>  listeProjetTQE_titre, List<string> listeProjetTQE_CP, List<string>  listeProjetTQE_DP, List<string>  listeProjetTQE_CP_email, List<string> listeProjetTQE_client_email)//DateTime ProjectStartDate, DateTime ProjectEndDate,
        {                 
            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);

            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            for (int i = 0; i<listeProjetTQE.Count; i++)
                {
            
                 MailMessage NewEmail = new MailMessage();
                     try
                {
          

                        var email = "";
                        email += "<html><head>";
                        email += "</br>";
                        email += "<div>(English message follows)</div>";
                        email += "</br>";
                        email += "<div>Évaluation de la qualité des services - " + listeProjetTQE_titre[i] + "</div>";
                        email += "</br>";
                        email += "<div>Bonjour,</div>";
                        email += "</br>";
                var name = listeProjetTQE_CP[i].Split('(')[0].Split(',')[1] + " " + listeProjetTQE_CP[i].Split('(')[0].Split(',')[0];
                        email += "<div>Afin de constamment améliorer nos services, nous sollicitons quelques minutes de votre temps pour connaître votre appréciation de Tetra Tech et des services rendus pour le projet "+ "\"" + listeProjetTQE_titre [i] + "\"" + "</div>";
                        email += "<div>Ce mandat, sous la charge de "+ name + ", est prêt à être évalué et nous désirons avoir votre opinion sur le déroulement du projet et sur la qualité des livrables.</div>";
                        email += "<div>Le court sondage en ligne comprend 14 questions à choix multiples: https://fr.surveymonkey.com/s/NWPNJMC?p=" + listeProjetTQE [i]+ "</div>";
                        email += "<div>L’information fournie servira à orienter nos efforts afin d’améliorer les points spécifiques que vous aurez identifiés et ainsi mieux vous servir lors de prochaines collaborations.</div>";
                        email += "<div>N’hésitez pas à communiquer avec nous pour toutes questions ou commentaires.</div>";
                        email += "<div>Au plaisir,</div>";
                        email += "</br>";
                        email += "<div>";
                if (unit == "TQE")
                {
                    email += "<p style='padding : 0; margin : 0;' >Jean-Philippe Veillette, ing. </p>";
                    email += "<p style='padding : 0; margin : 0;'>Directeur, Qualité</p>";
                    email += "<p style='padding : 0; margin : 0;'>514-706-9019</p>";
                }
                else
                {
                    email += "<p style='padding : 0; margin : 0;' >Jean-Philippe Coin</p>";
                    email += "<p style='padding : 0; margin : 0;'>Coord. assurance qualite/Qlty Insur. Coord.</p>";
                    email += "<p style='padding : 0; margin : 0;'>438-469-2444</p>";
                }
                email += "</div>";
                        email += "</br>";
                        email += "----------------------------------------------------------------------------";
                        email += "</br>";
                        email += "<div>Services Quality Evaluation - " +listeProjetTQE_titre[i] + "</div>";
                        email += "</br>";
                        email += "<div>Greetings, </div>";
                        email += "</br>";
                        email += "<div>In order to constantly improve our services, we are asking for a few minutes of your time to know your appreciation of Tetra Tech and the services received for the  project " + "\"" + listeProjetTQE_titre[i] + "\"" + "</div>";
                        email += "<div>This mandate under the supervision of " + name + " is ready to be evaluated and we would like your opinion regarding the project's progression and the quality of our deliverables.  </div>";
                        email += "<div>The short online survey contains 14 multiple choice answer questions: https://fr.surveymonkey.com/s/NWPNJMC?p=" + listeProjetTQE[i] + "</div>";
                        email += "<div>The information will allow us to focus our efforts towards improving the identified deficiencies in order to serve you better in future collaborations.</div>";
                        email += "<div>Do not hesitate to communicate with us should you have any questions or comments.</div>";
                        email += "<div>Best regards,</div>";
                        email += "</br>";
                        email += "<div>";
                        if (unit == "TQE")
                        {
                            email += "<p style='padding : 0; margin : 0;' >Jean-Philippe Veillette, ing. </p>";
                            email += "<p style='padding : 0; margin : 0;'>Directeur, Qualité</p>";
                            email += "<p style='padding : 0; margin : 0;'>514-706-9019</p>";
                        }
                        else
                        {
                            email += "<p style='padding : 0; margin : 0;' >Jean-Philippe Coin</p>";
                            email += "<p style='padding : 0; margin : 0;'>Coord. assurance qualite/Qlty Insur. Coord.</p>";
                            email += "<p style='padding : 0; margin : 0;'>438-469-2444</p>";
                        } 
                        email += "</div>";
                        email += "</body>";


                    // SmtpServer.EnableSsl = true;
                    ///////////////////////////////

                  

                    if (TypeEnvoi == "Test" || TypeEnvoi == "")
                    {
                        NewEmail.To.Add("rapports@tetratech.com");
                        NewEmail.From = new MailAddress("rapports@tetratech.com");
                    }
                    else
                    {
                        NewEmail.To.Add("rapports@tetratech.com");
                        NewEmail.To.Add(listeProjetTQE_client_email[1]);
                        if (unit == "TQE")
                        {
                            NewEmail.From = new MailAddress("j.veillette@tetratech.com");

                        }
                        else
                        {
                            NewEmail.From = new MailAddress("jeanphilippe.coin@tetratech.com");

                        }

                    }

                    NewEmail.From = new MailAddress("rapports@tetratech.com");//jean philippe veillette pour prod

                    NewEmail.Subject = listeProjetTQE_titre[i];
                    NewEmail.IsBodyHtml = true;
                    NewEmail.Body = new MessageBody(BodyType.HTML, email);

                    ServicePointManager.ServerCertificateValidationCallback =
                                        delegate (
                                            object s,
                                            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                            System.Security.Cryptography.X509Certificates.X509Chain chain,
                                            System.Net.Security.SslPolicyErrors sslPolicyErrors
                                        ) {
                                            return true;
                                        };

                    SmtpServer.Send(NewEmail);
                    NewEmail.Dispose();
                   
                    //File.AppendAllText(@"C:\TTRapport\TableDeTaux\TdTLog.txt",DateTime.Now + @" - Courriel '"+ titre +"' : succés" + Environment.NewLine);
                    }
                        catch (Exception e)
                        {
                            continue;
                        }

                
                 }
            SmtpServer.Dispose();
            }


        public void sendConfirmationEnvoiCourrielSatisfactionClient(List<string> emailErrors, int count, int countTotal, List<string> listeRecapitulative)
        {
            String emailMessage = "";
            var CourrielQIBConfirmationSatisfactionClientFROM = UserService.CourrielQIBConfirmationSatisfactionClientFROM();
            var CourrielQIBConfirmationSatisfactionClientTo = UserService.CourrielQIBConfirmationSatisfactionClientTo();            
            var CourrielQIBConfirmationSatisfactionClientCC = UserService.CourrielQIBConfirmationSatisfactionClientCC();

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {

                MailMessage NewEmail = new MailMessage();
                emailMessage += "<html><head>";
                emailMessage += "</head><body>";
                emailMessage += "</br>";
                if (emailErrors.Count == 0)
                {
                    emailMessage += count + " courriels envoyés avec succès sur " + countTotal + ".</P>";
                   
                }
                else
                {
                    emailMessage += "<p>L'envoi des courriels a rencontré des problémes pour les managers suivants:</p>";

                    foreach (var error in emailErrors)
                    {                        
                        emailMessage += "<div>" + error + "</div>";                                                
                    }


                    
                }

                emailMessage += "<div>";
                emailMessage += "<table style='border-style: solid;width: 100%;'";
                emailMessage += "<thead>";
                emailMessage += "<tr  style ='font-family: arial; font-size:13px; background-color: #003478;'>";
                emailMessage += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Numéro de projet</th>";
                emailMessage += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Chargé de projet</th>";
                emailMessage += "</tr>";

                foreach (string item  in listeRecapitulative)
                {
                    emailMessage += "<tr>";
                    emailMessage += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; ' >"
                               + item.Split('#')[1]
                               + "</td>";
                    emailMessage += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; ' > "
                               + item.Split('#')[0]  
                               + "</td>";
                    emailMessage += "</tr>";

                }

                emailMessage += "</tbody>";
                emailMessage += "</table>";
                emailMessage += "</div>";

                emailMessage += "</br>";
                emailMessage += "</body></html>";

                NewEmail.From = new MailAddress(CourrielQIBConfirmationSatisfactionClientFROM);
                //decommenter
                //NewEmail.To.Add(CourrielQIBConfirmationSatisfactionClientTo);
                //NewEmail.CC.Add(CourrielQIBConfirmationSatisfactionClientCC);
                NewEmail.To.Add("Rapports@tetratech.com");
                NewEmail.To.Add("marc-andre.menier@tetratech.com");
                NewEmail.Subject = "Satisfaction client QIB – Résultat d'envoi";
                NewEmail.IsBodyHtml = true;
                NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);

                ServicePointManager.ServerCertificateValidationCallback =
                                   delegate (
                                       object s,
                                       System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                       System.Security.Cryptography.X509Certificates.X509Chain chain,
                                       System.Net.Security.SslPolicyErrors sslPolicyErrors
                                   ) {
                                       return true;
                                   };
                SmtpServer.Send(NewEmail);
                emailMessage = "";
            }

            catch (Exception e)
            {
                var test = e.Message;
            }


        }

        public void sendConfirmationEnvoiCourrielSatisfactionClient_TQE(List<string> emailErrors, int count, int countTotal, List<string> listeRecapitulative)
        {
            String emailMessage = "";
            var CourrielTQEConfirmationSatisfactionClientFROM = UserService.CourrielTQEConfirmationSatisfactionClientFROM();
            var CourrielTQEConfirmationSatisfactionClientTo = UserService.CourrielTQEConfirmationSatisfactionClientTo();
            var CourrielTQEConfirmationSatisfactionClientCC = UserService.CourrielTQEConfirmationSatisfactionClientCC();

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {

                MailMessage NewEmail = new MailMessage();
                emailMessage += "<html><head>";
                emailMessage += "</head><body>";
                emailMessage += "</br>";
                if (emailErrors.Count == 0)
                {
                    emailMessage += count + " courriels envoyés avec succès sur " + countTotal + ".</P>";

                }
                else
                {
                    emailMessage += "<p>L'envoi des courriels a rencontré des problémes pour les managers suivants:</p>";

                    foreach (var error in emailErrors)
                    {
                        emailMessage += "<div>" + error + "</div>";
                    }



                }

                emailMessage += "<div>";
                emailMessage += "<table style='border-style: solid;width: 100%;'";
                emailMessage += "<thead>";
                emailMessage += "<tr  style ='font-family: arial; font-size:13px; background-color: #003478;'>";
                emailMessage += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Numéro de projet</th>";
                emailMessage += "<th style='border: 1px solid black; border-collapse: collapse;text-align: center; padding: 4px;'> Chargé de projet</th>";
                emailMessage += "</tr>";

                foreach (string item in listeRecapitulative)
                {
                    emailMessage += "<tr>";
                    emailMessage += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; ' >"
                               + item.Split('#')[1]
                               + "</td>";
                    emailMessage += "<td style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: center; padding: 4px; ' > "
                               + item.Split('#')[0]
                               + "</td>";
                    emailMessage += "</tr>";

                }

                emailMessage += "</tbody>";
                emailMessage += "</table>";
                emailMessage += "</div>";

                emailMessage += "</br>";
                emailMessage += "</body></html>";

                NewEmail.From = new MailAddress(CourrielTQEConfirmationSatisfactionClientFROM);
                //decommenter
                //NewEmail.To.Add(CourrielQIBConfirmationSatisfactionClientTo);
                //NewEmail.CC.Add(CourrielQIBConfirmationSatisfactionClientCC);
                NewEmail.To.Add("Rapports@tetratech.com");
                NewEmail.To.Add("marc-andre.menier@tetratech.com");
                NewEmail.To.Add("Rapports@tetratech.com");

                NewEmail.Subject = "Satisfaction client TQE – Résultat d'envoi";
                NewEmail.IsBodyHtml = true;
                NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);

                ServicePointManager.ServerCertificateValidationCallback =
                                   delegate (
                                       object s,
                                       System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                       System.Security.Cryptography.X509Certificates.X509Chain chain,
                                       System.Net.Security.SslPolicyErrors sslPolicyErrors
                                   ) {
                                       return true;
                                   };
                SmtpServer.Send(NewEmail);
                emailMessage = "";
            }

            catch (Exception e)
            {
                var test = e.Message;
            }
        }
    }
}
