using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class OpenProjectModel
    {   public int OpeningProject_Id { get; set; }
        public int? OS_Project_Id { get; set; }
        public int? Version_Id { get; set; }
        public byte? State { get; set; }

        //page1: 
        public string Project_Number { get; set; }
        public string ClientName { get; set; }
        public string Customer_Number { get; set; }
        public string Billing_Address { get; set; }
        public string Contact_Client { get; set; } //Nom de la personne responsable
        public string Phone_Client { get; set; }
        public string Phone_Extension { get; set; }
        public string Shipping_Address { get; set; }
        public string End_Client_Name { get; set; }
        public string Invoice_Email_Address { get; set; }
        public string Invoice_Email_Address2 { get; set; }
        public string Contract_Number { get; set; }


        //page2 
        public byte? Billing_Language { get; set; }
        public bool? Billing_Format_Paper { get; set; }
        public bool? Billing_Format_Electronic { get; set; }
        public string Project_Name { get; set; }
        public string Project_Description { get; set; }
        public string Project_Mgr_Number { get; set; }
        public string Program_Mgr_Number { get; set; }
        public string OtherKM_01 { get; set; }
        public string OtherKM_02 { get; set; }
        public string Initiated_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Currency_Id { get; set; }
        public DateTime? Project_Start_Date { get; set; }
        public DateTime? Project_End_Date { get; set; }

        //page3
        public int? Distribution_Rule_Id { get; set; }
        public int? Contract_Type_Id { get; set; }
        public int? Acct_Center_Id { get; set; }
        public int? Office_Address_Id { get; set; }
        public int? Work_Location_ID { get; set; }
        public int? Program_Id { get; set; }
        public int? Work_Type_Id { get; set; }
        public bool? Timesheet { get; set; }
        public bool? Expense_Report { get; set; }
        public bool? Invoice_Supplier { get; set; }
        public bool? Mandatory_Timesheet_Comment { get; set; }

        //page4
        public bool? Client_Written_Approval { get; set; }
        public string Client_Written_Approval_Desc { get; set; }
        public bool? Client_Verbal_Approval { get; set; }
        public bool? Interco { get; set; }
        public bool? War { get; set; }
        public bool? Multi_PO { get; set; }
        public bool? Joint_Venture { get; set; }
        public bool? Multi_Client { get; set; }


        //
        public string Bill_Specialist_Number { get; set; }
        public DateTime? Last_Modification_Date { get; set; }
        public string Modified_By { get; set; }
        public bool? Expense_Report_Original { get; set; }
        public bool? Invoice_Supplier_Original { get; set; }
        
       

    }

    
}