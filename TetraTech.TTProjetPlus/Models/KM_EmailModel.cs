using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class KM_EmailModel
    {

        //# projet = obj.project_number
        //Organisation du projet = acc_center
        //Nom de l’employé = liste[2].Split('#')[0]
        //Rôle du membre clé = roleEnglish
        //Titre = titre

        public string projetNum { get; set; }
        public string Organisation_projet { get; set; }
        public string empName { get; set; }
        public string role { get; set; }
        public string titre { get; set; }

    }
}