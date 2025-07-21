using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class NewClientController : ControllerBase
    {
        // GET: NewClient
        public ActionResult Index()
        {
            return View();
        }

        public bool addNewClient(string contact, string adresse, string ville,
            string codePostal, string province , string country, string telephone, string poste )
        {
            return true;
        }

    }
}