using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class Impersonate
    {
        [Display(Name = "EmployeeIdDisplayName", ResourceType = typeof(ImpersonateResources))]
        [Required(ErrorMessageResourceName = "EmployeeIdRequiredErrorMessage", ErrorMessageResourceType = typeof(ImpersonateResources))]
        public string EmployeeId { get; set; }

        public string TTAccount { get; set; }
    }
}