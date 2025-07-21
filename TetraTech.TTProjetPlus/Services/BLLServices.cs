using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TT.STEP.Services.Model;
using TT.STEP.Services.DAL;
using System.Web.Script.Serialization;
using BPR.ProjectOpening.Services.Model;
using BPR.ProjectOpening.Services.BLL;
using BPR.ProjectOpening.Services.DAL;
using BPR.ProjectOpening.Windows;
using BPR.Data.BusinessModel.Model;
using BPR.Data.BPRProject.Model;
using BPR.Vortex.Services.ProjectOpening.DAL;
using BPR.Vortex.Services.Common.Model;
using System.Net;
using BPR.Vortex.Plugin;
using BPR.Security;
using BPR.BPRProjet.BLL;
using BPR.BPRProjet.Model;
using System.Security;
using System.IO;

namespace TT.STEP.Services.BLL
{


    public enum StepWebServicesLogType:int
    {
        Information = 1,
        Error = 2
    }
    
    public class BLLServices
    {
        private readonly TTProjetPlusEntities _TTEntity = new TTProjetPlusEntities();

        DALServices mDALService = new DALServices();
        public StepWebServicesLogType mintLogType = StepWebServicesLogType.Information;
        public DateTime mStartDateTime;
        public DateTime mEndDateTime;
        public Boolean mblnCreationSuccessful = false;
        public String mstrSubmitBy = "";
        public String mstrInformation = "";
        public String mstrProjectNo = "";
        public String mstrError = "";

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.Client> ClientList(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.Client> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.Client>();
            List<BPR.Data.BusinessModel.Model.Client> uneListe = new List<BPR.Data.BusinessModel.Model.Client>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetClients(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.Employee> EmployeesList(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.Employee> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.Employee>();
            List<BPR.Data.BusinessModel.Model.Employee> uneListe = new List<BPR.Data.BusinessModel.Model.Employee>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetEmployees(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<ProjectNumberRequestDisplay> ProjectsByInitiatedBy(string initiatedBy)
        {
            webserviceresultGeneric<BPR.ProjectOpening.Services.Model.ProjectNumberRequestDisplay> ws = new webserviceresultGeneric<BPR.ProjectOpening.Services.Model.ProjectNumberRequestDisplay>();
            List<BPR.ProjectOpening.Services.Model.ProjectNumberRequestDisplay> uneListe = new List<ProjectNumberRequestDisplay>();            
            BPR.ProjectOpening.Services.DAL.DALProjectNumberRequest aDALService = new DALProjectNumberRequest();         
            int recordcount;

            uneListe = aDALService.GetProjectNumberRequestByInitiatedBy(initiatedBy, "fr");
            recordcount = uneListe.Count;           
            
            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> InterCoSeries(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.ProjectOpening.Services.Model.ProjectNumInterCoReservedSerie> uneListe = new List<BPR.ProjectOpening.Services.Model.ProjectNumInterCoReservedSerie>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListeRetour = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();
            BPR.Data.BusinessModel.Model.BusinessPlace aBusinessPlace;
            BPR.Vortex.Services.ProjectOpening.DAL.DALProjectNumberBPR aDALService = new BPR.Vortex.Services.ProjectOpening.DAL.DALProjectNumberBPR();                        
            int recordcount;                      
                  
            uneListe = aDALService.ObtainInterCoReservedSeries();
            recordcount = uneListe.Count;

            foreach (BPR.ProjectOpening.Services.Model.ProjectNumInterCoReservedSerie value in uneListe)
            {
                aBusinessPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
                aBusinessPlace.Id = value.Id.ToString();
                aBusinessPlace.Name = value.SerieLabelFr;
                uneListeRetour.Add(aBusinessPlace);
            }

            ws.data = uneListeRetour;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<ActivitySector> Activities(string search, string start, string limit)
        {
            webserviceresultGeneric<ActivitySector> ws = new webserviceresultGeneric<ActivitySector>();
            List<ActivitySector> uneListe = new List<ActivitySector>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetActivities(search, start, limit, ref recordcount);
            
            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> Offices(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetOffices(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> PotentialValueServiceOffer(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetPotentialValueServiceOffer(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> MasterProjectNumbers(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetMasterProjectNumbers(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ProjectModels(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BPRProject.Model.ProjectModel> uneListe = new List<BPR.Data.BPRProject.Model.ProjectModel>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListeRetour = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();
            BPR.Data.BusinessModel.Model.BusinessPlace aBusinessPlace;            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetProjectModels(search, start, limit, ref recordcount);

            foreach (BPR.Data.BPRProject.Model.ProjectModel value in uneListe)
            {
                aBusinessPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
                aBusinessPlace.Id = value.Id.ToString();
                aBusinessPlace.Name = value.Name;
                uneListeRetour.Add(aBusinessPlace);
            }

            ws.data = uneListeRetour;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ProjectSecurityModels(string search, string start, string limit, string modId)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BPRProject.Model.SecurityModel> uneListe = new List<BPR.Data.BPRProject.Model.SecurityModel>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListeRetour = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();
            BPR.Data.BusinessModel.Model.BusinessPlace aBusinessPlace;            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetProjectSecurityModels(search, start, limit, modId, ref recordcount);

            foreach (BPR.Data.BPRProject.Model.SecurityModel value in uneListe)
            {
                aBusinessPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
                aBusinessPlace.Id = value.Id.ToString();
                aBusinessPlace.Name = value.Name;
                uneListeRetour.Add(aBusinessPlace);
            }

            ws.data = uneListeRetour;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ProjectSelectionModels(string search, string start, string limit, string modId, string user)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BPRProject.Model.SelectionModel> uneListe = new List<BPR.Data.BPRProject.Model.SelectionModel>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListeRetour = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();
            BPR.Data.BusinessModel.Model.BusinessPlace aBusinessPlace;            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetProjectSelectionModels(search, start, limit, modId, user, ref recordcount);

            foreach (BPR.Data.BPRProject.Model.SelectionModel value in uneListe)
            {
                aBusinessPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
                aBusinessPlace.Id = value.Id.ToString();
                aBusinessPlace.Name = value.Name;
                uneListeRetour.Add(aBusinessPlace);
            }

            ws.data = uneListeRetour;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ProjectServers(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BPRProject.Model.Server> uneListe = new List<BPR.Data.BPRProject.Model.Server>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListeRetour = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();
            BPR.Data.BusinessModel.Model.BusinessPlace aBusinessPlace;            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetProjectServers(search, start, limit, ref recordcount);

            foreach (BPR.Data.BPRProject.Model.Server value in uneListe)
            {
                aBusinessPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
                aBusinessPlace.Id = value.Id.ToString();
                aBusinessPlace.Name = value.Name;
                uneListeRetour.Add(aBusinessPlace);
            }

            ws.data = uneListeRetour;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ContractingCompanies(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
            List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetContractingCompanies(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessUnit> BusinessUnitDivisions(string search, string start, string limit)
        {
            webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessUnit> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessUnit>();
            List<BPR.Data.BusinessModel.Model.BusinessUnit> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessUnit>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetBusinessUnitDivisions(search, start, limit, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        public webserviceresultGeneric<BPR.Data.BusinessModel.Model.Market> MarketDivisions(string search, string start, string limit, string buid)
        {
            webserviceresultGeneric<Market> ws = new webserviceresultGeneric<Market>();
            List<Market> uneListe = new List<Market>();            
            int recordcount;

            recordcount = 0;
            uneListe = mDALService.GetMarketDivisions(search, start, limit, buid, ref recordcount);

            ws.data = uneListe;
            ws.total = recordcount;

            return ws;
        }

        // -LG- 20151116 Je ne crois pas que ce soit utilisé
        //public webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> BusinessPlaces(string search, string start, string limit)
        //{
        //    webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace> ws = new webserviceresultGeneric<BPR.Data.BusinessModel.Model.BusinessPlace>();
        //    List<BPR.Data.BusinessModel.Model.BusinessPlace> uneListe = new List<BPR.Data.BusinessModel.Model.BusinessPlace>();            
        //    int recordcount;

        //    recordcount = 0;
        //    uneListe = mDALService.GetBusinessPlaces(search, start, limit, ref recordcount);

        //    ws.data = uneListe;
        //    ws.total = recordcount;

        //    return ws;
        //}

        public webserviceresultGeneric<string> MasterProjectPhases(string projectNumber)
        {
            webserviceresultGeneric<string> ws = new webserviceresultGeneric<string>();
            BPR.ProjectOpening.Services.DAL.DALProjects aDALService = new BPR.ProjectOpening.Services.DAL.DALProjects();
            List<string> returnValue = new List<string>();

            string level = "";

            projectNumber = projectNumber.Replace("'", "");

            for (int iterate = 0; iterate < 6; iterate++)
            {
                for (int count = 65; count < 91; count++)
                {
                    returnValue.Add(level + char.ConvertFromUtf32(count));
                }

                level = char.ConvertFromUtf32(65 + iterate);
            }

            foreach (string phasecode in aDALService.GetUsedPhase(projectNumber))
            {
                returnValue.Remove(phasecode);
            }

            if (returnValue.Count > 26)
            {
                for (int count = returnValue.Count - 1; count > 25; count--)
                {
                    returnValue.RemoveAt(count);
                }
            }

            ws.data = returnValue;
            ws.total = returnValue.Count;

            return (ws);
        }        

        // Règles
        //
        // 2015-06-03
        //     * Champ "PotentialServiceOfferValue": On accepte 0
        //     * Champ "ProjectSecurityModelId": Si je reçois 0, je vais récupérer l'ID de la sécurité de base du modèle sélectionné
        //
        public ProjectNumberCreation CreateProject(
            string ProjectTitle, 
            Boolean IsMaster, 
           // Boolean IsInterCo, 
           // int InterCoSeriesId,
            string MasterProjectNumber, 
            string PhaseCode, 
            string ClientId, 
            int ContractingCieId, 
            int BusinessUnitDivisionId,
            int MarketDivisionId, 
           // string ActivitySector, 
            //string RealisationSite, 
            //string OfficeId, 
            string Comments,
            string InitiatedBy, 
            string ProjectResp, 
            int PotentialValueId, 
            Boolean CreateBPRProject, //createFolder
            int ProjectModelId,
            int ProjectSecurityModelId, 
            int ProjectSelectionModelId, 
            int ProjectServerId)
        {            
            BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest = new BPR.ProjectOpening.Services.Model.ProjectNumberRequest();
            BPR.Data.BusinessModel.Model.Project aProject = new BPR.Data.BusinessModel.Model.Project();
            BPR.Data.BusinessModel.Model.Client aClient = new BPR.Data.BusinessModel.Model.Client();
            BPR.Vortex.Services.Common.Model.BusinessUnitDivision aBUDivision = new BPR.Vortex.Services.Common.Model.BusinessUnitDivision();
            BPR.Data.BusinessModel.Model.BusinessUnit aBUnit = new BPR.Data.BusinessModel.Model.BusinessUnit();
            BPR.Data.BusinessModel.Model.BusinessPlace aBPlace = new BPR.Data.BusinessModel.Model.BusinessPlace();
            BPR.Data.BusinessModel.Model.Employee anEmployee = new BPR.Data.BusinessModel.Model.Employee();
            ProjectModel aProjectModel = new ProjectModel();
            SelectionModel aSelectionModel = new SelectionModel();
            SecurityModel aSecurityModel = new SecurityModel();
            Server aServer = new Server();            
            ProjectNumberCreation aProjectNumberCreation = new ProjectNumberCreation();            
            string errorMessage = "";            

            try
            {
                mStartDateTime = DateTime.Now;
                mblnCreationSuccessful = false;
                mintLogType = StepWebServicesLogType.Information;

                // A utiliser seulement pour TESTS
                WriteParametersInLog(ProjectTitle, IsMaster, /*IsInterCo, InterCoSeriesId,*/
                    MasterProjectNumber, PhaseCode, ClientId, ContractingCieId, BusinessUnitDivisionId,
                    MarketDivisionId,/* ActivitySector, RealisationSite, OfficeId,*/ Comments,
                    InitiatedBy, ProjectResp, PotentialValueId, CreateBPRProject, ProjectModelId,
                    ProjectSecurityModelId, ProjectSelectionModelId, ProjectServerId);

                //prod
                // SaveEntryParameters(ProjectTitle, IsMaster, /*IsInterCo, InterCoSeriesId,*/
                //    MasterProjectNumber, PhaseCode, ClientId, ContractingCieId, BusinessUnitDivisionId,
                //    MarketDivisionId,/* ActivitySector, RealisationSite, OfficeId,*/ Comments,
                //    InitiatedBy, ProjectResp, PotentialValueId, CreateBPRProject, ProjectModelId,
                //    ProjectSecurityModelId, ProjectSelectionModelId, ProjectServerId);

                InitiatedBy = InitiatedBy.Trim();
                if (!InitiatorHasAccess(InitiatedBy))
                {                    
                    aProjectNumberCreation.success = false;                    
                    aProjectNumberCreation.error = "error: " + MessagesFR.ProjectNumberCreationAccessMissing;   
                    //-LG- 20151123 Ajout de ces 2 lignes:
                    mintLogType = StepWebServicesLogType.Error;
                    mstrError = "error: " + MessagesFR.ProjectNumberCreationAccessMissing; 
                }
                else
                {                                        
                    ProjectTitle = ProjectTitle.Trim();
                    if (ProjectTitle.Length == 0)
                    {                                                
                        aProjectNumberCreation.success = false;                        
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "ProjectTitle";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;                 
                        goto done;
                    }
                    aProject.Title = ProjectTitle;
                    pRequest.Project = aProject;

                    pRequest.IsMaster = IsMaster;
                    if (IsMaster)
                    {
                        MasterProjectNumber = "";
                        PhaseCode = "";
                    }
                    else
                    {
                        MasterProjectNumber=MasterProjectNumber.Trim();
                        PhaseCode=PhaseCode.Trim();
                        if (MasterProjectNumber.Length == 0 || PhaseCode.Length == 0)
                        {
                            aProjectNumberCreation.success = false;
                            aProjectNumberCreation.error = MessagesFR.RequiredField + "MasterProjectNumber / PhaseCode";
                            mintLogType = StepWebServicesLogType.Error;
                            mstrError = errorMessage;
                            goto done;
                        }
                    }
                    pRequest.MasterProjectNumber = MasterProjectNumber;
                    pRequest.PhaseCode = PhaseCode;

                    //pRequest.IsInterCo = IsInterCo;
                    //if (!IsInterCo)
                    //{
                    //    InterCoSeriesId = 0;
                    //}
                    //pRequest.InterCoSerieId = InterCoSeriesId;

                    ClientId = ClientId.Trim();
                    if (ClientId.Length == 0)
                    {
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "ClientId";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                        goto done;
                    }
                    aClient.Id = ClientId;
                    pRequest.GPCust = aClient;

                    if (ContractingCieId == 0)
                    {
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "ContractingCieId";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                        goto done;
                    }
                    //line: decommenter en prod
                   // pRequest.CieSuffix = mDALService.GetContractingCompaniesSuffix(ContractingCieId);
                    pRequest.CieSuffixId = ContractingCieId;

                    if (BusinessUnitDivisionId == 0)
                    {
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "BusinessUnitDivisionId";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                        goto done;
                    }
                    aBUDivision.Id = BusinessUnitDivisionId;
                    aBUnit.Id = BusinessUnitDivisionId.ToString();
                    pRequest.ProjectBusinessUnit = aBUnit;
                    pRequest.GenerationBusinessUnit = aBUDivision;

                    if (MarketDivisionId == 0)
                    {
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "MarketDivisionId";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                        goto done;
                    }
                    //line
                 //   pRequest.MarketDivision = mDALService.GetMarketDivisionsDescription(MarketDivisionId);

                   // pRequest.ActivitySector = ActivitySector.Trim();
                    //pRequest.RealisationSite = RealisationSite.Trim();

                    //aBPlace.Id = OfficeId.Trim();
                    //aBPlace.Name = mDALService.GetOfficeName(OfficeId);
                    pRequest.ProjectBusinessPlace = aBPlace;

                    pRequest.Comments = Comments.Trim();
                    
                    pRequest.InitiatedBy = InitiatedBy;

                    ProjectResp = ProjectResp.Trim();
                    if (ProjectResp.Length == 0)
                    {
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.error = MessagesFR.RequiredField + "ProjectResp";
                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                        goto done;
                    }
                    anEmployee.Id = ProjectResp.Trim();
                    pRequest.SubmitedBy = anEmployee;

                    pRequest.PotentialServiceOfferValue = PotentialValueId.ToString();

                    pRequest.CreateBPRProject = CreateBPRProject;
                
                    if (CreateBPRProject)
                    {                        
                        //-LG- Debut: A ENLEVER EN PROD
                      //  \\TTS349TEST03\prj_reg 
                          ProjectServerId = 68;
                       // ProjectServerId = 100;
                        //-LG- Fin: A ENLEVER EN PROD

                        aServer.Id = ProjectServerId;
                        pRequest.ServeurBPRProject = aServer;

                        aProjectModel.Id = ProjectModelId;
                        pRequest.ModelBPRProject = aProjectModel;

                        aSelectionModel.Id = ProjectSelectionModelId;
                        pRequest.SelectionModelBPRProject = aSelectionModel;
                        
                        if (ProjectSecurityModelId == 0)
                        {
                            ProjectSecurityModelId = ObtenirIDSecuriteDeBase(ProjectModelId.ToString());
                        }
                        aSecurityModel.Id = ProjectSecurityModelId;
                        pRequest.SecurityModelBPRProject = aSecurityModel;
                    }

                    //Line . a decommenter en prod
                    pRequest.StatusID = 1; // int.Parse(System.Configuration.ConfigurationManager.AppSettings["ProjectStatusId"]);

                    //TEST
                    //pRequest.Project.Number = "20079TT";
                    //mstrProjectNo = pRequest.Project.Number;
                    //mblnCreationSuccessful = true;
                    //aProjectNumberCreation.success = true;
                    //aProjectNumberCreation.projectNumber = mstrProjectNo;
                    //aProjectNumberCreation.error = "";

                    //if (pRequest.CreateBPRProject)
                    //{
                    //    if (!ProjectExistsInTTProjet(pRequest.Project.Number))
                    //    {
                    //        AddProjectInTTProjet(pRequest);
                    //    }
                    //}                       
                    //

                    //PROD
                    if (GetNextProjectNumber(ref pRequest, ref errorMessage))
                    {                        
                        AddProjectInSTEPAndTTProject(pRequest);

                        mblnCreationSuccessful = true;
                        aProjectNumberCreation.success = true;
                        aProjectNumberCreation.projectNumber = mstrProjectNo;
                    }
                    else
                    {
                        mblnCreationSuccessful = false;
                        aProjectNumberCreation.success = false;
                        aProjectNumberCreation.projectNumber = mstrProjectNo;
                        if (errorMessage.Length > 0)
                        {
                            aProjectNumberCreation.error = errorMessage;
                        }
                        else
                        {
                            aProjectNumberCreation.error = MessagesFR.UnableToGenerateAProjectNumber;
                        }

                        mintLogType = StepWebServicesLogType.Error;
                        mstrError = errorMessage;
                    }
                    //PROD                    

                done:                                       
                    // Traitement terminé
                    mEndDateTime = DateTime.Now;
                }
            }
            catch (Exception err)
            {
                mintLogType = StepWebServicesLogType.Error;                
                mblnCreationSuccessful = false; 

                aProjectNumberCreation.success = false;
                aProjectNumberCreation.projectNumber = mstrProjectNo;
                if (mstrError.Length > 0)
                {
                    mstrError = "error:" + mstrError + System.Environment.NewLine + " " + err.Message;
                }
                else
                {
                    mstrError = err.Message;                    
                }
                aProjectNumberCreation.error = "error:" + mstrError;
                //throw;
            }
            finally
            {                
                mEndDateTime = DateTime.Now;
                LogProjectCreationRequest();
            }

            return aProjectNumberCreation;
        }

        private void WriteParametersInLog(string ProjectTitle, Boolean IsMaster, /*Boolean IsInterCo, int InterCoSeriesId,*/
            string MasterProjectNumber, string PhaseCode, string ClientId, int ContractingCieId, int BusinessUnitDivisionId,
            int MarketDivisionId,/* string ActivitySector, string RealisationSite, string OfficeId,*/ string Comments,
            string InitiatedBy, string ProjectResp, int PotentialValueId, Boolean CreateBPRProject, int ProjectModelId,
            int ProjectSecurityModelId, int ProjectSelectionModelId, int ProjectServerId)
        {
            using (StreamWriter w = File.AppendText("C:\\Temp\\STEPWebServicesFred.txt"))
            {
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                                DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", "ProjectTitle: " + ProjectTitle);
                w.WriteLine("  :{0}", "IsMaster: " + IsMaster.ToString());
              //  w.WriteLine("  :{0}", "IsInterCo: " + IsInterCo.ToString());
              //  w.WriteLine("  :{0}", "InterCoSeriesId: " + InterCoSeriesId.ToString());
                w.WriteLine("  :{0}", "MasterProjectNumber: " + MasterProjectNumber);
                w.WriteLine("  :{0}", "PhaseCode: " + PhaseCode);
                w.WriteLine("  :{0}", "ClientId: " + ClientId);
                w.WriteLine("  :{0}", "ContractingCieId: " + ContractingCieId.ToString());
                w.WriteLine("  :{0}", "BusinessUnitDivisionId: " + BusinessUnitDivisionId.ToString());
                w.WriteLine("  :{0}", "MarketDivisionId: " + MarketDivisionId.ToString());
              //  w.WriteLine("  :{0}", "ActivitySector: " + ActivitySector);
              //  w.WriteLine("  :{0}", "RealisationSite: " + RealisationSite);
              //  w.WriteLine("  :{0}", "OfficeId: " + OfficeId);
                w.WriteLine("  :{0}", "Comments: " + Comments);
                w.WriteLine("  :{0}", "InitiatedBy: " + InitiatedBy);
                w.WriteLine("  :{0}", "ProjectResp: " + ProjectResp);
                w.WriteLine("  :{0}", "PotentialValueId: " + PotentialValueId.ToString());
                w.WriteLine("  :{0}", "CreateBPRProject: " + CreateBPRProject.ToString());
                w.WriteLine("  :{0}", "ProjectModelId: " + ProjectModelId.ToString());
                w.WriteLine("  :{0}", "ProjectSecurityModelId: " + ProjectSecurityModelId.ToString());
                w.WriteLine("  :{0}", "ProjectSelectionModelId: " + ProjectSelectionModelId.ToString());
                w.WriteLine("  :{0}", "ProjectServerId: " + ProjectServerId.ToString());
                w.WriteLine("-------------------------------");                
            }            
            
        }

        private void SaveEntryParameters(string ProjectTitle, Boolean IsMaster, /*Boolean IsInterCo, int InterCoSeriesId,*/
            string MasterProjectNumber, string PhaseCode, string ClientId, int ContractingCieId, int BusinessUnitDivisionId,
            int MarketDivisionId,/* string ActivitySector, string RealisationSite, string OfficeId,*/ string Comments,
            string InitiatedBy, string ProjectResp, int PotentialValueId, Boolean CreateBPRProject, int ProjectModelId,
            int ProjectSecurityModelId, int ProjectSelectionModelId, int ProjectServerId)
        {            
            mstrSubmitBy = InitiatedBy;
            mstrInformation = "Input parameters: " + '\r' + '\n' + ';' +
                "ProjectTitle: " + ProjectTitle + '\r' + '\n' + ';' +
                "IsMaster: " + IsMaster.ToString() + '\r' + '\n' + ';' +
              //  "IsInterCo: " + IsInterCo.ToString() + '\r' + '\n' + ';' +
               // "InterCoSeriesId: " + InterCoSeriesId.ToString() + '\r' + '\n' + ';' +
                "MasterProjectNumber: " + MasterProjectNumber + '\r' + '\n' + ';' +
                "PhaseCode: " + PhaseCode + '\r' + '\n' + ';' +
                "ClientId: " + ClientId + '\r' + '\n' + ';' +
                "ContractingCieId: " + ContractingCieId.ToString() + '\r' + '\n' + ';' +
                "BusinessUnitDivisionId: " + BusinessUnitDivisionId.ToString() + '\r' + '\n' + ';' +
                "MarketDivisionId: " + MarketDivisionId.ToString() + '\r' + '\n' + ';' +
               // "ActivitySector: " + ActivitySector + '\r' + '\n' + ';' +
               // "RealisationSite: " + RealisationSite + '\r' + '\n' + ';' +
               // "OfficeId: " + OfficeId + '\r' + '\n' + ';' +
                "Comments: " + Comments + '\r' + '\n' + ';' +
                "InitiatedBy: " + InitiatedBy + '\r' + '\n' + ';' +
                "ProjectResp: " + ProjectResp + '\r' + '\n' + ';' +
                "PotentialValueId: " + PotentialValueId.ToString() + '\r' + '\n' + ';' +
                "CreateBPRProject: " + CreateBPRProject.ToString() + '\r' + '\n' + ';' +
                "ProjectModelId: " + ProjectModelId.ToString() + '\r' + '\n' + ';' +
                "ProjectSecurityModelId: " + ProjectSecurityModelId.ToString() + '\r' + '\n' + ';' +
                "ProjectSelectionModelId: " + ProjectSelectionModelId.ToString() + '\r' + '\n' + ';' +
                "ProjectServerId: " + ProjectServerId.ToString();
        } 

        public void LogProjectCreationRequest()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["logProjectCreationRequest"] == "1")
            {
                int logType = (int)mintLogType;
                mDALService.LogOperation(logType, mstrSubmitBy, mStartDateTime, mEndDateTime, mblnCreationSuccessful, mstrInformation, mstrProjectNo, mstrError);
            }
        } 

        private Boolean InitiatorHasAccess(string initiatedBy)
        {
            //wsSecurity.Security ws = new wsSecurity.Security();
            Boolean hasAccess=false;
                        
            //System.Security.Principal.IIdentity anIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            
            //ICredentials pCredentials = System.Net.CredentialCache.DefaultCredentials;
            //ws.UseDefaultCredentials = true;
            //ws.Credentials = pCredentials;
            //hasAccess = ws.IsUserAllowedForObject(int.Parse(System.Configuration.ConfigurationManager.AppSettings["webservices.security.applicationid"]), initiatedBy, System.Configuration.ConfigurationManager.AppSettings["webservices.security.userobject.projectopening"]);

            hasAccess = true;
            return hasAccess;
        } 
       
        private Boolean GetNextProjectNumber(ref BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest, ref string errorMessage)
        {
            //DALProjectNumberBPR aDALService = new DALProjectNumberBPR();            
            Boolean retour = true;
            string projectNo = "";

            //if (!pRequest.IsMaster)
            //{
            //    // Valide qu'on a un numéro de projet maitre et une phase sinon générer une erreur
            //    if (pRequest.MasterProjectNumber.Length == 0 || pRequest.PhaseCode.Length == 0)
            //    {
            //        // soulever une erreur
            //        errorMessage = MessagesFR.MasterProjectAndPhaseRequired;
            //        retour = false;
            //    }
            //    else
            //    {
            //        if (MasterPhaseProjectExistsInSTEP(pRequest.MasterProjectNumber, pRequest.PhaseCode, ref projectNo))
            //        {
            //            errorMessage = string.Format(MessagesFR.MasterProjectAndPhaseAlreadyUsed, projectNo);
            //            retour = false;
            //        }
            //        else
            //        {
            //            if (pRequest.CieSuffix.Length > 0)
            //            {
            //                if (pRequest.MasterProjectNumber.ToUpper().Contains("TT"))
            //                {
            //                    projectNo = pRequest.MasterProjectNumber + pRequest.PhaseCode;
            //                }
            //                else
            //                {
            //                    projectNo = pRequest.MasterProjectNumber + pRequest.CieSuffix.ToUpper() + pRequest.PhaseCode;
            //                }
            //            }
            //            else
            //            {
            //                if (pRequest.MasterProjectNumber.ToUpper().Contains("TT"))
            //                {
            //                    projectNo = pRequest.MasterProjectNumber.ToUpper().Replace("TT", "") + pRequest.PhaseCode;
            //                }
            //                else
            //                {
            //                    projectNo = pRequest.MasterProjectNumber + pRequest.PhaseCode;
            //                }
            //            }
            //        }
            //    }


            //    // On vérifie si le projet existe dans la BD et si oui, on retourne false
            //    //if (ProjectExistsInSTEP(projectNo))
            //    //{                    
            //    //    errorMessage = string.Format(MessagesFR.ProjectFolderAlreadyExistsInSTEP, projectNo);
            //    //    retour = false;
            //    //}
            //}
            //else
            //{
            //    projectNo = aDALService.GetProjectNumber(pRequest);                
            //}

            //pRequest.Project.Number = projectNo;
            //mstrProjectNo = projectNo;

            //pour test seulement
            projectNo =  _TTEntity.usp_GetProjectNumberCounter().ToString();
            pRequest.Project.Number = projectNo;
            mstrProjectNo = projectNo;

          //  return result.GetValueOrDefault();
            return retour;
        }

        //private Boolean ProjectExistsInBPRProjet(BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest)
        //{
        //    Boolean blnPrjExists = false;

        //    return blnPrjExists;
        //}

        private void AddProjectInSTEPAndTTProject(BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest)
        {
            //AddProjectInSTEP(pRequest);

            //if (pRequest.CreateBPRProject)
            //{                
            //    if (!ProjectExistsInTTProjet(pRequest.Project.Number))
            //    {
                    AddProjectInTTProjet(pRequest);
            //    }                
            //}                                           
        }

        private BPR.Data.BPRProject.Model.Server GetServerInfo(string projectNo)
        {            
            return mDALService.GetServerInfo(projectNo);   
        }

        public void AddProjectInSTEP(BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest)//private
        {
            //getNextProjectNumberAttempts = 0; // reset            
            //pRequest.RequestID = CurrentBLLRequest().AddProjectNumberRequest(pRequest);

            BPR.ProjectOpening.Services.DAL.DALProjectNumberRequest aDALProject = new DALProjectNumberRequest();

            // J'enlève la valeur avant l'ajout dans la table Project et remet la valeur par la suite
            String oldProjectBusinessPlaceId = pRequest.ProjectBusinessPlace.Id;
            pRequest.ProjectBusinessPlace.Id = "";
            pRequest.RequestID = aDALProject.AddProjectNumberRequest(pRequest);
            pRequest.ProjectBusinessPlace.Id = oldProjectBusinessPlaceId;
            
            if (pRequest.Project.Number != null && pRequest.Project.Number != "")
            {
                aDALProject.AddProjectOtherInfo(pRequest, pRequest.ProjectBusinessPlace.Name, 0);                                
            }
        }

        private void AddProjectInTTProjet(BPR.ProjectOpening.Services.Model.ProjectNumberRequest pRequest)
        {
            try
            {
                ProjetInfo projetInfo = new ProjetInfo();
                projetInfo.Id = pRequest.Project.Number;

                RepertoireCollectionInfo rep = new RepertoireCollectionInfo();
                projetInfo.Repertoires = rep;
                //projetInfo.Repertoires = new RepertoireCollectionInfo();

                ModeleSecurite modeleSecurite = new ModeleSecurite();

                Serveur serveur = new Serveur();
                projetInfo.UtilisateurId = pRequest.InitiatedBy;
                projetInfo.ModeleSecuriteId = pRequest.SecurityModelBPRProject.Id;
                projetInfo.ModeleSecurite = modeleSecurite.Obtenir(pRequest.SecurityModelBPRProject.Id);

                foreach (RepertoireInfo repertoire in projetInfo.ModeleSecurite.Modele.Repertoires)
                {
                    this.AjouterRepertoireBase(repertoire, ref projetInfo);
                }
                projetInfo.ServeurId = pRequest.ServeurBPRProject.Id;
                //commenté pour test:
                projetInfo.Serveur = serveur.Obtenir(pRequest.ServeurBPRProject.Id);
                projetInfo.DateCreation = DateTime.Now;
                projetInfo.EtatProjetId = 1;
                ModeleSelection modeleSelection = new ModeleSelection();
                ModeleSelectionInfo modeleSelectionInfo = new ModeleSelectionInfo();
                //modeleSelectionInfo = modeleSelection.Obtenir(pRequest.SelectionModelBPRProject.Id);
                //projetInfo.Repertoires = modeleSelectionInfo.Repertoires;
                Projet projet = new Projet();
                projet.Ajouter(projetInfo);
            }
            catch (Exception err)
            {
                mstrError = "Erreur lors de la création de la structure de répertoires du projet " + mstrProjectNo + "." + System.Environment.NewLine + " " + err.Message;
                //throw ex;
            }
        }

        private Boolean MasterPhaseProjectExistsInSTEP(string MasterProjectNo, string phase, ref string projectNo)
        {
            return mDALService.MasterPhaseProjectExistsInSTEP(MasterProjectNo, phase, ref projectNo);
        }

        private Boolean ProjectExistsInTTProjet(string projectNo)
        {
            return false; // mDALService.ProjectExistsInTTProjet(projectNo);           
        }

        private void AjouterRepertoireBase(RepertoireInfo pRepertoireInfo, ref ProjetInfo pProjetInfo)
        {
            if (pRepertoireInfo.Base)
            {
                RepertoireInfo repertoireInfo = new RepertoireInfo();
                repertoireInfo.ModeleId = pRepertoireInfo.ModeleId;
                repertoireInfo.Id = pRepertoireInfo.Id;
                repertoireInfo.Nom = pRepertoireInfo.Nom;
                repertoireInfo.Description = pRepertoireInfo.Description;
                repertoireInfo.ParentId = pRepertoireInfo.ParentId;
                repertoireInfo.Parent = pRepertoireInfo.Parent;
                repertoireInfo.Dernier = pRepertoireInfo.Dernier;
                repertoireInfo.Base = pRepertoireInfo.Base;
                repertoireInfo.Racine = pRepertoireInfo.Racine;
                repertoireInfo.RacinePrincipale = pRepertoireInfo.RacinePrincipale;
                repertoireInfo.Repertoires = new RepertoireCollectionInfo();
                repertoireInfo.GroupeSecurite = pRepertoireInfo.GroupeSecurite;
                repertoireInfo.InfoBulle = pRepertoireInfo.InfoBulle;
                repertoireInfo.Actif = pRepertoireInfo.Actif;
                repertoireInfo.Special = pRepertoireInfo.Special;
                repertoireInfo.Special = pRepertoireInfo.Special;
                repertoireInfo.Special_sec = pRepertoireInfo.Special_sec;
                if (pRepertoireInfo.Parent == null)
                {
                    pProjetInfo.Repertoires.Add(repertoireInfo);
                }
                else
                {
                    pProjetInfo.Repertoires.Find(pRepertoireInfo.Parent.Id).Repertoires.Add(repertoireInfo);
                }
            }
            if (pRepertoireInfo.Repertoires != null)
            {

            }
            foreach (RepertoireInfo repertoire in pRepertoireInfo.Repertoires)
            {
                this.AjouterRepertoireBase(repertoire, ref pProjetInfo);
            }
        }

        private int ObtenirIDSecuriteDeBase(string modeleId)
        {         
            List<SecurityModel> aList;
            int intRecordCount = 0;
            int value = 0;

            aList = mDALService.GetProjectSecurityModels("base", "0", "1000", modeleId, ref intRecordCount);

            if (aList.Count > 0 )
            {
                value = aList[0].Id;
            }

            return value;
        }       
    }
}
