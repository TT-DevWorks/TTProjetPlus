using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class ProjetSecuriserService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();


        public List<usp_get_ProjetSecuriserList_Result> returnProjetSecuriserList()
        {
            List<usp_get_ProjetSecuriserList_Result> listProjetSecuriser = _entities.usp_get_ProjetSecuriserList().ToList();
            return listProjetSecuriser;
        }

        public List<usp_ListeProjetaSecuriser_Result> returnlistProjetASecuriserStatut()
        {
            List<usp_ListeProjetaSecuriser_Result> listProjetSecuriserStatut = _entitiesBPR.usp_ListeProjetaSecuriser().ToList();
            return listProjetSecuriserStatut;
        }

        public List<structureItem> returnListeEmployeeActif_Inactive()
        {
            var now = DateTime.Now;
            var list = _entities.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= now && p.EFFECTIVE_END_DATE >= now).ToList();
            List<structureItem> finalList = new List<structureItem>();
            foreach (EmployeeActif_Inactive item in list)
            {
                structureItem STmodel = new structureItem();
                STmodel.Name = item.LAST_NAME + " " + item.FIRST_NAME + " (" + item.ORG + ")";
                STmodel.Value = item.EMPLOYEE_NUMBER;
                finalList.Add(STmodel);
            }
            return finalList.OrderBy(p => p.Name).ToList();
        }

        public bool InsertIntoProjetSecuriserTable(string Projet, string Repertoire, string GroupeAD, string Chemin, string ListeUsers, string ListeUsersNames)
        {
            try
            {
                _entities.usp_Insert_ProjetSecuriser(Projet, Repertoire, GroupeAD, Chemin, ListeUsers, ListeUsersNames);

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateProjetSecuriserTable(string id, string Projet, string Repertoire, string GroupeAD, string Chemin, string ListeUsers, string ListeUsersNames)
        {
            try
            {
                _entities.usp_Update_ProjetSecuriser(id,Projet, Repertoire, GroupeAD, Chemin, ListeUsers, ListeUsersNames);

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public bool ChangeProjectStatutSecuriser(string Projet)
        {
            try
            {
                _entitiesBPR.usp_ModifierStatutProjectASecuriser(Projet);

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }


    }

}