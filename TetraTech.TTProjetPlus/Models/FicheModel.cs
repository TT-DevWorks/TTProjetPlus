using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class FicheModel
    {
        public Boomerang_tempFicheLang ficheDetails{ get; set;  }

        public string image { get; set; } = null;
        public string OPQIBI { get; set; } = null;
        public string award { get; set; } = null;
        public string awardPDF { get; set; } = null;
        public string services { get; set; } = null;
        public List<string> equipeList { get; set; } = null;
        public string periode_mois_annee_debut_initial { get; set; } = "";
        public string periode_mois_annee_fin_initial { get; set; } = "";
        public string periode_mois_annee_debut_final { get; set; } = "";
        public string periode_mois_annee_fin_final { get; set; } = "";
        public string echeancier_mois_annee_debut_initial { get; set; } = "";
        public string echeancier_mois_annee_fin_initial { get; set; } = "";
        public string echeancier_mois_annee_debut_final { get; set; } = "";
        public string echeancier_mois_annee_fin_final { get; set; } = "";
        public string planDevis_mois_annee_debut_initial { get; set; } = "";
        public string planDevis_mois_annee_fin_initial { get; set; } = "";
        public string planDevis_mois_annee_debut_final { get; set; } = "";
        public string planDevis_mois_annee_fin_final { get; set; } = "";
        public string construction_mois_annee_debut_initial { get; set; } = "";
        public string construction_mois_annee_fin_initial { get; set; } = "";
        public string construction_mois_annee_debut_final { get; set; } = "";
        public string construction_mois_annee_fin_final { get; set; } = "";
        public iTextSharp.text.Chunk texteDesc { get; set; }
    }
}