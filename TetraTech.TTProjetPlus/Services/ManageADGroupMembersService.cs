using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class ManageADGroupMembersService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        private readonly DirectoryServiceForAD _directoryServiceForAD = new DirectoryServiceForAD();


        public List<SelectListItem> GetSecurityDirectoryADGroupForAdmin(string ttuser)
        {
            return _entitiesBPR.usp_GetSecurityDirectoryADGroupForAdmin(ttuser)
                .Select(i => new SelectListItem { Text = i.ADGroupName, Value = i.ADGroup }).ToList();
        } 

        public ManageADGroupMembersDetails GetDetailsForSelectedGroup(string selectedGroup)
        {
             ManageADGroupMembersDetails result = new ManageADGroupMembersDetails { SelectedGroup = selectedGroup, ADGroupMembersList = new List<GroupMember>() };

            if (!string.IsNullOrEmpty(selectedGroup))
            {
                var groupmembers = _directoryServiceForAD.GetADGroupMembers(selectedGroup); 

                result.ADGroupMembersList = groupmembers
                    .Select(m=> new GroupMember { ADGroup = selectedGroup, Employee =new TetraTechUser(m) }).ToList();
                 
            } 
            return result;
        }

        public OperationStatus AddEmployeeToADGroup(string employeeid, string groupName)
        {
            return _directoryServiceForAD.AddEmployeeToADGroup(employeeid, groupName);
        }

        public OperationStatus RemoveEmployeeFromADGroup(string employeeid, string groupName)
        {
            return _directoryServiceForAD.RemoveEmployeeFromADGroup(employeeid, groupName);
        }
    }
     

    
}