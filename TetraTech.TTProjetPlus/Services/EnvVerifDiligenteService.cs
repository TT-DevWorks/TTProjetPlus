using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class EnvVerifDiligenteService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();


        public List<usp_get_OS_Project_Env_Result> returnInfoProjet ()
        {
            List<usp_get_OS_Project_Env_Result> listInfoProjet = _entities.usp_get_OS_Project_Env().ToList();
            return listInfoProjet;
        }
    }
}