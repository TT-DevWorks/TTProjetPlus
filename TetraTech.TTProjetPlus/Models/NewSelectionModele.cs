using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class NewSelectionModele
    {
        [Required(ErrorMessageResourceType = typeof(Views.ModeleSelection.ModeleSelectionManagementRessources), ErrorMessageResourceName = "requiredField")]
        public string modeleSelectionName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.ModeleSelection.ModeleSelectionManagementRessources), ErrorMessageResourceName = "requiredField")]
        public string modeleSelectionNameTomodify { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.ModeleSelection.ModeleSelectionManagementRessources), ErrorMessageResourceName = "requiredField")]
        public string modeleSelectionNameToDelete { get; set; }

        
        [Required(ErrorMessageResourceType = typeof(Views.ModeleSelection.ModeleSelectionManagementRessources), ErrorMessageResourceName = "requiredField")]
        public string modeleSelectionDescription { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.ModeleSelection.ModeleSelectionManagementRessources), ErrorMessageResourceName = "requiredField")]
        public string modID { get; set; }

        public string copyingFromExistingModeleSelection { get; set; }
    }
}