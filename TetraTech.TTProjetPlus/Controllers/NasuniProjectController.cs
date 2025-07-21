using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class NasuniProjectController : Controller
    {
        // GET: NasuniProject

        public NasuniProjectService _NasuniProjectService = new NasuniProjectService();
        //NasuniProjectModel model = new NasuniProjectModel();

        public ActionResult Index(string menuVisible = "true", string activeproject = "yes" )
        {
            ViewBag.MenuVisible = menuVisible;
            var model = _NasuniProjectService.returnListeProjectNasuniAll(activeproject);            
            return View(model);
        }


    }
}