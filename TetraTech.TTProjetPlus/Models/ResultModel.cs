using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class ResultModel
    {
        public List<usp_TTIndex_getFiles_Result> result { get; set; }

        //il faut corriger cette partie:
      //  public List<usp_TTIndex_getFiles2_Result> result2 { get; set; }

        public string itemsNumber { get; set; }
        public int elapsedTime { get; set; }
    }
}