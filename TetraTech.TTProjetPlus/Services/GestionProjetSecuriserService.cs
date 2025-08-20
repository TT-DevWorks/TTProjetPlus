using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class GestionProjetSecuriserService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();

        private readonly DirectoryServiceForAD _directoryServiceForAD = new DirectoryServiceForAD();


        public GestionProjetSecuriserList GetGestionProjetSecuriserList(string empNum, string empName)
        {
            var empname = empName.Replace(",", "").Trim();

            GestionProjetSecuriserList model = new GestionProjetSecuriserList();
            var items = (from d in _entities.ProjetSecuriser_GroupAD
                         join e in _entities.ProjetSecuriser_Manager on d.id equals e.ProjetSecuriser_GroupAD_ID
                         where e.Employee_Number.ToString() == empNum && e.Employee_Name.ToString() == empname
                         select new
                         {
                             DisciplineName = d.GroupAD,
                             Disc_ID = d.id
                         });

            List<GestionProjetSecuriserItem> di = items.Select(i =>
            new GestionProjetSecuriserItem
            {
                DisciplineName = i.DisciplineName,
                DiscId = i.Disc_ID
            }).ToList();
                        
            model.disciplineItem = di;            
            return model;
        }


        public CommonModelGestionProjetSecuriser GetDetailsForSelectedGroup(string selectedGroup)
        {
            var adGroupName = _entities.ProjetSecuriser_GroupAD
                                .Where
                                    (p => p.id.ToString() == selectedGroup)
                                .Select
                                    (p => p.GroupAD).FirstOrDefault();

            //var adGroupName = selectedGroup;

            var availableToEmployeesMembersOfTmp = "TT.Projects.Quebec";

            string availableToEmployeesMembersOf = "";
            if (availableToEmployeesMembersOfTmp != null)
            {
                string[] words = availableToEmployeesMembersOfTmp.Split(';');

                foreach (var word in words)
                {
                    switch (word)
                    {
                        case "BPR.GG_FNC_ING":
                            if (availableToEmployeesMembersOf.Length > 0) { availableToEmployeesMembersOf = availableToEmployeesMembersOf + ", "; };
                            availableToEmployeesMembersOf = availableToEmployeesMembersOf + "Ingénieurs";

                            break;
                        case "BPR.GG_FNC_SEC":
                            if (availableToEmployeesMembersOf.Length > 0) { availableToEmployeesMembersOf = availableToEmployeesMembersOf + ", "; };
                            availableToEmployeesMembersOf = availableToEmployeesMembersOf + "Secrétaires";
                            break;
                        case "BPR.GG_FNC_DAO":
                            if (availableToEmployeesMembersOf.Length > 0) { availableToEmployeesMembersOf = availableToEmployeesMembersOf + ", "; };
                            availableToEmployeesMembersOf = availableToEmployeesMembersOf + "Dessinateurs";
                            break;
                        case "TT.Projects.Quebec":
                            if (availableToEmployeesMembersOf.Length > 0) { availableToEmployeesMembersOf = availableToEmployeesMembersOf + ", "; };
                            availableToEmployeesMembersOf = availableToEmployeesMembersOf + "TT.Projects.Quebec";
                            break;
                        default:

                            break;
                    }
                }
            }
            else
            {
                availableToEmployeesMembersOf = "";
            }

            CommonModelGestionProjetSecuriser result = new CommonModelGestionProjetSecuriser
            {
                SelectedGestionProjetSecuriser = adGroupName,
                AvailableToEmployeesMembersOf = availableToEmployeesMembersOf,
                GestionProjetSecuriserGroupMembersList = new List<GestionProjetSecuriserMember>()
            };

            if (!string.IsNullOrEmpty(adGroupName))
            {
                var groupmembers = _directoryServiceForAD.GetADGroupMembers(adGroupName);

                // Dans certains cas comme lors de l'appel de la discipline "EMISSIONS (TQE)", la récupération du groupe AD "BPR.GG_FIC_EMISSIONS" contient des comptes TT et également d'autres groupes AD.
                // On ne veut pas afficher ces sous-groupes AD ... donc on les enlève de la liste finale en vérifiant s'il y a une valeur dans le champ EmployeeId
                result.GestionProjetSecuriserGroupMembersList = groupmembers                    
                    .Select(m => new GestionProjetSecuriserMember { GestionProjetSecuriserGroup = adGroupName, GestionProjetSecuriserGroupID = selectedGroup, Employee = new TetraTechUser(m) }).Where(p => p.Employee.EmployeeId.Length > 0).ToList();
            }

            return result;
        }

        public List<GestionProjetSecuriserEmployeeItem> GetListEmployeeForGestionProjetSecuriser(string selectedGroup)
        {
            try
            {
                var query = _entities.usp_getGestionProjetSecuriserADGroupMembers(Int32.Parse(selectedGroup));

                var aList = from p in query
                            select new GestionProjetSecuriserEmployeeItem
                            {
                                Full_Name = p.displayName,
                                TTAccount = p.sAMAccountName,
                                discDocProjId = selectedGroup
                            };
                return aList.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public OperationStatus AddEmployeeToGroupGestionProjetSecuriser(string TTAccount, string discGroupID)
        {
            var discGroupName = _entities.ProjetSecuriser_GroupAD
                                .Where
                                    (p => p.id.ToString() == discGroupID)
                                .Select
                                    (p => p.GroupAD).FirstOrDefault();
            //LogAction("Add", TTAccount, Int32.Parse(discGroupID));
            return _directoryServiceForAD.AddEmployeeToADGroup(TTAccount, discGroupName);
        }

        public void LogAction(string action, string TTAccount, int DiscOrDocProjID)
        {
            var entity = new Discipline_Logs();

            entity.SubmitBy = UserService.AuthenticatedUser.TTAccount;
            entity.Action = action;
            entity.ForEmployeeTTAccount = TTAccount;
            entity.Disc_ID = DiscOrDocProjID;
            

            entity.UTCDateTime = DateTime.Now;
            _entities.Discipline_Logs.Add(entity);
            _entities.SaveChanges();
        }


        public bool GetSecurityProjetSecuriser(string EmployeeNumber)
        {
            try
            {
                var UserProjetSecuriser = _entities.TTProjetSecurities.Where(p => p.Employee_Number == EmployeeNumber && p.Security_Desc == "Projet Securiser").FirstOrDefault();

                if (UserProjetSecuriser != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}