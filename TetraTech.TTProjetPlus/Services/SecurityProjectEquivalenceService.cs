using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class SecurityProjectEquivalenceService
    {
        
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();

        public List<Security_Project_Equivalence> GetListeSecurityProjectEquivalence()
        {

            var liste = _entitiesBPR.Security_Project_Equivalence.ToList();

            return liste;
        }


        public string insertInProject(string TTprojetID, string TlinxProjetID)
        {
            try
            {
                Security_Project_Equivalence model = new Security_Project_Equivalence();
                model.TTprojetID = TTprojetID;
                model.TlinxProjetID = TlinxProjetID;

                _entitiesBPR.Security_Project_Equivalence.Add(model);
                _entitiesBPR.SaveChanges();
                return "1.successInsert";
            }

            catch (Exception e)
            {
                return "2.failedInsert";
            }
        }

        public string ModifyInProject(string TTprojetID, string TlinxProjetID)
        {
            try
            {
                var result = "";

                var itemproject = _entitiesBPR.Security_Project_Equivalence.Where(p => p.TTprojetID == TTprojetID).FirstOrDefault();

                if (itemproject == null)
                {
                    itemproject = _entitiesBPR.Security_Project_Equivalence.Where(p => p.TlinxProjetID == TlinxProjetID).FirstOrDefault();
                    itemproject.TTprojetID = TTprojetID;
                    _entitiesBPR.SaveChanges();
                    result = "3.successModify";
                }
                else
                {
                    itemproject.TlinxProjetID = TlinxProjetID;
                    _entitiesBPR.SaveChanges();
                    result = "3.successModify";

                }
                return result;
            }
            catch (Exception e)
            {                
                return "4.failedModify";
            }
        }

        public string DeleteInProject(string TTprojetID)
        {
            try
            {
                var item = _entitiesBPR.Security_Project_Equivalence.Where(p => p.TTprojetID == TTprojetID).FirstOrDefault();
                
                _entitiesBPR.Security_Project_Equivalence.Remove(item);
                _entitiesBPR.SaveChanges();                
                return "5.successRemove";
            }

            catch (Exception e)
            {
                return "6.failedRemove";
            }
        }



    }
}