using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class CustomerService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();


        public List<Customer> returnListeCustomers ()
        {
            var liste = _entities.Customers.ToList();

            return liste;
        }

        public bool InsertIntoCustomerTable(string CustomerNumber, string CustomerName, string CreatedBy, string Reference)
        {
            try
            {
                Customer model = new Customer();                
                model.Customer_Number  = CustomerNumber;
                model.Customer_Name = CustomerName;
                model.CreatedBy = CreatedBy;
                model.Reference = Reference;
                _entities.Customers.Add(model);                
                _entities.SaveChanges();

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public bool returnExistCustomerNumber(string CustomerNumber)
        {
            
            var isCustomerNumberExist = _entities.Customers.Where(p => p.Customer_Number == CustomerNumber).ToList().Count();

            if (isCustomerNumberExist == 0)
            {
                return false;
            }
            else return true;
        }
    }

}