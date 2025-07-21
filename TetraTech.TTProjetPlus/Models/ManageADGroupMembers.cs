using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Views.ManageADGroupMembers;

namespace TetraTech.TTProjetPlus.Models
{
    public class ManageADGroupMembersViewModel: BaseViewModel
    {
        [Display(Name = "ADGroupsList", ResourceType = typeof(ManageADRessources))]
        public List<SelectListItem> ADGroupsList { get; set; } 

        public ManageADGroupMembersDetails Details { get; set; }

        public ManageADGroupMembersViewModel(BaseViewModel _base) : base(_base) { }
    }

    public class ManageADGroupMembersDetails
    {
        [Display(Name = "GroupMembers", ResourceType = typeof(ManageADRessources))]
        [Required]
        public string SelectedGroup { get; set; } = "";

        public List<GroupMember> ADGroupMembersList { get; set; } = new List<GroupMember>();
         
    }

    public class EmployeeADGroup
    {
        [Display(Name = "GroupMembers", ResourceType = typeof(ManageADRessources))]
        [Required]
        public string SelectedGroup { get; set; } 


        [Display(Name = "Employee", ResourceType = typeof(ManageADRessources))]
        [Required(ErrorMessageResourceType = typeof(ManageADRessources), ErrorMessageResourceName = "MessageSelectEmployee")]
        public string SelectedEmployeeNumber { get; set; }

        public string SelectedEmployeeName { get; set; }

        public OperationStatus OperationStatus { get; set; } = new OperationStatus();
    }

    public class GroupMember
    { 
        public string ADGroup { get; set; }

        public TetraTechUser Employee { get; set; }
    }

    public class OperationStatus
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "";
    }
}