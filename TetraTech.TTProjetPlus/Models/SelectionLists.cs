using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public enum SelectionType { SMStatus, SMPriority, SMRisk }
     

    public class SelectItem:IComparable<SelectItem>
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? estimatedDate { get; set; }
        public string TLinxProjectNumber { get; set; }

        public string displayClass { get; set; }
        public int CompareTo(SelectItem obj)
        {
            return this.Value.CompareTo(obj.Value);
        }
    }

     
} 