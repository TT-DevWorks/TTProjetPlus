using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class modeleInfo
    {
        public int _id { get; set; }
        public string _Description { get; set; }
        public string _Nom { get; set; }
        public bool _Actif { get; set; }
        public int _Compagnie { get; set; }

        //  ' Objets membres disponibles par propriétés
        public RepertoireCollectionInfo _Repertoires { get; set; }
    }

}
