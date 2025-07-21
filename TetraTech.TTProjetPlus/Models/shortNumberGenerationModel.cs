using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class shortNumberGenerationModel
    {
        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectNumberError")]
        public string projetcNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ServerError")]
        public string Server { get; set; }
        public string ServerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ServerModelError")]
        public string ServerModel { get; set; }
        public string ServerModelName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "SecurityModelError")]
        public string SecurityModel { get; set; }
        public string SecurityModelName { get; set; }

        
        public string SelectionModel { get; set; }
        public string SelectionModelName { get; set; }
    }
}