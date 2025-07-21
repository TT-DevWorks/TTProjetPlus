using System; 
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Views.ProjectToClose;

namespace TetraTech.TTProjetPlus.Models
{
    public class ProjectToCloseViewModel : BaseViewModel
    {  
        public ProjectToCloseModel Details { get; set; }

        public ProjectToCloseViewModel(BaseViewModel _base) : base(_base) { }

    }

    public class ProjectToCloseModel
    {  
        public int OSProjectID { get; set; }
        public int ProjectToCloseId { get; set; }
        public int TlinxProjectId { get; set; }

        public string OSProjectNumber { get; set; }

        public string TlinxProjectNumber { get; set; }

        public string CurrentUser { get; set; } 

        public bool MenuVisible { get; set; } = true;

        public TypesEnum AccessType { get; set; } 

        public ProjectInformationsModel ProjectInformations { get; set; } = new ProjectInformationsModel(); 


        //view 

        public int StatusID { get; set; } 

        public DateTime? SubmittedDate { get; set; }

        [DateValidation]
        [RequiredIfOtherField("EstimateEndDateIsRequired", ErrorMessageResourceName = "EstimatedEndDateRequired", ErrorMessageResourceType = typeof(ProjectToCloseViewModelResources))]
        [Display(Name = "EstimateEndDate", ResourceType = typeof(ProjectToCloseRessources))] 
        public string EstimateEndDateString { get; set; }

        public DateTime? EstimateEndDate
        {
            get
            {
                return string.IsNullOrEmpty(EstimateEndDateString) ? (DateTime?)null : DateTime.Parse(EstimateEndDateString);
            }
            set
            {
                EstimateEndDateString = value?.ToString("yyyy-MM-dd");
            }
        } 

        public string EstimateEndDateDefault {get;set;} 

        public int DocStatusID { get; set; }  

        public string StatusDocName { get; set; }

        public string submitBy { get; set; }

        public string submitByName { get; set; }

        [DateValidation]        
        [RequiredIfOtherField("DocPostponedDateTimeIsRequired", ErrorMessageResourceName = "PostponementDateRequired", ErrorMessageResourceType = typeof(ProjectToCloseViewModelResources))]
        [Display(Name = "Doc_PostponedDateTime", ResourceType = typeof(ProjectToCloseRessources))]
        public string DocPostponedDateTimeString { get; set; }
        public DateTime? DocPostponedDateTime
        {
            get
            {
                return string.IsNullOrEmpty(DocPostponedDateTimeString) ? (DateTime?)null : DateTime.Parse(DocPostponedDateTimeString);
            }
            set
            {
                DocPostponedDateTimeString = value?.ToString("yyyy-MM-dd");
            }
        } 

        public string DocPostponedDateDefault { get; set; } 

        public string ProjectToCloseProjectManager { get; set; } 
        
        [AllowHtml]
        public string CommentFermeture_Projet { get; set; }


        public bool ToBeClosed { get; set; }

        public bool ToBeArchived { get; set; }

        public bool IsClosedOrPostponed { get; set; }

        public bool IsArchivedOrPostponed { get; set; }

        public string EmailLang { get; set; }


        #region "Properties"  

        public bool IsProject => new int[] { 8, 6, 4 }.Any(s => s == StatusID);

        public bool IsActiveProject => (StatusID == 4);

        public bool NoDocumentation => (DocStatusID == 5); 

        public bool CurrentUserIsPM_AF => (string.IsNullOrEmpty(CurrentUser)? false: (CurrentUser == ProjectInformations.ProjectManager_Num || CurrentUser == ProjectInformations.BillingSpecialist_Num));

        public string SubmittedDateLabel
        {
            get { CultureInfo culture = new CultureInfo("fr-CA", true); return ((SubmittedDate != null) ? SubmittedDate?.ToString("dd MMM yyyy", culture) : "N/A"); }
        } 

        public bool EstimateEndDateIsRequired => (!ToBeClosed);

        public bool DocPostponedDateTimeIsRequired => ToBeClosed && ToBeArchived==false &&  NoDocumentation==false; 

        public bool HasAccess => AccessType == TypesEnum.ProjectToClose || AccessType == TypesEnum.ReadOnly;

        public string AccessMessage =>
           (AccessType == TypesEnum.NA) ? string.Empty:
           (AccessType == TypesEnum.AccessDenied) ? string.Format(ProjectToCloseRessources.ProjectToCloseAccessDenied, ProjectInformations.ProjectManager_Name, ProjectInformations.BillingSpecialist_Name) :
           (AccessType == TypesEnum.NotFound) ? string.Format(Resources.TTProjetPlusResource.NoResultsForProjectNumber, "OSProjectID:" +  OSProjectID) :
           (AccessType == TypesEnum.NotFoundTlx) ? string.Format(Resources.TTProjetPlusResource.NoResultsForProjectNumber, TlinxProjectNumber) :
           (AccessType == TypesEnum.Other) ? string.Format(ProjectToCloseRessources.NotAProject, OSProjectNumber) :
            ""; 

        #endregion

        public ProjectToCloseModel() { }

        public ProjectToCloseModel(string employeeid, int osProjectID, string menuVisible) {
            OSProjectID = osProjectID;
            CurrentUser = employeeid;
            MenuVisible = (menuVisible == "true");
        }
    }

    public class ProjectInformationsModel
    {
        public string ProjectDescription { get; set; }

        public string ProjectManager_Num { get; set; }
        public string ProjectManager_Name { get; set; }
        public string ProjectManager_Email { get; set; } 

        public string BillingSpecialist_Num { get; set; }
        public string BillingSpecialist_Name { get; set; }
        public string BillingSpecialist_Email { get; set; } 

        public string ContractAdministrator_Num { get; set; }
        public string ContractAdministrator_Name { get; set; }
        public string ContractAdministrator_Email { get; set; }

        public string ProgramManager_Num { get; set; }
        public string ProgramManager_Name { get; set; }
        public string ProgramManager_Email { get; set; }    

        public string AcctCenterName { get; set; }

        public string CustomerName { get; set; }
         
        public DateTime? ProjectStartDate { get; set; }
        public string ProjectStartDateString => ProjectStartDate?.ToString("yyyy-MM-dd"); 

        public DateTime? ProjectEndDate { get; set; }
        public string ProjectEndDateString => ProjectEndDate?.ToString("yyyy-MM-dd");
        public bool isKeyMember { get; set; }
        public string Client_ProjectManager { get; set; }
        public string Client_Email_ProjectManager { get; set; }
    }
      

}