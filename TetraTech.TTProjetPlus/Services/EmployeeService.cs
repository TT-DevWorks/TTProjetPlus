using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Controllers;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models; 
using System.Data;
 

namespace TetraTech.TTProjetPlus.Services
{
    public class EmployeeService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2(); 
          
        public List<EmployeeItem> GetEmployeesForSearch()
        {
            try
            {                
                var now = DateTime.Now;
                var query = from p in _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now)
                        select new EmployeeItem { Full_Name = p.FULL_NAME, Employee_Number = p.EMPLOYEE_NUMBER.ToString(), Org = p.ORG, Office = p.LOCATION_CODE, Email = p.EMAIL_ADDRESS };
                return query.Distinct().ToList(); 
            } 
            catch  
            {
                return null;
            }
        } 

        public Dictionary<string, EmployeeBase> GetEmployees(string[] employeesnumbersList)
        {
            Dictionary<string, EmployeeBase> employees = new Dictionary<string, EmployeeBase>();
            if (employeesnumbersList != null)
            {
                var now = DateTime.Now;
                employees = _entities.EmployeeActif_Inactive
                    .Where(e => e.EFFECTIVE_START_DATE <= now && e.EFFECTIVE_END_DATE >= now && employeesnumbersList.Contains(e.EMPLOYEE_NUMBER))
                    .Select(e =>
                    new EmployeeBase
                    {
                        Number = e.EMPLOYEE_NUMBER,
                        Name = e.FULL_NAME,
                        Email = e.EMAIL_ADDRESS
                    })
                    .ToDictionary(e => e.Number);
            }

            return employees;
        }

        public EmployeeBase GetContractAdministrator(string os_Project_Number)
        {
            os_Project_Number = "-" + os_Project_Number;
            DateTime now = DateTime.Now;
            var result = (from p in _entitiesSuiviMandat.XX_PROJECT //.Where(p => ).Take(1)
                          join km in _entitiesSuiviMandat.XX_PROJECT_KEYMEMBERS on p.Project_ID equals km.project_id
                          from e in _entitiesSuiviMandat.EMPLOYEEs.Where(e => km.employee_number == e.EMPLOYEE_NUMBER)
                             .DefaultIfEmpty()
                          where p.Project_Number.EndsWith(os_Project_Number) &&
                                   km.role == "Contract Administrator" &&
                                   km.start_date_active <= now && now >= km.end_date_active
                          orderby km.start_date_active
                          select
                             new EmployeeBase
                             {
                                 Number = km.employee_number,
                                 Name = e.FULL_NAME,
                                 Email = e.EMAIL_ADDRESS
                             }
                          ).FirstOrDefault();

            return result ?? new EmployeeBase();
        }

        
    } 
}
