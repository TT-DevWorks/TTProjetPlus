using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class TDT_PSWListe_BDController : Controller
    {
        private readonly TDT_PSWPermissionService _TDT_PSW = new TDT_PSWPermissionService();
        // GET: TDT_PSWListe_BD
        public ActionResult Index(string menuVisible = "true")
        {
            var listePSW = _TDT_PSW.getPSWList();
            ViewBag.MenuVisible = menuVisible;

            return View(listePSW);
        }


        public ActionResult insertIntoTable(string moyen, string partiel, string complet, string dateChgt)
        {
            var success = _TDT_PSW.insertIntoTablePSW(moyen, partiel, complet, dateChgt);
            return Json(success, JsonRequestBehavior.AllowGet);


        }
    }
}
