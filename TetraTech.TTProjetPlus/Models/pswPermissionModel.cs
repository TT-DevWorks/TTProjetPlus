using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class pswPermissionModel
    {
        public string Nom_employee { get; set; }
        public string Num_employee { get; set; }
        public string email { get; set; }
        public string Titre { get; set; }
        public string Division { get; set; }
        public string Commentaire { get; set; }
        public string Fait { get; set; }
        public string Retrait { get; set; }
        public string listeType;

        //pour liste approbateurs
        public string Nom_approbateur;
        public string Nom_Division_market;
        public string autorisation;

    }
}