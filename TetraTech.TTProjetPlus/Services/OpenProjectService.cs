using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class OpenProjectService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public List<string> GetOffersList(int status)
        {
            var list = _entities.OS_Project.Where(p => p.Status_ID == status).Select(p => p.Project_Number).ToList();
            return list;
        }

        public List<int?> returnVersionsList(string project_num)
        {
            var list = _entities.OpeningProjects.Where(p => p.Project_Number == project_num).Select(p => p.Version_Id).ToList();
            return list;
        }

        public List<structureItem> getListeEmployeeActif_Inactive()
        {
            var now = DateTime.Now;
            var list = _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (EmployeeActif_Inactive item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.LAST_NAME + " " + item.FIRST_NAME + " (" + item.ORG + ")";
                STmodel.Value = item.EMPLOYEE_NUMBER;
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }
        public List<structureItem> getCurrencies()
        {
            var list = _entities.Currencies.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Currency item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Currency_Name;
                STmodel.Value = item.Currency_ID.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getListeDistribution()
        {
            var list = _entities.Distribution_Rule.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Distribution_Rule item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Rule;
                STmodel.Value = item.dbo_Distribution_Rule_Id.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getContractType()
        {
            var list = _entities.Contract_Type.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Contract_Type item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Contract_Type_DescriptionFr; //pour la version francaise
                STmodel.Description = item.Contract_Type_DescriptionEn; //pour la version anglaise
                STmodel.Value = item.Contract_Type_ID.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getAcctCenters()
        {
            var list = _entities.Acct_Center.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Acct_Center item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Acct_Center1; //pour la version francaise
                STmodel.Value = item.Acct_Center_ID.ToString();
                if (!STmodel.Name.Contains("714 "))
                {
                    finalList.Add(STmodel);
                }

            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getOfficeAddress()
        {
            var list = _entities.Office_Address.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Office_Address item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Office_Address1; //pour la version francaise
                STmodel.Value = item.Office_Address_ID.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getWorkLocations()
        {
            var list = _entities.Work_Location.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Work_Location item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Work_Location1; //pour la version francaise
                STmodel.Value = item.Work_Location_Id.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getPrograms()
        {
            var list = _entities.Programs.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Program item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Program_Description; //pour la version francaise
                STmodel.Value = item.Program_ID.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public List<structureItem> getWorkType()
        {
            var list = _entities.Work_Type.ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (Work_Type item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.Work_Type_Description; //pour la version francaise
                STmodel.Value = item.Work_Type_ID.ToString();
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public string returnProjet(string customer_Number)
        {
            var project = _entities.OS_Project.Where(p => p.Customer_Number == customer_Number).Select(p => p.Customer_Number + ": " + p.Customer_Name).FirstOrDefault();
            return project;
        }

        public List<XX_PROJECT_TASKS_WBS> returnTasksList(string project_number)
        {
            var list = _entitiesSuiviMandat.XX_PROJECT_TASKS_WBS.Where(p => p.project_number == project_number).OrderBy(p=>p.task_number).ToList();
            return list;
        }
    }
}