using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class SpecialDocsByCustomerController : Controller
    {
        // GET: SpecialDocsByCustomer
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            return View();
        }
    }
}