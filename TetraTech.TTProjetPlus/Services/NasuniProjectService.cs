using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class NasuniProjectService
    {
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        
     
        public ListProjectNasuni2 returnListeProjectNasuniAll(string activeproject)
        {
            if (activeproject == "yes")
            {
                var liste = _entitiesBPR.Nasuni_TT_ProjectsToSecured.Where(p => p.Completed == false).ToList();
                ListProjectNasuni2 model = new ListProjectNasuni2();
                model.ListProjectNasuni = liste;
                model.Checked = "checked";
                return model;
            }
            else
            {
                var liste = _entitiesBPR.Nasuni_TT_ProjectsToSecured.ToList();
                ListProjectNasuni2 model = new ListProjectNasuni2();
                model.ListProjectNasuni = liste;
                model.Checked = "unchecked";
                return model;
            }
        }

    }
}