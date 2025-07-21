using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class SearchCriteriasModel
    {
        public List<structureItem> listeProjectNumbers { get; set; }
        public List<structureItem> listeProjectDescription { get; set; }
        public List<structureItem> ListeCustomersNumbers { get; set; }
        public List<structureItem> listeCustomersName { get; set; }
        public List<structureItem> listeAcctCenterID { get; set; }
        public List<structureItem> listeInitiatedBy { get; set; }
        public List<structureItem> listeProgramMgrNumber { get; set; }
        public List<structureItem> ListeBillSpecialistNumber { get; set; }
        public List<structureItem> listeStatusID  { get; set; }
        public List<structureItem> listeSubmittedDate { get; set; }
        public List<structureItem> listeArchivedate { get; set; }
        public List<structureItem> listeDocPostponedDate { get; set; }
        public List<structureItem> listeDocStatutID { get; set; }
        public List<structureItem> listeFinalClient{ get; set; }
    }
}