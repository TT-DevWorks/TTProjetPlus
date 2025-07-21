using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Web;

namespace TetraTech.TTProjetPlus.Controllers
{
    [AdminAuthorize]
    public class AdminController : ControllerBase
    {
        private const string NotFoundErrorKey = "NotFound";
        private readonly AdminService _adminService = new AdminService();

        [HttpGet]
        public ActionResult Impersonate()
        {
            string EmployeeId = "";

            if (System.Diagnostics.Debugger.IsAttached)
            {
                //EmployeeId = "531853";
                EmployeeId = "533874";
            }
            else
            {
                //EmployeeId = "531853";
                EmployeeId = "533874";
            }

            return View(new Impersonate
            {
                EmployeeId = EmployeeId
            });
    }

        [HttpGet]
        public ActionResult Regress()
        {
            UserService.ImpersonatedUser = null;
            return Redirect("~/");//("~/")
        }

        [HttpPost]
        public ActionResult Impersonate(Impersonate impersonate)
        {
            if (ModelState.IsValid)
            {
                var directoryService = new DirectoryService();
                var principal = directoryService.FindUserByEmployeeId(impersonate.EmployeeId);

                if (principal != null)
                {
                    UserService.ImpersonatedUser = new TetraTechUser(principal);
                    return Redirect("~/");
                }
                else
                    ModelState.AddModelError(NotFoundErrorKey, ImpersonateResources.NotFoundErrorMessage);
            }

            return View(impersonate);
        }


       public ActionResult IndexAdmin (string menuVisible = "true")
        {
            var listClosedProjects = _adminService.returnClosedProjects();
            ViewBag.MenuVisible = menuVisible;
            return View(listClosedProjects);
        }

        public ActionResult submitProjectForClosingCancelling(string projectId)
        {
            var result = _adminService.CancelProjectClosing(projectId);
            return Json( result , JsonRequestBehavior.AllowGet);

        }
    }

   
}