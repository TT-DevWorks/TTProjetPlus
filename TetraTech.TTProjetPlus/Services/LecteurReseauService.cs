using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class LecteurReseauService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public List<structureItem> getServersName()
        {
            var ListServers = from p in _entitiesBPR.usp_ObtenirServeur(null)
                              select new structureItem { Name =  p.SER_Description, Value = p.SER_Chemin_Partage };
            return ListServers.ToList();
        }

        public List<structureItem> getServersNameSpecial()
        {
            var ListServers = from p in _entitiesBPR.SpecialMapServers
                              select new structureItem { Name = p.ServerName + "-" + p.DescriptionFr, Value = p.PathPartage + "*" + p.Driver };
            return ListServers.ToList();
        }

    }
}