using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;  
namespace TetraTech.TTProjetPlus.Controllers
{
    public class ManageADGroupMembersController : ControllerBase
    {
        private readonly ManageADGroupMembersService _ManageAdGroupService = new ManageADGroupMembersService(); 

        public ActionResult Index(string menuVisible = "true")
        { 
            ManageADGroupMembersViewModel result = new ManageADGroupMembersViewModel(CurrentBaseViewModel)
            {
                ADGroupsList = _ManageAdGroupService.GetSecurityDirectoryADGroupForAdmin(CurrentUser.TTAccount),
                Details = new ManageADGroupMembersDetails() 
            };

            ViewBag.MenuVisible = menuVisible;
            return View(result);
        }

        public PartialViewResult GetDetailsForSelectedGroup(string selectedGroup)
        { 

            ManageADGroupMembersDetails model = _ManageAdGroupService.GetDetailsForSelectedGroup(selectedGroup);

            return PartialView("_DetailsGroupMembers", model);
        } 

        public PartialViewResult EmployeeADGroup(string selectedGroup, OperationStatus operationStatus)
        {
            return PartialView("_AddEmployeeToGroup", new EmployeeADGroup { SelectedGroup=selectedGroup, OperationStatus= operationStatus });
        }

        [HttpPost]
        public PartialViewResult AddEmployeeToADGroup(EmployeeADGroup data)
        {
            OperationStatus operationStatus = new OperationStatus { Status = false }; 

            if (ModelState.IsValid)
            {
                try
                {
                    operationStatus = _ManageAdGroupService.AddEmployeeToADGroup(data.SelectedEmployeeNumber, data.SelectedGroup);
                    if (operationStatus.Status)
                    {
                        return PartialView("_AddEmployeeToGroup", new EmployeeADGroup { SelectedGroup = data.SelectedGroup, OperationStatus = operationStatus });
                    }
                   else
                        ModelState.AddModelError("Error", Resources.TTProjetPlusResource.ErrorMessageOther);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message); 
                }

            }            

            data.OperationStatus = operationStatus;

            return PartialView("_AddEmployeeToGroup", data);
        }
         

        [HttpPost]
        public ActionResult RemoveEmployeeFromADGroup(EmployeeADGroup data)
        { 
            OperationStatus operationStatus = new OperationStatus { Status = false };

            if (ModelState.IsValid)
            {
                try
                { 
                    operationStatus = _ManageAdGroupService.RemoveEmployeeFromADGroup(data.SelectedEmployeeNumber, data.SelectedGroup);

                    if (!operationStatus.Status) 
                        ModelState.AddModelError("Error", Resources.TTProjetPlusResource.ErrorMessageOther);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }

            }
             
            return Json(new {selectedGroup = data.SelectedGroup, success = operationStatus.Status,
                successmessage = operationStatus.Message,
                errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)  }, JsonRequestBehavior.AllowGet); 
        }

    }
}