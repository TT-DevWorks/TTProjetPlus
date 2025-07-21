using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class BoomerangSearchModel
    {
        public List<usp_Boomerang_getExpertises_Result> listeExpertise { get; set; }
        public FicheModel fiche { get; set; }
        public List<string> listePole { get; set; }
        public List<string> listActivite { get; set; }
        public List<ComplexItem> listItems { get; set; }
        public List<string> listProjects { get; set; }
        public List<string> listBusinessPlaces { get; set; }
        public List<string> typeUnities { get; set; }
        public List<string> customers { get; set; }
        public List<string> disciplines { get; set; }
        public List<panier> listePaniers { get; set; }
        public List<province> listeProvinces { get; set; }
        public List<pay> listePays { get; set; }
        public List<type_mission> listeTypeMission { get; set; }
        public List<type_unite> listeTypeUnits { get; set; }
        public List<agence> listeAgences { get; set; }
    }
    public class ComplexItem
    {
        public Nullable<decimal> id { get; set; }
        public string libelle { get; set; }
        public string color { get; set; }

    }

    public class researchResult
    {
        public string id_fiche { get; set; }
        public string localisation { get; set; }
        public string endYear{ get; set; }
        public string intitutle_court { get; set; }
         public string status { get; set; }
        public string nom_award { get; set; }
        public string lastmod_opqibi { get; set; }
        public string existInEn { get; set; }
        public string existInEsp { get; set; }
        public string phototheque { get; set; }
        public string hasArgumentaire { get; set; }

    }

}
