using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class BoomerangHomeModel
    {
        public List<panier> listePanier { get; set; }
        public List<researchResult> listeFiches { get; set; }
        public List<usp_Boomerang_getDraftsForUser_Result> listeFichesDrafts { get; set; }
        public List<usp_Boomerang_getDroitByUA_Result> listeADroit { get; set; }
        public List<utilisateur> listeResp { get; set; }
    }
}