using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Models
{
    public class FileUploadModel
    {
        public List<string> listFolders { get; set; }
        public List<string> listDocNames { get; set; }
        public List<structureItem> listeAllClient { get; set; }
    }
}