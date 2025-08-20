using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class getProjetAvecDateDepasseModel
    {
        public List<usp_TT_getProjetAvecDateDepasse_Result> listeProjects { get; set; }
        public List<CP> listeCP { get; set; }
    
    }

    public class CP
    {
        public string CP_num { get; set; }
        public string CP_name { get; set; }
    }


    public class LigneData
    {
        public string cpNum { get; set; }
        public string cpName { get; set; }
        public string ProjetNum { get; set; }
        public string NouvelleDate { get; set; }
        public string AFermer { get; set; }
        public string Commentaire { get; set; }
    }
}