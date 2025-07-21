using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class LitigeService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();


        public List<Projets_Litige> returnLitigeProjects ()
        {
            var liste = _entities.Projets_Litige.ToList();
            return liste;
        }

        public string insertInLitige(string projectNumber, string dateAdded, string comment, string closedFile)
        {
            try
            {
            Projets_Litige model = new Projets_Litige();
            model.Project_Number = projectNumber;
                model.DateAdded = dateAdded;
                model.Comments = comment;
            model.Dossier_Clos = closedFile == "Non" ? false : true;

                _entities.Projets_Litige.Add(model);
                _entities.SaveChanges();
                return "1.successInsert";
            }

            catch (Exception e)
            {
                return "2.failedInsert";
            }
        }

        public string editInLitige(string projectNumber, string dateAdded, string comment, string closedFile)
        {
            try
            {
                var item = _entities.Projets_Litige.First(g => g.Project_Number == projectNumber);
                item.DateAdded = dateAdded;
                item.Comments = comment;
                item.Dossier_Clos = closedFile == "Non" ? false : true;
                _entities.SaveChanges();
                return "3.successEdit";
            }

            catch (Exception e)
            {
                return "4.failedEdit";
            }
        }
    }
}