using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class AdditionalInfoService
    {
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();


        public List<usp_getProject_ProgramManager_Result> returnInfo (string infoType)
        {
            List<usp_getProject_ProgramManager_Result> listInfo = _entitiesSuiviMandat.usp_getProject_ProgramManager(infoType).ToList();
            return listInfo;
        }
    }
}