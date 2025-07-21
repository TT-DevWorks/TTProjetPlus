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
 

    public class ManageGroupeADMembersViewModel : BaseViewModel
    {
        public List<SelectListItem> GestionGroupeADGroupsList { get; set; }

        public GestionGroupeADMemberDetails GestionGroupeADDetails { get; set; }
               
        public EmployeeGroupGroupeAD Details { get; set; }

        public ManageGroupeADMembersViewModel(BaseViewModel _base) : base(_base) { }

        //[Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public List<TetraTechUser> ADEmployeeList { get; set; } = new List<TetraTechUser>();
    }

    public class GestionGroupeADList
    {
        public List<GestionGroupeADItem> disciplineItem { get; set; }
        
    }

    public class GestionGroupeADItem
    {
        public int DiscId { get; set; }
        public string DisciplineName { get; set; }
    }

    public class GestionGroupeADMember
    {
        public string GestionGroupeADGroup { get; set; }
        public string GestionGroupeADGroupID { get; set; }
        public TetraTechUser Employee { get; set; }
    }

    public class GestionGroupeADMemberDetails
    {
        [Required]
        public string SelectedGestionGroupeAD { get; set; } = "";

        public List<GestionGroupeADMember> GestionGroupeADGroupMembersList { get; set; } = new List<GestionGroupeADMember>();

    }

    public class EmployeeGroupGroupeAD
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


    public class CommonModelGestionGroupeAD
    {
        [Required]
        public string SelectedGestionGroupeAD { get; set; } = "";
        public string SelectedGestionGroupeADID { get; set; } = "";

        public string AvailableToEmployeesMembersOf { get; set; } = "";

        public List<GestionGroupeADMember> GestionGroupeADGroupMembersList { get; set; } = new List<GestionGroupeADMember>();

        public string PanelTitle { get; set; } = "";

        [Required]
               
        public TetraTechUser Employee { get; set; }
    }

    public class GestionGroupeADEmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
        public string TTAccount { get; set; }
        public string discDocProjId { get; set; }
    }

    public class GestionGroupeADEmployeeItem2
    {
        public TetraTechUser anEmployee { get; set; }

        public static explicit operator GestionGroupeADEmployeeItem2(List<TetraTechUserPrincipal> v)
        {
            throw new NotImplementedException();
        }
    }


}