using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class SecuriteInfo
    {

        public string _Nom { get; set; }

        public int _Droit { get; set; }
        public string _Suffixe { get; set; }
        public string _GroupeSecuriteAD { get; set; }
        public string _GroupeSecuriteADPrefix { get; set; }

    }
}