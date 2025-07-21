using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Controllers;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class ListeOffresController : ControllerBase
    {
        private readonly ListeOffresProjetsService _ListeOffresProjetsService = new ListeOffresProjetsService();
        private readonly HomeService _HomeService = new HomeService();
        public static List<structureItem> ListeStatuts = new List<structureItem>();
        public static List<structureItem> ListeDocStatuts = new List<structureItem>();
        public static List<structureItem> ListeDocStatutsP = new List<structureItem>();
        public static List<Customer> ListeCustomers = new List<Customer>();
        public static List<structureItem> ListeCompanies = new List<structureItem>();
        public static List<structureItem> ListeUA = new List<structureItem>();
        public static List<structureItem> ListeORG = new List<structureItem>();
        public static List<ProjectManagerItem> ListeGestionnaires = new List<ProjectManagerItem>();
        public string sFilePath = HttpRuntime.AppDomainAppPath;
        // GET: ListeOffre
        public ActionResult Index(string menuVisible = "true", string userId = "")
        {
            ListeCompanies = _HomeService.GetCompanies();
            ListeStatuts = _ListeOffresProjetsService.returnStatusList();
            ListeDocStatuts = _ListeOffresProjetsService.returnDocStatusList();
            ListeCustomers = _HomeService.GetCustomersNumberName();
            ListeGestionnaires = _HomeService.GetProjectManagers();
            ViewBag.MenuVisible = menuVisible;
            ViewBag.UserIdIFrame = userId;
            Session["isProductionMode"] = UserService.isPRODEnvironment();

            return View();
        }

        public ActionResult IndexProjet(string menuVisible = "true")
        {
            ListeDocStatutsP = _ListeOffresProjetsService.returnDocStatusList();
            ViewBag.MenuVisible = menuVisible;
            return View();
        }


        public ActionResult returnOffersList(string userId, string visible, string type = "offer", string active = "yes")
        {
            ViewBag.menuVisible = visible;
            ListOfOffers model = new ListOfOffers();
            model.OffersList = _ListeOffresProjetsService.returnListOfOffersForUser(userId, type, active);
            if (type == "offer")
            {
                return PartialView("ListeOffresPartial", model);
            }
            else return PartialView("ListeProjetsPartial", model);
        }

        public ActionResult returnDetailsForProjetInModal(string Id)
        {
            OSProjectModel model = new OSProjectModel();
            model = _ListeOffresProjetsService.returnOfferProject(Id);
            return PartialView("DetailsProjet", model);
        }

        public ActionResult returnDetailsForOfferInModal(string Id)
        {
            OSProjectModel model = new OSProjectModel();
            model = _ListeOffresProjetsService.returnOfferProject(Id);

            return PartialView("DetailsOffre", model);
        }


        public ActionResult submitAction(string os_project_id, string title, string customer, string company, string businessUnit,
            string org, string projectMgr, string programMgr, string finalClient, string approximateAmount,
            string status, string statusDoc, string dateArchivage, string dateRepport, string initiatedBy, string comments, string project_number, string NamePM, string EmailPM, Boolean prjFolderNeedsUpdate)
        {

            var update = _ListeOffresProjetsService.updateOSProjectTable("OS", os_project_id, title, customer, company, businessUnit,
               org, projectMgr, programMgr, finalClient, approximateAmount,
              status, statusDoc, dateArchivage, dateRepport, initiatedBy, comments, NamePM, EmailPM);
            if (update == "success")
            {
                if (prjFolderNeedsUpdate)
                {
                    HomeController ctrlHomeController = new HomeController();
                    if ((Session["isProductionMode"] == null ? 1 : (int)Session["isProductionMode"]) == 1)
                    {                        
                        ctrlHomeController.addSecurityOnFolders(project_number, "", 1, "submitAction", "ListeOffres");
                    }
                    else
                    {
                        // Si c'est un projet sur le serveur test, on sécurise
                        if (ctrlHomeController.isProjectOnTestServer(project_number))
                        {
                            ctrlHomeController.addSecurityOnFolders(project_number, "", 1, "submitAction", "ListeOffres");
                        }
                    }
                }

                return Json("succes", JsonRequestBehavior.AllowGet);
            }
            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        public ActionResult submitActionForProject(string os_project_id, string dateArchivage, string dateRepport, string statusDoc, string comments, string NamePM, string EmailPM)
        {

            var update = _ListeOffresProjetsService.updateOSProjectTable("PR", os_project_id, null, null, null, null, null,
             null, null, null, null,
             null, statusDoc, dateArchivage, dateRepport, null, comments, NamePM, EmailPM);
            if (update == "success")
            {
                return Json("succes", JsonRequestBehavior.AllowGet);
            }
            return Json("fail", JsonRequestBehavior.AllowGet);
        }



        public ActionResult DownloadEmail(string Project_Number, string Project_Description,
            string Customer_Name, string Company, string Acct_Group_Name, string Organisation,
            string Initiated_By_Name, string GestionnaireProjet, string Program_Mgr_Name,
            string SubmittedDate, string Comments, string Project_Mgr_Number)
        {
            var message = new MailMessage();
            message.Headers.Add("X-Unsent", "1");
            var emailFrom = _ListeOffresProjetsService.ReturnUserEmail(UserService.CurrentUser.EmployeeId);
            var emailTo =   _ListeOffresProjetsService.ReturnUserEmail(Project_Mgr_Number);

            if (emailTo == null || emailTo == "")
            {
                emailTo = emailFrom;
            }

            message.From = new MailAddress(emailFrom);
            message.To.Add(emailTo);
            message.Subject = "Projet " + Project_Number;
            message.Body = "";
            message.Body += "Informations standards (compléter les informations et expédier aux personnes concernées): " + "\n";
            message.Body += "Numéro de projet: " + Project_Number + "\n";
            message.Body += "Titre du projet : " + Project_Description + "\n";
            message.Body += "Client # : " + Customer_Name + "\n";
            message.Body += "Compagnie:" + Company + "\n";
            message.Body += "Unité d'affaires : " + Acct_Group_Name + "\n";
            message.Body += "Organisation : " + Organisation + "\n"; ;
            message.Body += "Initié par : " + Initiated_By_Name + "\n";
            message.Body += "Références : " + Comments.Replace("9k9q9w9 ", "\n").Replace("9k9q9w9 ", "\r") + "\n";
            message.Body += "Gestionnaire de projet : " + GestionnaireProjet + "\n";
            message.Body += "Gestionnaire de programme  : " + Program_Mgr_Name + "\n";
            message.Body += "Attribué le : " + SubmittedDate + "\n"; ;
            message.Body += "---------------------------------------------" + "\n";
            message.Body += "Informations supplémentaires facultatives selon la politique de chaque unité d’affaires" + "\n";
            message.Body += "Résultat de l’analyse de risque :" + "\n"; ;
            message.Body += "Informations requises si le niveau de risque est au moins jaune, si le client est nouveau ou si le montant des honoraires estimés risque d’être supérieur à 100 000$:" + "\n"; ;
            message.Body += "Mandat :" + "\n"; message.Body += "Type de service (horaire, forfait, etc.) :" + "\n";
            message.Body += "Montant des honoraires estimés approximatif :" + "\n";
            message.Body += "Potentiel de marge brute :" + "\n"; ;
            message.Body += "Niveau de probabilité d’obtention du contrat :" + "\n";
            message.Body += "Description des risques majeurs du projet :" + "\n";
            message.Body += "Stratégie d’entreprise (motifs pour lesquels on devrait faire cette offre) :" + "\n";
            message.Body += "Notes et recommandations : ";
            message.BodyTransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
            message.BodyEncoding = Encoding.UTF8;
            using (var client = new SmtpClient())
            {
                var id = Guid.NewGuid();

                var tempFolder = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name);

                tempFolder = Path.Combine(tempFolder, "MailMessageToEMLTemp");

                // create a temp folder to hold just this .eml file so that we can find it easily.
                tempFolder = Path.Combine(tempFolder, id.ToString());

                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = tempFolder;
                client.Send(message);

                // tempFolder should contain 1 eml file

                var filePath = Directory.GetFiles(tempFolder).Single();

                // stream out the contents - don't need to dispose because File() does it for you
                var fs = new FileStream(filePath, FileMode.Open);

                return File(fs, "application/vnd.ms-outlook", "TTProjetPlusCourriel.eml");
            }
        }

        public ActionResult DownloadEmail2(string Project_Number, string lang)
        {
            // On récupère le ID
            string OSProjectID = _ListeOffresProjetsService.returnOSProjectId(Project_Number);
            //string lang = Session["lang"].ToString();            

            // On récupère donnée de l'offre
            OSProjectModel model = new OSProjectModel();
            model = _ListeOffresProjetsService.returnOfferProject(OSProjectID);

            var message = new MailMessage();
            message.Headers.Add("X-Unsent", "1");
            var emailFrom = _ListeOffresProjetsService.ReturnUserEmail(UserService.CurrentUser.EmployeeId);
            var emailTo = _ListeOffresProjetsService.ReturnUserEmail(model.Project_Mgr_Number);

            if (emailTo == null || emailTo == "")
            {
                emailTo = emailFrom;
            }

            message.From = new MailAddress(emailFrom);
            message.To.Add(emailTo);
            if(lang == "fr")
            {
                message.Subject = "Projet " + Project_Number;
                message.Body = "";
                message.Body += "Informations standards (compléter les informations et expédier aux personnes concernées): " + "\n";
                message.Body += "Numéro de projet: " + Project_Number + "\n";
                message.Body += "Titre du projet: " + model.Project_Description + "\n";
                message.Body += "Client #: " + model.Customer_Name + "\n";
                message.Body += "Compagnie: " + model.Company + "\n";
                message.Body += "Unité d'affaires: " + model.Acct_Group_Name + "\n";
                message.Body += "Organisation: " + model.Organisation + "\n"; ;
                message.Body += "Initié par: " + model.Initiated_By_Name + "\n";
                message.Body += "Références: " + model.Comments.Replace("9k9q9w9 ", "\n").Replace("9k9q9w9 ", "\r") + "\n";
                message.Body += "Gestionnaire de projet: " + model.Project_Mgr_Name + "\n";
                message.Body += "Gestionnaire de programme: " + model.Program_Mgr_Name + "\n";
                message.Body += "Attribué le: " + model.submittedDate + "\n"; ;
                message.Body += "---------------------------------------------" + "\n";
                message.Body += "Informations supplémentaires facultatives selon la politique de chaque unité d’affaires" + "\n";
                message.Body += "Résultat de l’analyse de risque: " + "\n"; ;
                message.Body += "Informations requises si le niveau de risque est au moins jaune, si le client est nouveau ou si le montant des honoraires estimés risque d’être supérieur à 100 000$: " + "\n"; ;
                message.Body += "Mandat: " + "\n"; message.Body += "Type de service (horaire, forfait, etc.) :" + "\n";
                message.Body += "Montant des honoraires estimés approximatif: " + "\n";
                message.Body += "Potentiel de marge brute: " + "\n"; ;
                message.Body += "Niveau de probabilité d’obtention du contrat: " + "\n";
                message.Body += "Description des risques majeurs du projet: " + "\n";
                message.Body += "Stratégie d’entreprise (motifs pour lesquels on devrait faire cette offre): " + "\n";
                message.Body += "Notes et recommandations: ";
            }
            else if(lang == "en")
            {
                message.Subject = "Project " + Project_Number;
                message.Body = "";
                message.Body += "Standard information (complete the information and send to the persons concerned): " + "\n";
                message.Body += "Project number: " + Project_Number + "\n";
                message.Body += "Project title: " + model.Project_Description + "\n";
                message.Body += "Client #: " + model.Customer_Name + "\n";
                message.Body += "Company: " + model.Company + "\n";
                message.Body += "Business unit: " + model.Acct_Group_Name + "\n";
                message.Body += "Organization: " + model.Organisation + "\n"; ;
                message.Body += "Initiated by: " + model.Initiated_By_Name + "\n";
                message.Body += "References: " + model.Comments.Replace("9k9q9w9 ", "\n").Replace("9k9q9w9 ", "\r") + "\n";
                message.Body += "Project Manager: " + model.Project_Mgr_Name + "\n";
                message.Body += "Program manager: " + model.Program_Mgr_Name + "\n";
                message.Body += "Attributed on: " + model.submittedDate + "\n"; ;
                message.Body += "---------------------------------------------" + "\n";
                message.Body += "Optional additional information according to the policy of each business unit" + "\n";
                message.Body += "Result of the risk analysis: " + "\n"; ;
                message.Body += "Information required if the level of risk is at least yellow, if the client is new or if the amount of estimated fees is likely to be greater than $100 000: " + "\n"; ;
                message.Body += "Mandate: " + "\n";
                message.Body += "Type of service (schedule, package, etc.): " + "\n";
                message.Body += "Approximate estimated fee amount: " + "\n";
                message.Body += "Gross margin potential: " + "\n"; ;
                message.Body += "Level of probability of obtaining the contract: " + "\n";
                message.Body += "Description of the major risks of the project: " + "\n";
                message.Body += "Business strategy (reasons why we should make this offer): " + "\n";
                message.Body += "Notes and recommendations: ";
            }

            message.BodyTransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
            message.BodyEncoding = Encoding.UTF8;
            using (var client = new SmtpClient())
            {
                var id = Guid.NewGuid();

                var tempFolder = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name);

                tempFolder = Path.Combine(tempFolder, "MailMessageToEMLTemp");

                // create a temp folder to hold just this .eml file so that we can find it easily.
                tempFolder = Path.Combine(tempFolder, id.ToString());

                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = tempFolder;
                client.Send(message);

                // tempFolder should contain 1 eml file

                var filePath = Directory.GetFiles(tempFolder).Single();

                // stream out the contents - don't need to dispose because File() does it for you
                var fs = new FileStream(filePath, FileMode.Open);

                return File(fs, "application/vnd.ms-outlook", "TTProjetPlusCourriel.eml");
            }
        }

        public ActionResult createBatchFile(string folderLocation)
        {
            try
                {
                //    if (System.IO.File.Exists(Path.Combine(sFilePath, "TTOpenFolder.bat")))
                //{
                //        // If file found, delete it    
                //        System.IO.File.Delete(Path.Combine(sFilePath, "TTOpenFolder.bat"));
                //}

                //var sFileName = "TTOpenFolder.bat";
                var sFileName = "TTProjetPlusRepertoire.bat";

                // Set a variable to the Documents path.
                //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


                using (StreamWriter sw = new StreamWriter(Path.Combine(sFilePath, sFileName)))
                    {
                        //Nouveau fichier BAT
                        sw.WriteLine("@echo off");
                        sw.WriteLine("%SystemRoot%\\explorer.exe "+ folderLocation);
                        sw.WriteLine(":End");
                        sw.WriteLine("");
                        sw.Close();

                    }

                    string contentType = "text/plain";
                    return File(sFilePath + "\\" + sFileName, contentType, "TTProjetPlusRepertoire.bat");

                }
                catch (Exception e)
                {
                    return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
                }
            }

        public ActionResult checkIfDirectoryOnDisk(string path)
        {
            if (Directory.Exists(path))
            {
                return Json(new { exist = "yes" }, JsonRequestBehavior.AllowGet);



            }
            return Json(new { exist = "no" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult isProjectSecurityNeedsUpdate(string proId)
        {
            if (_ListeOffresProjetsService.isProjectSecurityNeedsUpdate(proId)            )
            {
                return Json(new { needsUpdate = "yes" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { needsUpdate = "no" }, JsonRequestBehavior.AllowGet);
        }



    }

   

}