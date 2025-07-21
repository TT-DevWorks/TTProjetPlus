using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Data;


namespace TetraTech.TTProjetPlus.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer

        public CustomerService _CustomerService = new CustomerService();
        

        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            CustomerModel model = new CustomerModel();
            model.ListCustomerModel  = _CustomerService.returnListeCustomers();            
            return View(model);


        }
             
        public ActionResult InsertInfos(string CustomerNumber, string CustomerName, string CreatedBy, string Reference)
        {
            try
            {
                var result = _CustomerService.InsertIntoCustomerTable(CustomerNumber, CustomerName, CreatedBy, Reference);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }


        public ActionResult returnExistCustomerNumber(string CustomerNumber)
        {
            
            var isCustomerNumberExist = _CustomerService.returnExistCustomerNumber(CustomerNumber);

            if (isCustomerNumberExist == true)
            {
                return Json(new { result = "exist" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { result = "not exist"}, JsonRequestBehavior.AllowGet); ;

        }

    }
}