using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class ModeleSelectionService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

        #region modele selection
        public List<structureItem> GetModeleSelectionForModelId2(string user, int modelId)
        {
            var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN") && x.MOD_Id == modelId).OrderBy(p => p.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };
            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user) && x.MOD_Id == modelId).OrderBy(p => p.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom, Value = p.MOD_SEL_Id.ToString() };

            var l1 = projectsList1.ToList();
            var l2 = projectsList2.ToList();
            l1.AddRange(l2);
            return l1;

        }


        public string AddNewModeleSelection(string user, string modeleName, string modeleDescription,
               string modeleID, string modeleSelectionToCopy)
        {
            try
            {


                //liste des repertoires a ajouter dans la table [Modele_Selection_Repertoire] pour le nouveau modele de selection
                //(en fonction du mod_id):
                List<int> listRepertoiresForModelId = new List<int>();
                listRepertoiresForModelId = (modeleSelectionToCopy == "" || modeleSelectionToCopy == null) ?
                    _entitiesBPR.Repertoires.Where(p => p.MOD_Id.ToString() == modeleID && p.REP_Base == true)
                    .Select(p => p.REP_Id).ToList() :
                     _entitiesBPR.Modele_Selection_Repertoire.Where(p => p.MOD_SEL_Id.ToString() == modeleSelectionToCopy)
                    .Select(p => p.REP_Id).ToList();


                if (listRepertoiresForModelId.Count() == 0)
                {
                    return "001";
                }

                else
                {
                    var entity = new Modele_Selection();

                    entity.MOD_SEL_Utilisateur = user;
                    entity.MOD_SEL_Nom = modeleName;
                    entity.MOD_SEL_Description = modeleDescription;
                    entity.MOD_Id = Int32.Parse(modeleID);
                    _entitiesBPR.Modele_Selection.Add(entity);

                    _entitiesBPR.SaveChanges();

                    //on recupére le mod_sel_id  qui a été attribué au nouveau modele de selection
                    var newModSelId = _entitiesBPR.Modele_Selection.Where(p => p.MOD_SEL_Utilisateur == user)
                        .Max(p => p.MOD_SEL_Id);

                    //créer les nouvelles lignes dans la table [Modele_Selection_Repertoire]:
                    foreach (int item in listRepertoiresForModelId)
                {
                    var entityModele = new Modele_Selection_Repertoire();

                    entityModele.MOD_SEL_Id = newModSelId;
                    entityModele.REP_Id = item;
                    _entitiesBPR.Modele_Selection_Repertoire.Add(entityModele);

                }
                _entitiesBPR.SaveChanges();
                return "";
                }
                
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        public string DeleteRepForModeleSelection(int modeleSelection)
        {
            try
            {
                var remove = _entitiesBPR.Modele_Selection_Repertoire
                             .Where(p => p.MOD_SEL_Id == modeleSelection)
                             .ToList();


                if (remove != null)
                {
                    _entitiesBPR.Modele_Selection_Repertoire.RemoveRange(remove);
                    _entitiesBPR.SaveChanges();
                    return "";
                }
                else
                {
                    return "001";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        public string ReturnModIdForModeleSelection(string modeleSelection)
        {
            var modId = _entitiesBPR.Modele_Selection.Where(x => x.MOD_SEL_Id.ToString() == modeleSelection)
                                  .Select(x => x.MOD_Id).FirstOrDefault().ToString();
            return modId;
        }

        public List<string> ReturnRepForModele(int selectionModel)
        {
            var listOfRepForModId = _entitiesBPR.Modele_Selection_Repertoire.Where(p => p.MOD_SEL_Id == selectionModel)
                                  .Select(x => x.REP_Id.ToString()).ToList();
            return listOfRepForModId;
        }

        public void AddRepForModeleSelection(int modeleSelection, List<string> listOfNodes)
        {
            try
            {
                foreach (string item in listOfNodes)
                {
                    var item2 = Int32.Parse(item);
                    var entityModele = new Modele_Selection_Repertoire();

                    entityModele.MOD_SEL_Id = modeleSelection;
                    entityModele.REP_Id = item2;
                    _entitiesBPR.Modele_Selection_Repertoire.Add(entityModele);

                }
                _entitiesBPR.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public string DeleteModeleSelection(int modeleSelection)
        {
            try
            {
                var remove = _entitiesBPR.Modele_Selection
                             .Where(p => p.MOD_SEL_Id == modeleSelection)
                             .FirstOrDefault();


                if (remove != null)
                {
                    _entitiesBPR.Modele_Selection.Remove(remove);
                    _entitiesBPR.SaveChanges();
                }

                return "";
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        public bool ReturnIsModeleModifiable(string selectionModel)
        {
            var type = _entitiesBPR.Modele_Selection.Where(p => p.MOD_SEL_Id.ToString() == selectionModel)
                .Select(p => p.MOD_SEL_Utilisateur).FirstOrDefault();
        if (type == "COMMUN") return false;
            else return true;
        }

        public bool ReturnIsModeleNameExists(string modeleSelectionName, string user)
        {
            var count = _entitiesBPR.Modele_Selection.Where(p => p.MOD_SEL_Nom == modeleSelectionName && p.MOD_SEL_Utilisateur == user).Count();
            if (count >= 1) return true;
            else return false;
        }



        public List<structureItem> GetModeleSelectionToDelete(string user)
        {
            //var projectsList1 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == "COMMUN")).OrderBy(x => x.MOD_SEL_Nom)
            //                    select new structureItem { Name = p.MOD_SEL_Nom + " * " + p.MOD_SEL_Utilisateur, Value = p.MOD_SEL_Id.ToString() };

            var projectsList2 = from p in _entitiesBPR.Modele_Selection.Where(x => (x.MOD_SEL_Utilisateur == user)).OrderBy(x => x.MOD_SEL_Nom)
                                select new structureItem { Name = p.MOD_SEL_Nom + " * " + p.MOD_SEL_Utilisateur, Value = p.MOD_SEL_Id.ToString() };

            var l1 = projectsList2.ToList();
            //var l2 = projectsList2.ToList();
            //l1.AddRange(l2);
            return l1;
        }
        #endregion

    }

    public class CodeComparer : IComparer<node>
    {
        public int Compare(node x, node y)
        {
            var regex = new Regex("^(d +)");  // ^ (d +)

            // run the regex on both strings
            var xRegexResult = regex.Match(x.Code);
            var yRegexResult = regex.Match(y.Code);

            // check if they are both numbers
          //  if (xRegexResult.Success && yRegexResult.Success)
               if(x.Code.All(char.IsDigit)== true && y.Code.All(char.IsDigit)== true)
            {
                return int.Parse(x.Code).CompareTo(int.Parse(y.Code));
            }

            // otherwise return as string comparison
            return (x.Code).CompareTo(y.Code);
        }
    }

    
}