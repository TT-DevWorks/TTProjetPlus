using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Services;
using System.Web;
using TetraTech.TTProjetPlus.Models;



namespace TetraTech.TTProjetPlus.Controllers
{
    public class EnvVerifDiligenteController : Controller
    {
        private readonly EnvVerifDiligenteService _EnvVerifDiligenteService = new EnvVerifDiligenteService();

        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;

            var listeInfo = _EnvVerifDiligenteService.returnInfoProjet();                      

            return View(listeInfo);
        }

       


    }
}