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
    //public class Discipline
    //{
    //}

    public class ManageDisciplineMembersViewModel : BaseViewModel
    {
        public List<SelectListItem> DisciplineGroupsList { get; set; }

        public DisciplineMemberDetails DiscDetails { get; set; }

        public DocProjMemberDetails DocProjDetails { get; set; }

        public EmployeeGroup Details { get; set; }

        public ManageDisciplineMembersViewModel(BaseViewModel _base) : base(_base) { }

        //[Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public List<TetraTechUser> ADEmployeeList { get; set; } = new List<TetraTechUser>();
    }

    public class DisciplineDocProj
    {
        public List<disciplineItem> disciplineItem { get; set; }
        public List<docProjItem> docProjItem { get; set; }
    }
    public class disciplineStructureItem
    {
        public string Company { get; set; }
        public string DisciplineName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class disciplineItem
    {
        public int DiscId { get; set; }
        public string DisciplineName { get; set; }
    }

    public class docProjItem
    {
        public int DocProj_ID { get; set; }
        public string DocProj_Name { get; set; }

    }

    public class DisciplineMemberDetails
    {
        [Required]
        public string SelectedDiscipline { get; set; } = "";

        public List<DisciplineMember> DisciplineGroupMembersList { get; set; } = new List<DisciplineMember>();

    }

    public class DisciplineMember
    {
        public string DiscGroup { get; set; }
        public string DiscGroupID { get; set; }
        public TetraTechUser Employee { get; set; }
    }

    public class DocProjMemberDetails
    {
        [Required]
        public string SelectedDocProj { get; set; } = "";

        public List<DocProjMember> DocProjGroupMembersList { get; set; } = new List<DocProjMember>();
    }

    public class DocProjMember
    {
        public string DocProjGroupID { get; set; }
        public string DocProjGroup { get; set; }

        public TetraTechUser EmployeeD { get; set; }
    }

    public class CommonModel
    {
        [Required]
        public string SelectedDiscipline { get; set; } = "";
        public string SelectedDisciplineID { get; set; } = "";

        public string AvailableToEmployeesMembersOf { get; set; } = "";

        public List<DisciplineMember> DisciplineGroupMembersList { get; set; } = new List<DisciplineMember>();

        public string PanelTitle { get; set; } = "";

        [Required]
        public string SelectedDocProj { get; set; } = "";

        public List<DocProjMember> DocProjGroupMembersList { get; set; } = new List<DocProjMember>();

        public TetraTechUser Employee { get; set; }
    }

    public class EmployeeGroup
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

    public class DisciplineEmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
        public string TTAccount { get; set; }
        public string discDocProjId { get; set; }
    }

    public class DisciplineEmployeeItem2
    {
        public TetraTechUser anEmployee { get; set; }

        public static explicit operator DisciplineEmployeeItem2(List<TetraTechUserPrincipal> v)
        {
            throw new NotImplementedException();
        }
    }
}