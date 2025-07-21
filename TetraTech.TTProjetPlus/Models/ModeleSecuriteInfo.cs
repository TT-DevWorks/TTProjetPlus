using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class ModeleSecuriteInfo
    {
        public int  _Id { get; set; }
        public int _ModeleId { get; set; }
        public string _Nom { get; set; }
        public string _Description { get; set; }
        public modeleInfo _Modele { get; set; }

    }
}