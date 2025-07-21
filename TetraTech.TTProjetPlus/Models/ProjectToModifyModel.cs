using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ProjectToModifyModel
    {
       [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectNumberError")]
       public string projetcNumber { get; set; }
       public int  selectionModel { get; set; }
       public List<string> listFolderToAdd { get; set; }
    }
}