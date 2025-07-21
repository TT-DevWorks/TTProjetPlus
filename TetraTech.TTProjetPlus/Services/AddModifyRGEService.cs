using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class AddModifyRGEService
    {

        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public List<RGE_Inclure_KM> GetListeRGEInclureKM()
        {

            var liste = _entitiesSuiviMandat.RGE_Inclure_KM.ToList();

            return liste;
        }


        public string ModifyRGE(int id, string First_Name, string Employee_Number, string Organization, string Email_Address, string DateDebutProjet, string NoProjet)
        {
            try
            {
                var result = "";

                var itemproject = _entitiesSuiviMandat.RGE_Inclure_KM.Where(p => p.id == id).FirstOrDefault();
                
                itemproject.First_Name = First_Name;
                itemproject.Employee_Number = Employee_Number;
                itemproject.Organization = Organization;
                itemproject.Email_Address = Email_Address;

                DateTime? dateFormatee = !string.IsNullOrEmpty(DateDebutProjet) ? (DateTime?)Convert.ToDateTime(DateDebutProjet): null;
                itemproject.DateDebutProjet = dateFormatee;
                itemproject.NoProjet = NoProjet;
                _entitiesSuiviMandat.SaveChanges();
                result = "3.successModify";
                                
                return result;
            }
            catch (Exception e)
            {
                return "4.failedModify";
            }
        }


        public string DeleteInRGE(int id)
        {
            try
            {
                var item = _entitiesSuiviMandat.RGE_Inclure_KM.Where(p => p.id == id).FirstOrDefault();

                _entitiesSuiviMandat.RGE_Inclure_KM.Remove(item);
                _entitiesSuiviMandat.SaveChanges();
                return "5.successRemove";
            }

            catch (Exception e)
            {
                return "6.failedRemove";
            }
        }


        public string insertInRGE(string First_Name, string Employee_Number, string Organization, string Email_Address, string DateDebutProjet, string NoProjet)
        {
            try
            {
                RGE_Inclure_KM model = new RGE_Inclure_KM();

                var newId = _entitiesSuiviMandat.RGE_Inclure_KM.Max(x => x.id) + 1;

                model.id = newId;
                model.First_Name = First_Name;
                model.Employee_Number = Employee_Number;
                model.Organization = Organization;
                model.Email_Address = Email_Address;

                DateTime? dateFormatee = !string.IsNullOrEmpty(DateDebutProjet) ? (DateTime?)Convert.ToDateTime(DateDebutProjet) : null;
                model.DateDebutProjet = dateFormatee;
                model.NoProjet = NoProjet;

                _entitiesSuiviMandat.RGE_Inclure_KM.Add(model);
                _entitiesSuiviMandat.SaveChanges();
                return "1.successInsert";
            }

            catch (Exception e)
            {
                return "2.failedInsert";
            }
        }


    }
}