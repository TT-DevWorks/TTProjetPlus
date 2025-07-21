using TetraTech.TTProjetPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class SecuriteService
    {
        private readonly HomeService _HomeService = new HomeService();
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
    private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
    private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public string insertIntoSecurityTable (string employee)
        {
                try
                {
                    var projectToUpdate = _entities.TTProjetSecurities
                   .Where(p => p.FULL_NAME == employee).FirstOrDefault();

                if (projectToUpdate != null)
                    {
                    return "existsUpdate";
                }
                else
                {
                    var now = DateTime.Now;
                    TTProjetSecurity itemToAdd = new TTProjetSecurity();
                    itemToAdd.Security_Role = "14";                    
                    itemToAdd.Employee_Number = _entities.EmployeeActif_Inactive.Where(p => p.FULL_NAME == employee && p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).Select(p => p.EMPLOYEE_NUMBER).FirstOrDefault();
                    itemToAdd.FULL_NAME = employee;
                    itemToAdd.Company = null;
                    itemToAdd.Security_Desc = "Responsable Projet";
                    _entities.TTProjetSecurities.Add(itemToAdd);

                    _entities.SaveChanges();

                    return "successUpdate";

                }

                }
                catch (Exception e)
                {
                _HomeService.addErrorLog(e, "SecuriteService", "insertIntoSecurityTable", "employee: " + employee);
                    return "failUpdate";
                }
                
            }

       public string removeResponsible (string id)
        {
            try
            {
                var employeeToRemove = _entities.TTProjetSecurities
               .Where(p => p.Employee_Number == id).FirstOrDefault();

                if (employeeToRemove == null)
                {
                    return "notExistRemove";
                }
                else
                {
                    _entities.TTProjetSecurities.Remove(employeeToRemove);
                    _entities.SaveChanges();

                    return "successRemove";

                }

            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "SecuriteService", "removeResponsible", "employeeId: " + id);
                return "failRemove";
            }

        }


       public List<string> GetEmployeesList()
        {            
            var now = DateTime.Now;
            var employeeList = _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).OrderBy(x => x.FULL_NAME).Select(p => p.FULL_NAME).ToList();
            return employeeList;
        }

    }
}