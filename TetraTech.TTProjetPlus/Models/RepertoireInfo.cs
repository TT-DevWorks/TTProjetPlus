using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraTech.TTProjetPlus.Models
{
    public class RepertoireInfo
    {
        public int _modeleId { get; set; }
        public int _Id { get; set; }
        public string _nom { get; set; }
        public string _nomBilingue { get; set; }
        public string _description { get; set; }
        public int _parentId { get; set; }
        public bool _dernier { get; set; }
        public bool _base { get; set; }
        public bool _racine { get; set; }
        public bool _racinePrincipale { get; set; }
        public string _GroupeSecurite { get; set; }
        public bool _InfoBulle { get { return _InfoBulle; } set { _InfoBulle = false; } }
        public bool _Actif { get { return _Actif; } set { _Actif = true; } }
        public bool _Special { get { return _Special; } set { _Special = false; } }
        public string _Special_sec { get; set; }
        public string _GroupeSecuriteAD { get; set; }
        public bool _ActionSecuriteAD { get { return _ActionSecuriteAD; } set { _ActionSecuriteAD = true; } }

        //   ' Objets membres disponibles par propriétés
        public RepertoireCollectionInfo _repertoires { get; set; }
        public SecuriteCollectionInfo _securites { get; set; }
        public RepertoireInfo _parent { get; set; }

        public string _GroupeSecuriteADPrefix
        {
            get { return _GroupeSecuriteADPrefix; }
            set
            {
                _GroupeSecuriteADPrefix = "GL_FIC_";
            }
        }
    }
}
