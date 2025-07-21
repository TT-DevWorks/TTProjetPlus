using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Views.Disciplines;

namespace TetraTech.TTProjetPlus.Models
{
  
    public class Manage10ConfTQEMembersViewModel : BaseViewModel
    {
        public List<SelectListItem> Gestion10ConfTQEGroupsList { get; set; }

        public Gestion10ConfTQEMemberDetails Gestion10ConfTQEDetails { get; set; }
               
        public EmployeeGroup10ConfTQE Details { get; set; }

        public Manage10ConfTQEMembersViewModel(BaseViewModel _base) : base(_base) { }

        //[Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public List<TetraTechUser> ADEmployeeList { get; set; } = new List<TetraTechUser>();
    }

    public class Gestion10ConfTQEList
    {
        public List<Gestion10ConfTQEItem> disciplineItem { get; set; }
        
    }

    public class Gestion10ConfTQEItem
    {
        public int DiscId { get; set; }
        public string DisciplineName { get; set; }
    }

    public class Gestion10ConfTQEMember
    {
        public string Gestion10ConfTQEGroup { get; set; }
        public string Gestion10ConfTQEGroupID { get; set; }
        public TetraTechUser Employee { get; set; }
    }

    public class Gestion10ConfTQEMemberDetails
    {
        [Required]
        public string SelectedGestion10ConfTQE { get; set; } = "";

        public List<Gestion10ConfTQEMember> Gestion10ConfTQEGroupMembersList { get; set; } = new List<Gestion10ConfTQEMember>();

    }

    public class EmployeeGroup10ConfTQE
    {
        //[Display(Name = "GroupMembers", ResourceType = typeof(Discipline))]
        [Required]
        public string SelectedGroup { get; set; }

        public string GroupType { get; set; }


        [Display(Name = "Employee", ResourceType = typeof(Discipline))]
        //[Required(ErrorMessageResourceType = typeof(Discipline), ErrorMessageResourceName = "MessageSelectEmployee")]
        public string SelectedEmployeeNumber { get; set; }

        public string SelectedEmployeeName { get; set; }

        public OperationStatus OperationStatus { get; set; } = new OperationStatus();
    }


    public class CommonModelGestion10ConfTQE
    {
        [Required]
        public string SelectedGestion10ConfTQE { get; set; } = "";
        public string SelectedGestion10ConfTQEID { get; set; } = "";

        public string AvailableToEmployeesMembersOf { get; set; } = "";

        public List<Gestion10ConfTQEMember> Gestion10ConfTQEGroupMembersList { get; set; } = new List<Gestion10ConfTQEMember>();

        public string PanelTitle { get; set; } = "";

        [Required]
               
        public TetraTechUser Employee { get; set; }
    }

    public class Gestion10ConfTQEEmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
        public string TTAccount { get; set; }
        public string discDocProjId { get; set; }
    }

    public class Gestion10ConfTQEEmployeeItem2
    {
        public TetraTechUser anEmployee { get; set; }

        public static explicit operator Gestion10ConfTQEEmployeeItem2(List<TetraTechUserPrincipal> v)
        {
            throw new NotImplementedException();
        }
    }


}