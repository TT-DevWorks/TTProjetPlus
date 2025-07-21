using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class FoldersTree
    {
        public string rep_id { get; set;  }
        public string rep_desc { get; set; }
        public string rep_parent { get; set; }
    }
}