using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ListOfProjectsAndOS
    {
        public List<OSProjectModel> ProjectsList { get; set; }
        public  List<OSProjectModel> OffersList { get; set; }
    }
}