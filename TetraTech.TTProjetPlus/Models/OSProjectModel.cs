using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class OSProjectModel
    {
      
        public int OS_Project_ID { get; set; }
        public string Project_Number { get; set; }
        public string Project_Description { get; set; } //	Titre abrégé de l’OS / Titre abrégé du projet
        public bool Is_Master { get; set; } //	Projet lié ou un projet maître
        public string Project_Master_ID { get; set; }
        public int? Project_Master_Origine_ID { get; set; }
        public string PhaseCode { get; set; }
        public int? PhaseCodeType_ID { get; set; }
        public string Customer_Number { get; set; }
        public string Customer_Name { get; set; }   //	Nom du client
        public string Company { get; set; } //	Compagnie
        public string Acct_Group_ID { get; set; }
        public string Acct_Group_Name { get; set; } //	Unité d’affaire
        public string Acct_Center_ID { get; set; } 
        public string Acct_Center_Name { get; set; }//	ORG -> Organisation
        public string Project_Mgr_Number { get; set; }  //	Gestionnaire de projet
        public string Project_Mgr_Name { get; set; }
        public string End_Client_Name { get; set; }  //	Gestionnaire de projet
        public string Contact_Client { get; set; }  //	Contact Client
        public string Bill_Specialist_Number { get; set; } //	Agente de facturation
        public string Initiated_By { get; set; }    //	Initié par (OS  ou projet)
        public int? Approx_Amount { get; set; }  //	Montant approximatif
        public string Comments { get; set; }    //	Références et Commentaires
        public string Billing_Address { get; set; } //	Adresse de facturation
        public string Shipping_Address { get; set; }    //	Adresse d’expédition
        public string Invoice_Email_Address { get; set; }   //	Adresse courriel d’envoi de la facture (si envoi électronique)
        public int? Contract_Type_ID { get; set; }   //	Type de contrat
        public int? Program_ID { get; set; } //	Programme
        public string Work_Type_ID { get; set; }    //	Catégorie du travail (SF330)
        public int? Office_Address_Id { get; set; }   //	Adresse du bureau (facture)
        public string Program_Mgr_Number { get; set; }  //	Gestionnaire de programme
        public string Program_Mgr_Name { get; set; }  //	Gestionnaire de programme
        public int? Currency_ID { get; set; } //	Devise
        public string Contract_Number { get; set; } //	No de contrat/Bon de commande
        public DateTime? Required_Opening_Date { get; set; } //	Date d’ouverture du projet
        public int? Status_ID { get; set; }	//	Statut
        public string Last_Modification_Date { get; set; }
        public string Last_Modification_User { get; set; }
        public string Organisation { get; set; }
        public string GestionnaireProjet { get; set; }
        public string Initiated_By_Name { get; set; }
        public string StatusName { get; set; }
        public string StatusDocName { get; set; }
        public string Doc_PostponedDate { get; set; }
        public DateTime? dDoc_PostponedDate { get; set; }
        public string FDoc_PostponedDate { get; set; }        
        public string submittedDate { get; set; }
        public DateTime? submittedDateForSorting { get; set; }
        public string emailContent { get; set; }
        public string path { get; set; }
        public List<structureItem> ListeUAForCompany { get; set; }
        public List<structureItem> ListeORGForUA { get; set; }
        public string ProjectModId { get; set; }
        public string Client_ProjectManager { get; set; }
        public string Client_Email_ProjectManager { get; set; }

    }
}