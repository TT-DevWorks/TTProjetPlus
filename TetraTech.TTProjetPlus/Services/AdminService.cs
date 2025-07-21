using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class AdminService
    {

        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();


        public List<OS_Project> returnClosedProjects()
        {
            //SELECT TOP(1000) [Status_ID]
            //,[Description_FR]
            //,[Parent_Status_ID]
            //,[isActive]
            //  FROM[TTProjetPlus].[dbo].[Status]
            //  where Status_ID in (4,6,8)

            var liste = _entities.OS_Project.Where(p=>p.Status_ID == 6 || p.Status_ID == 10 || p.Status_ID == 8).ToList();
            return liste;
        }


        public string CancelProjectClosing(string projectId)
        {
            try
            {

            var result =  _entities.usp_AnnulerFermetureProjet(projectId).Select(p=>p.closingCancellation).FirstOrDefault().ToString();               
                return result;
            }
            catch (Exception e)
            {
                return "L'annulation de fermeture a echouée";
            }
        }

    }
}