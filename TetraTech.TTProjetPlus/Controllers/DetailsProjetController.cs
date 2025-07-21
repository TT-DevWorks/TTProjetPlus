using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class DetailsProjetController : ControllerBase
    {
        private readonly DashBoardService _DashboardService = new DashBoardService();

        // GET: DetailsProjet
        public ActionResult Index(string projectID = "")
        {
            ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();
            return View();
        }
    }
}