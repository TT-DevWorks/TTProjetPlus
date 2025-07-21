using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class DashBoardService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        public string GetCurrentPeriodWeekDate()
        {
            var CurrentPeriodWeekDate = _entitiesSuiviMandat.XX_PROJECT_DATA.Max(pd => pd.Period_Week_Date);
            return CurrentPeriodWeekDate.ToString("yyyy-MM-dd");
        }

    }
}