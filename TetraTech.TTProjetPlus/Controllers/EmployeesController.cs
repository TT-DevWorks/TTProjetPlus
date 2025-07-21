using System.Web.Mvc; 
using TetraTech.TTProjetPlus.Services; 
namespace TetraTech.TTProjetPlus.Controllers
{
    public class EmployeesController : ControllerBase
    { 
        private readonly EmployeeService _EmployeeService = new EmployeeService(); 

        public PartialViewResult ReturnListEmployees()
        {
            var model = _EmployeeService.GetEmployeesForSearch(); 
            return PartialView(@"~\Views\Shared\Components\_EmployeesListPartial.cshtml", model);
        }
         
    }
}