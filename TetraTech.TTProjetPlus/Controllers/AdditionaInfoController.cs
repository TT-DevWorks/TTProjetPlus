using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Services;
using System.Web;


namespace TetraTech.TTProjetPlus.Controllers
{

    public class AdditionaInfoController : Controller
    {
         private readonly AdditionalInfoService _AdditionalInfoService = new AdditionalInfoService();
        // GET: AdditionaInfo
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
          
            return View();
        }

    public ActionResult displayInfo(string typeInfo= "BOTH")
    {              
            HttpContext context = System.Web.HttpContext.Current;
            var  lang = context.Session["lang"].ToString();
            ViewBag.lang = lang;
            var listeInfo = _AdditionalInfoService.returnInfo(typeInfo);

            return PartialView("_PartialAdditionalInfo", listeInfo);
    }


    }


}