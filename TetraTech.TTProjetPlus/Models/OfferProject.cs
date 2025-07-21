using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class OfferProject
    {
        public string Title { get; set; }
        public string ProjectNumber { get; set; }
        public int ClientNumber { get; set; }
        public string ClientName { get; set; }
        public string Company { get; set; }
        public string BussinessUnit { get; set; }
        public string ORG { get; set; }
        public string ProjectManager { get; set; } // gestionnaire de projet
        public string ProgramManager { get; set; } // gestionnaire de programme
        public string FacturationAgent { get; set; }

        public string FinalClient { get; set; }
        public string ContactClient { get; set; }
        public int ApproximateAmount { get; set; }
        public DateTime Date { get; set; }
        public string RefAndComments { get; set; }
        public string InitiatedBy { get; set; }
        public string Status { get; set; }
        public string CodePhase { get; set; }
        public string MasterProject { get; set; }

        public string FacturationAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string EmailAdressIf { get; set; }
        public string ContractType { get; set; }
        public string Program { get; set; }
        public string WorkCategory { get; set; }
        public string OfficeAddress { get; set; }
        public string Currency { get; set; }
        public string ContractNumber { get; set; }
        public DateTime OpeningDate { get; set; }


    }
}