using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class searchModelForMasterProject
    {
        public List<masterProjectItem> listOfItems { get; set; }
        public string searchTerm { get; set; }
}
}