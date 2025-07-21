using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class OpenProjectController : Controller
    {
        OpenProjectService _OPService = new OpenProjectService();
        // GET: OpenProject
        public static List<structureItem> ListeEmployeeActif_Inactive = new List<structureItem>();
        public static List<structureItem> ListeCurrencies = new List<structureItem>();
        public static List<structureItem> ListeDistribution = new List<structureItem>();
        public static List<structureItem> ListeContractType = new List<structureItem>();
        public static List<structureItem> ListeAcctCenters = new List<structureItem>();
        public static List<structureItem> ListeOfficeAddress = new List<structureItem>();
        public static List<structureItem> ListeWorkLocations = new List<structureItem>();
        public static List<structureItem> ListePrograms = new List<structureItem>();
        public static List<structureItem> ListeWorkType = new List<structureItem>();
        public  static List<XX_PROJECT_TASKS_WBS> ListeTasksForProject = new List<XX_PROJECT_TASKS_WBS>();//changer a session variable

        public ActionResult Index(string project_number, string client_number, string origin, string menuVisible = "true")
        {

            //var DeserializedModel = JsonConvert.DeserializeObject<OSProjectModel>(model);
            ViewBag.MenuVisible = menuVisible;

            ViewBag.project_number = project_number;

            var initial_client_input = _OPService.returnProjet(client_number);
            ViewBag.client = initial_client_input;
            ViewBag.origin = origin;

            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ListeEmployeeActif_Inactive = renderList("GP");
            ListeCurrencies = renderList("Currency");
            ListeDistribution = renderList("Distribution");
            ListeContractType = renderList("ContractType");
            ListeAcctCenters = renderList("AcctCenters");
            ListeOfficeAddress = renderList("OfficeAddress");
            ListeWorkLocations = renderList("WorkLocations");
            ListePrograms = renderList("Programs");
            ListeWorkType = renderList("WorkType");
            ListeTasksForProject = _OPService.returnTasksList("715-21171TT");//project_number
            List<int?> listParents = new List<int?>();
            foreach(XX_PROJECT_TASKS_WBS item in ListeTasksForProject)
            {
                listParents.Add(item.parent_task_id);
            }
            foreach(XX_PROJECT_TASKS_WBS item in ListeTasksForProject)
            {
                if (listParents.Contains(item.task_id))
                {
                    item.task_description = "parent";
                }
            }

            return View();
        }

        public JsonResult returnOSList(int status)
        {
            var list = _OPService.GetOffersList(status);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult returnVersionsList(string project_number)
        {
            List<int?> list1 = new List<int?>();
             list1 = _OPService.returnVersionsList(project_number);
            if (list1.Count == 0)
            {
                List<int?> list2 = new List<int?>() { -1};
                return Json(list2, JsonRequestBehavior.AllowGet);
            }
           else return Json(list1, JsonRequestBehavior.AllowGet);
        }


        public List<structureItem> renderList(string methodName)
        {
            List<structureItem> liste = new List<structureItem>();

            switch (methodName)
            {
                case "GP":
                    liste = _OPService.getListeEmployeeActif_Inactive();
                    break;
                case "Currency":
                    liste = _OPService.getCurrencies();
                    break;
                case "Distribution":
                    liste = _OPService.getListeDistribution();
                    break;
                case "ContractType":
                    liste = _OPService.getContractType();
                    break;
                case "AcctCenters":
                    liste = _OPService.getAcctCenters();
                    break;
                case "OfficeAddress":
                    liste = _OPService.getOfficeAddress();
                    break;
                case "WorkLocations":
                    liste = _OPService.getWorkLocations();
                    break;
                case "Programs":
                    liste = _OPService.getPrograms();
                    break;
                case "WorkType":
                    liste = _OPService.getWorkType();
                    break;

            }
            return liste;
        }
       public void saveOpenProject(string project_number, string client, string clientFinal, string billingAddress, string shippingAddress, string contactClient, string phone,
           string extension, string invoiceEmailAddress1, string invoiceEmailAddress2, string contractNumber, string billingLanguage, string prefCom,
           string projectName, string projectDescription, string programMgrNumber, string billSpecialistNumber, string otherKM01, string otherKM02, string requiredBy, 
           string currency, string date, string startDate, string endDate, string distributionRuleId, string ContractTypeId, string acctCenterId, string officeAddressId, 
           string workLocationID, string programId, string workTypeId, string budegtCost, string timesheet, string timesheetCom, string expenseReport, string expenseReportOriginal, 
           string supplierInv, string supplierInvOriginal, string writtenApproval, string verbalApproval, string noneApproval, string interCompany, string tar, 
           string multipleOrderPurc, string consortium, string multipleClient)
         { 






           
             
        }
    }

   
}