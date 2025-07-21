using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class YourProjectsModel
    {
        public string ProjectNumber { get; set; }
        public string ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public string Client { get; set; }
        public string ProjectManager { get; set; }
        public int Revenu { get; set; }
        public int Cost{ get; set; }
    }
}