using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class SearchEmployeesViewModel
    { 
        public string ModalDescription { get; set; }
        public string ResultIdForName { get; set; }
        public string ResultIdForNumber { get; set; }
        public bool Preload { get; set; } = false;
    }

    public class EmployeeItem
    {
        public string Full_Name { get; set; }
        public string Employee_Number { get; set; }
        public string Org { get; set; }
        public string Office { get; set; }
        public string Email { get; set; }
    }
}