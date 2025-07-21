using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class DashboardController : ControllerBase
    {
        private readonly DashBoardService _DashboardService = new DashBoardService();
        private readonly HomeService _HomeService = new HomeService();
        public static List<structureItem> ListeProjetsDB = new List<structureItem>();
        public static List<structureItem> ListeModeleNameDB = new List<structureItem>();


        // GET: Dashboard
        public ActionResult Index()
        {
            ListeProjetsDB = _HomeService.getListWSDR();
            ListeModeleNameDB = _HomeService.GetModeleName();
            ViewData["CurrentPeriodWeekDate"] = _DashboardService.GetCurrentPeriodWeekDate();
            return View();
        }

    }
}