using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class TTProjetMenuService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();



        public List<string> getProjetAvecStrucDeRepertoires()
        {
            var query = _entitiesBPR.Projets.Select(p => p.PRO_Id).ToList();
            return query.Distinct().ToList(); ;
        }

        public List<string> getProjectsNumbersinTTProjet()
        {
            var query = _entities.OS_Project.Select(p => p.Project_Number).ToList();
            return query.Distinct().ToList(); ;
        }

        public List<structureItem> GetModeleSecuriteForModelId(int modelId)
        {
            var ListSecuritet = from p in _entitiesBPR.Modele_Securite.Where(x => x.MOD_Id == modelId)
                                select new structureItem { Name = p.MOD_SEC_Nom, Value = p.MOD_SEC_Id.ToString() };
            return ListSecuritet.ToList();

        }

        public int getModSecIdForProjecctNumber(string pro_id)
        {
            var modSecId = _entitiesBPR.Projets.Where(x => x.PRO_Id == pro_id)
                                .Select(p => p.MOD_SEC_Id).FirstOrDefault();
            return modSecId;
        }

        public bool returnUserSatus( string userID)
        {
            var isAdmin = _entities.TTProjetSecurities.Where(p => p.Employee_Number == userID && p.Security_Desc == "Admin TI").ToList().Count(); ;
            if (isAdmin == 0)
            {
                return false;
            }
            else return true;
        }


        public bool getRapportSatisfactionClientFinale(string ProjectStartDate, string ProjectEndDate)
        {
            var ProjectStartDateFinale = "";
            var ProjectEndDateFinale = "";
            try
            {
                

                if (ProjectStartDate == "") {
                    ProjectStartDateFinale = "1900-01-01";

                }
                else
                {
                    ProjectStartDateFinale = ProjectStartDate;
                }

                if (ProjectEndDate == "") {
                    ProjectEndDateFinale = "1900-01-01";

                }
            else
                {
                    ProjectEndDateFinale = ProjectEndDate;
                }

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public List<Projet> returnProjectsToActived()
        {       
            var liste = _entitiesBPR.Projets.Where(p => p.ETA_PRO_ID == 2).ToList();
            return liste;
        }


        public string ActiveProject(string projectId)
        {
            try
            {
                var result = "";

                var itemproject = _entitiesBPR.Projets.Where(p => p.PRO_Id == projectId).FirstOrDefault();

                if (itemproject == null)
                {                   
                    result = "La réactivation du projet a echouée";
                }
                else
                {
                    itemproject.ETA_PRO_ID = 1;
                    _entitiesBPR.SaveChanges();
                    result = "La réactivation du projet a réussie";

                }
                return result;
            }
            catch (Exception e)
            {
                return "La réactivation du projet a echouée";
            }
        }

        public bool returnAdditionalInfoPermission(string userID)
        {
            var isAdmin = _entitiesSuiviMandat.infoPermissions.Where(p => p.empNumber == userID).ToList().Count(); ;
            if (isAdmin == 0)
            {
                return false;
            }
            else return true;
        }

    }
}