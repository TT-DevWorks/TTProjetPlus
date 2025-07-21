using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ttindexsearch
    {
        public string parentfolder { get; set; }
        public string filename { get; set; }
        public string filetype { get; set; }
        public string filedatecreation { get; set; }
        public string fileLastmodificationdate { get; set; }
        public System.Guid pathfileId_ { get; set; }
    }
}