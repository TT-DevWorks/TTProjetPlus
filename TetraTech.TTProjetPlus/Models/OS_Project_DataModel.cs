using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq; 
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class OS_Project_DataModel
    { 

        public OS_Project OS_Project { get; set; } = new OS_Project();

        public int OS_ProjectID => OS_Project.OS_Project_ID;

        public string Acct_Center_Name { get; set; }

        public string Acct_Group_Name { get; set; }  
         
        public string StatusName { get; set; }
         
        public string StatusDocName { get; set; }

        public EmployeeBase ProjectManager { get; set; } 

        public EmployeeBase BillingSpecialist { get; set; } 

        public EmployeeBase InitiatedBy { get; set; } 

        public EmployeeBase ProgramManager { get; set; } 

    }  

    public class TLinxProjectModelBase
    { 
        public int ProjectID { get; set; }
        public string Project_Number { get; set; }
        public DateTime Project_start_date { get; set; }
        public DateTime Project_end_date { get; set; }

        public string ProjectMgrNumber { get; set; }
        public string ProjectMgrName { get; set; }
        public string ProgramMgrNumber { get; set; }
        public string ProgramMgrName { get; set; } 

        public string BillSpecialistName { get; set; }
        public string ContractAdministratorName { get; set; }

    }

   
}