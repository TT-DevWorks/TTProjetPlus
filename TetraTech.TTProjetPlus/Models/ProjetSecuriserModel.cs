using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class ProjetSecuriserModel
    {
        public List<usp_get_ProjetSecuriserList_Result> listProjetSecuriser { get; set; }
        
        public List<structureItem> listeEmployeeActif_Inactive { get; set; }

        public List<usp_ListeProjetaSecuriser_Result> listProjetASecuriserStatut { get; set; }

    }
}