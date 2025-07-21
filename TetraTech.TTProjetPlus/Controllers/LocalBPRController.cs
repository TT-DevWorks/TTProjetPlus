using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;





namespace TetraTech.TTProjetPlus.Controllers
{
    public class LocalBPRController : Controller
    {
        // GET: LocalBPR
        
        private readonly LocalBPRService _LocalBPRService = new LocalBPRService();        

            
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            LocalBPRModel model = new LocalBPRModel();
            model.ListADTTLocalBPR = _LocalBPRService.GetListeADLocalBPR();
            return View(model);
        }

 
    }
}