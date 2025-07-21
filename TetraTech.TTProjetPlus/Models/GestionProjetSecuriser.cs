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
   
    public class ManageProjetSecuriserMembersViewModel : BaseViewModel
    {
        public List<SelectListItem> GestionProjetSecuriserGroupsList { get; set; }

        public GestionProjetSecuriserMemberDetails GestionProjetSecuriserDetails { get; set; }
               
        public EmployeeGroupProjetSecuriser Details { get; set; }

        public ManageProjetSecuriserMembersViewModel(BaseViewModel _base) : base(_base) { }

        //[Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public List<TetraTechUser> ADEmployeeList { get; set; } = new List<TetraTechUser>();
    }

    public class GestionProjetSecuriserList
    {
        public List<GestionProjetSecuriserItem> disciplineItem { get; set; }
        
    }

    public class GestionProjetSecuriserItem
    {
        public int DiscId { get; set; }
        public string DisciplineName { get; set; }
    }

    public class GestionProjetSecuriserMember
    {
        public string GestionProjetSecuriserGroup { get; set; }
        public string GestionProjetSecuriserGroupID { get; set; }
        public TetraTechUser Employee { get; set; }
    }

    public class GestionProjetSecuriserMemberDetails
    {
        [Required]
        public string SelectedGestionProjetSecuriser { get; set; } = "";

        public List<GestionProjetSecuriserMember> GestionProjetSecuriserGroupMembersList { get; set; } = new List<GestionProjetSecuriserMember>();

    }

    public class EmployeeGroupProjetSecuriser
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


    public class CommonModelGestionProjetSecuriser
    {
        [Required]
        public string SelectedGestionProjetSecuriser { get; set; } = "";
        public string SelectedGestionProjetSecuriserID { get; set; } = "";

        public string AvailableToEmployeesMembersOf { get; set; } = "";

        public List<GestionProjetSecuriserMember> GestionProjetSecuriserGroupMembersList { get; set; } = new List<GestionProjetSecuriserMember>();

        public string PanelTitle { get; set; } = "";

        [Required]
               
        public TetraTechUser Employee { get; set; }
    }

    public class GestionProjetSecuriserEmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
        public string TTAccount { get; set; }
        public string discDocProjId { get; set; }
    }

    public class GestionProjetSecuriserEmployeeItem2
    {
        public TetraTechUser anEmployee { get; set; }

        public static explicit operator GestionProjetSecuriserEmployeeItem2(List<TetraTechUserPrincipal> v)
        {
            throw new NotImplementedException();
        }
    }


}