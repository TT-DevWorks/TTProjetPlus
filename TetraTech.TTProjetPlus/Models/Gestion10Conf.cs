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
    //public class Gestion10Conf
    //{
    //}

    public class Manage10ConfMembersViewModel : BaseViewModel
    {
        public List<SelectListItem> Gestion10ConfGroupsList { get; set; }

        public Gestion10ConfMemberDetails Gestion10ConfDetails { get; set; }
               
        public EmployeeGroup10Conf Details { get; set; }

        public Manage10ConfMembersViewModel(BaseViewModel _base) : base(_base) { }

        //[Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public List<TetraTechUser> ADEmployeeList { get; set; } = new List<TetraTechUser>();
    }

    public class Gestion10ConfList
    {
        public List<Gestion10ConfItem> disciplineItem { get; set; }
        
    }

    public class Gestion10ConfItem
    {
        public int DiscId { get; set; }
        public string DisciplineName { get; set; }
    }

    public class Gestion10ConfMember
    {
        public string Gestion10ConfGroup { get; set; }
        public string Gestion10ConfGroupID { get; set; }
        public TetraTechUser Employee { get; set; }
    }

    public class Gestion10ConfMemberDetails
    {
        [Required]
        public string SelectedGestion10Conf { get; set; } = "";

        public List<Gestion10ConfMember> Gestion10ConfGroupMembersList { get; set; } = new List<Gestion10ConfMember>();

    }

    public class EmployeeGroup10Conf
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


    public class CommonModelGestion10Conf
    {
        [Required]
        public string SelectedGestion10Conf { get; set; } = "";
        public string SelectedGestion10ConfID { get; set; } = "";

        public string AvailableToEmployeesMembersOf { get; set; } = "";

        public List<Gestion10ConfMember> Gestion10ConfGroupMembersList { get; set; } = new List<Gestion10ConfMember>();

        public string PanelTitle { get; set; } = "";

        [Required]
               
        public TetraTechUser Employee { get; set; }
    }

    public class Gestion10ConfEmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
        public string TTAccount { get; set; }
        public string discDocProjId { get; set; }
    }

    public class Gestion10ConfEmployeeItem2
    {
        public TetraTechUser anEmployee { get; set; }

        public static explicit operator Gestion10ConfEmployeeItem2(List<TetraTechUserPrincipal> v)
        {
            throw new NotImplementedException();
        }
    }


}