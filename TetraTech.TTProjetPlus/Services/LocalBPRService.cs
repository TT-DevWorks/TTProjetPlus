using System.Collections.Generic;
using System.Linq;
using TetraTech.TTProjetPlus.Data;




namespace TetraTech.TTProjetPlus.Services
{
    public class LocalBPRService
    {
       
        private readonly AD_TT_LOCAL_BPREntities1 _entitiesLOCAL_BPR = new AD_TT_LOCAL_BPREntities1();

           public List<tbAD_TT_Local_BPR> GetListeADLocalBPR()
            {                   

                var liste = _entitiesLOCAL_BPR.tbAD_TT_Local_BPR.ToList();

                return liste;

        }


    }




}
