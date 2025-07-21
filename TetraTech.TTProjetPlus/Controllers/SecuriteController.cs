using System.Collections.Generic;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class SecuriteController : ControllerBase
    {
        public static List<structureItem> ListeResponsableProjet = new List<structureItem>();
        private readonly HomeService _HomeService = new HomeService();
        private readonly SecuriteService _SecuriteService = new SecuriteService();


        // GET: Securite
        public ActionResult Index(string menuVisible = "true")
        {
            ListeResponsableProjet = _HomeService.GetAdministratorsList("14");
            ViewBag.MenuVisible = menuVisible;
            ViewBag.EmployeeArray = EmployeeArrayFromEntity();
            Session["HasAccessToSecurite"] = _HomeService.HasAccessToSecurite(UserService.CurrentUser.EmployeeId);
            return View();
        }
        public ActionResult returnTableListResponsibles()
        {
            ListeResponsableProjet = _HomeService.GetAdministratorsList("14");
            return PartialView("ResponsibleProjectListPartial");
        }


        public ActionResult givePermissionToEmployee(string employee)
        {
            var updated = _SecuriteService.insertIntoSecurityTable(employee);

            return Json(new { data = updated }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult removeResponsible(string employeeId)
        {
            var removed = _SecuriteService.removeResponsible(employeeId);
            return Json(new { data = removed }, JsonRequestBehavior.AllowGet);
        }

        public string[] EmployeeArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _SecuriteService.GetEmployeesList();

            return rtnlist.ToArray();
        }

    }
}