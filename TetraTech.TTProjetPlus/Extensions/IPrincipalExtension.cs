using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Controllers;
using TetraTech.TTProjetPlus.Data;
using System.Collections.Generic;

namespace TetraTech.TTProjetPlus.Extensions
{
    public static class IPrincipalExtension
    {
        private const string AdminsKey = "Admins";

        public static bool IsAdmin(this IPrincipal principal)
        {
            TTProjetPlusEntitiesNew _entitiesTTProjetPlus = new TTProjetPlusEntitiesNew();
            Boolean isAdmin = false;
            var result = _entitiesTTProjetPlus.usp_isEmployeeAdmin(principal.Identity.Name).FirstOrDefault();
            if (result == 1)
            {
                isAdmin = true;
            }

            return isAdmin;

            //SuiviMandatEntitiesNew2 _entities = new SuiviMandatEntitiesNew2();
            //var employees = _entities.PROJECT_CONTROL_SECURITY.Where(p => p.Security_Role == "ADMIN").Select(p => p.full_name).ToList();//  && p.Security_Role == "ADMIN").FirstOrDefault();
            //List<string> listEmployees = new List<string>();
            //foreach (string item in employees)
            //{

            //    if (item != null)
            //    {

            //        if (item.Contains(','))
            //        {
            //            var firstName = item.Split(' ')[1];
            //            var familyName = item.Split(' ')[0].Split(',')[0];
            //            var entryFinal = "TT\\" + firstName + "." + familyName;
            //            listEmployees.Add(entryFinal);
            //        }
            //        else
            //        {
            //            var newEntry = "TT\\" + item.Split(' ')[0] + "." + item.Split(' ')[1];
            //            listEmployees.Add(newEntry);
            //        }
            //    }
            //}
            //return listEmployees != null && listEmployees.Any(name => String.Compare(name.Trim(), principal.Identity.Name, true) == 0);

        }
    }
}