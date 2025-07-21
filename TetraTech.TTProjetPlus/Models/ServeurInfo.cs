using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ServeurInfo
    {
        [Required(ErrorMessageResourceType = typeof(Views.LecteurReseau.LecteurReseauRessources), ErrorMessageResourceName = "ServerNumberError")]
        public int SER_id { get; set; }
        public string SER_Nom { get; set; }
        public  string SER_Ip { get; set; }
        public string SER_Description { get; set; }
        public string SER_Compagnie { get; set; }
        public string SER_Domaine { get; set; }
        public string SER_Chemin_Local { get; set; }
        public string SER_Utilisateur { get; set; }
        public string SER_Chemin_Partage { get; set; }
        public bool SER_Map_Seulement { get; set; }
        public bool SER_Bilingue { get; set; }

    }
}