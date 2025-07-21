using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class numberGenerationModel
    {
        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectNumberError")]
        public string projetcNumber  { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations),ErrorMessageResourceName = "TitleError")]
        public string AbreviatedTitle { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "HasAMasterProjectError")]
        public string HasAMasterProject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "MasterProjectNumber")]
        public string MasterProjectNumber { get; set; }

        public string PhaseCode { get; set; }
        public string PhaseCodeFinal { get; set; }
        public string PhaseCodeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ClientNumberError")]
        public string ClientNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ClientNameError")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "CompanyError")]
        public string Company { get; set; }
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "BusinessUnitError")]
        public string BusinessUnit { get; set; }
        public string BusinessUnitName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ORGError")]
        public string ORG { get; set; }
        public string ORGName { get; set; }

        public string IniatedBy { get; set; }


        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ProjectMgrError")]
        public string ProjectMgr { get; set; }

        public string ProgramMgr { get; set; }

        public string ProjectMgrNumber { get; set; }
        public string ProgramMgrNumber { get; set; }

        public string ClientFinal { get; set; }

        public string ClientContact { get; set; }

        public string  ApproximateAmount { get; set; }

        public string RefAndComments { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ServerError")]
        public string Server { get; set; }
        public string ServerName { get; set; }
        public string ServerCheminPartage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "ServerModelError")]
        public string ServerModel { get; set; }
        public string ServerModelName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Views.Home.informations), ErrorMessageResourceName = "SecurityModelError")]
        public string SecurityModel { get; set; }
        public string SecurityModelName { get; set; }

        [Required]
        public string SelectionModel { get; set; }
        public string SelectionModelName { get; set; }

        public int GeneratedNumber { get; set; }

		public bool HasprojectStruture { get; set; }

        public bool ShowAutomaticEmailAddresses { get; set; }

        public string previewNumber  { get; set; }

        public string ClientProjectManager { get; set; }

        public string ClientEmailProjectManager { get; set; }

        public bool HasProjectDiligente { get; set; }

    }
    public class Color
    {
        public int ColorId { get; set; }
        public string Name { get; set; }
    }

    public class Letter
    {
        public char LetterName { get; set; }
        public char Value { get; set; }
    }

	public class Number
	{
		public int NumberName{ get; set; }
		public int Value { get; set; }
	}

    public class structureItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string email { get; set; }
    }

    public class masterProjectItem
    {
        public string Name { get; set; }
        public string ProjectNumber { get; set; }
        public string ClientName { get; set; }
        public string Org { get; set; }

    }
    public class ProjectManagerItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string email { get; set; }
    }
    public class ResumeItem
    {
        //Description du projet -> project_desc
        //Organisation -> acct_center
        //Statut du projet -> project_status_code
        //Client -> customer_name
        //Gestionnaire de projet -> project_mgr_name
        //Gestionnaire de programme -> program_mgr_name
        //Agent de facturation -> bill_specialist_name

       public string Description_projet { get; set; }//-> project_desc
       public string Organisation { get; set; } //-> acct_center
       public string Statut_projet { get; set; }//-> project_status_code
       public string Client { get; set; } //-> customer_name
       public string Gestionnaire_projet { get; set; } //-> project_mgr_name
       public string Gestionnaire_programme { get; set; }//-> program_mgr_name
       public string Agent_facturation { get; set; }//-> bill_specialist_name

    }


    public class NodeItem
    {
        public string nodeID { get; set; }
        public string nodeDescription { get; set; }
    }


    public class parentChildItemString
    {
        public List<string> parentList { get; set; }
        public string child { get; set; }
    }

    public class childParentItemString
    {
        public List<string> childList { get; set; }
        public string parent { get; set; }
    }


    public class parentChildItem
    {
        public int? parent { get; set; }
        public int? child { get; set; }
    }
    public class treeStructure
    {
        public int Parent { get; set; }
        public object[] ChidrenListTree { get; set; }
    }

    public class node
    {
        public string name { get; set; }
        public string key { get; set; }
        public string parentKey { get; set; }
        public bool  Checked { get; set; }
        public string RepId { get; set; }
        public bool Disabled123 { get; set; }//pour garder ecocher les nodes 1,2,3
        public bool AllDisabled { get; set; } //pour voir les modeles communs sans les editer
        public string Code { get; set; }
        public bool FolderCheckDisabled { get; set; }
        public string HasThisRep { get; set; }

        public List<node> children { get; set; }

        public node(string Name, string Key, string PK, bool tick, string repId, bool disabled123, bool allDisabled, string code, bool folderCheckDisabled, string hasThisRep)
        {
            name = Name;
            key = Key;
            parentKey = PK;
            Checked = tick;
            RepId = repId;
            Disabled123 = disabled123;
            AllDisabled = allDisabled;
            Code = code;
            FolderCheckDisabled = folderCheckDisabled;
            HasThisRep = hasThisRep;
        }
    }
    public class node2
    {
        public node item { get; set; }
        public IList<node> children { get; set; }
    }

}