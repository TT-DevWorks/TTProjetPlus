using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Views.ManageDirectoryRights;

namespace TetraTech.TTProjetPlus.Models
{
    public class ManageDirectoryRightsViewModel : BaseViewModel
    {
        
        [Display(Name = "DirectoryRightList", ResourceType = typeof(MDRessources))]
        public List<SelectListItem> DirectoryRightList { get; set; }

        public ManageDirectoryRightsDetails DirectoryRightsDetails { get; set; }        

        public ManageDirectoryRightsViewModel(BaseViewModel _base) : base(_base) { }

        public List<SelectListItem> PermissionList { get; set; } = new List<SelectListItem>();
        public string SelectedPermissionFromEmployeeObj { get; set; } = "";
        public string SelectedEmployeeName { get; set; }
    }

    public class ManageDirectoryRightsDetails
    {
        [Display(Name = "DirectoryRight", ResourceType = typeof(MDRessources))]
        [Required]
        public string SelectedDirectoryRight { get; set; } = "";       
        [Display(Name = "ProjectList", ResourceType = typeof(MDRessources))]
        public List<SelectListItem> ProjectList { get; set; }

        [Display(Name = "SelectedProject", ResourceType = typeof(MDRessources))]
        [Required]
        public string SelectedProject { get; set; } = "";
        [Display(Name = "DirectoryList", ResourceType = typeof(MDRessources))]
        public List<TTProjetDirectory> DirectoryList { get; set; }

        public List<TTProjetDirectory> OriginalDirectoryList { get; set; }

        public int SelectedDirectoryId { get; set; }
        public string SelectedDirectory { get; set; }
        public int SelectedDirectoryAllowedPermission { get; set; }

        public List<DirectoryAccess> DirectoryAccessList { get; set; } = new List<DirectoryAccess>();

        public List<SelectListItem> PermissionListFromDetails { get; set; } = new List<SelectListItem>();
        public string SelectedPermissionFromDetails { get; set; } = "";

        public string TTAccountRoleNo { get; set; } = "";
    }

    public class TTProjetDirectory
    {
        public int RepId { get; set; }
        public string RepCode { get; set; }
        public string RepDescription { get; set; }
        public int? SecurityFolderPermissionId { get; set; }
        public string RepIdAndSecurityFolderPermissionId { get; set; }
        public string TTAccountRole { get; set; }
    }

    public class DirectoryAccess
    {
        public int RepId { get; set; }
        public string RepCode { get; set; }
        public int? SecurityFolderPermissionId { get; set; }
        public string Value { get; set; }
        public string AccesTetraLinx { get; set; }
        public string Permission { get; set; }
        public int ProjectSecurity { get; set; }
        public int? SecurityAuthentificationTypeID { get; set; }
        public int? Rn { get; set; }
    }

    public class EmployeeDirectoryRight
    {
        [Display(Name = "DirectoryRight", ResourceType = typeof(MDRessources))]
        [Required]
        public string SelectedDirectoryRight { get; set; } = "";

        public List<SelectListItem> PermissionList { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> EmployeeList { get; set; } = new List<SelectListItem>();

        //[Required(ErrorMessage = "Vous devez sélectionner un type de permission")]        
        public string SelectedPermissionFromEmployeeObj { get; set; } = "";

        [Display(Name = "Employee", ResourceType = typeof(MDRessources))]
        [Required(ErrorMessageResourceType = typeof(MDRessources), ErrorMessageResourceName = "MessageSelectEmployee")]
        public string SelectedEmployeeNumber { get; set; }

        public string SelectedEmployeeName { get; set; }

        public string SelectedProject { get; set; }

        public int SelectedRepId { get; set; }

        public OperationStatus OperationStatus { get; set; } = new OperationStatus();
    }
}