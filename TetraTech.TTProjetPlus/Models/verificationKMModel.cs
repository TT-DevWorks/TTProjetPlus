using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class verificationKMModel
    {
         public List<TableVerificationKM> liste_TableVerificationKM { get; set; }

        public List<usp_ReturnTableVerificationKM_ColumnsName_Result> liste_Columns { get; set; }

        public DataTable TableVerificationKM { get; set; }

        public class TableRowDto
        {
            public List<string> Cells { get; set; }
        }

    }
}