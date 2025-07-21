using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class GestionGroupeADService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();

        private readonly DirectoryServiceForAD _directoryServiceForAD = new DirectoryServiceForAD();


        public GestionGroupeADList GetGestionGroupeADList(string empNum)
        {

            GestionGroupeADList model = new GestionGroupeADList();
            var items = (from d in _entities.GestionGroupeAD_Admin
                         join e in _entities.GestionGroupeADs on d.Disc_ID equals e.Disc_ID
                         where d.Employee_Number.ToString() == empNum
                         select new
                         {
                             DisciplineName = e.Name,
                             Disc_ID = d.Disc_ID
                         });

            List<GestionGroupeADItem> di = items.Select(i =>
            new GestionGroupeADItem
            {
                DisciplineName = i.DisciplineName,
                DiscId = i.Disc_ID
            }).ToList();
                        
            model.disciplineItem = di;            
            return model;
        }


        public CommonModelGestionGroupeAD GetDetailsForSelectedGroup(string selectedGroup)
        {
            var adGroupName = _entities.GestionGroupeAD_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == selectedGroup)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();

            var availableToEmployeesMembersOfTmp = _entities.GestionGroupeAD_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == selectedGroup)
                                .Select
                                    (p => p.AvailableToEmployeesMembersOf).FirstOrDefault();

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

            CommonModelGestionGroupeAD result = new CommonModelGestionGroupeAD
            {
                SelectedGestionGroupeAD = adGroupName,
                AvailableToEmployeesMembersOf = availableToEmployeesMembersOf,
                GestionGroupeADGroupMembersList = new List<GestionGroupeADMember>()
            };

            if (!string.IsNullOrEmpty(adGroupName))
            {
                var groupmembers = _directoryServiceForAD.GetADGroupMembers(adGroupName);

                // Dans certains cas comme lors de l'appel de la discipline "EMISSIONS (TQE)", la récupération du groupe AD "BPR.GG_FIC_EMISSIONS" contient des comptes TT et également d'autres groupes AD.
                // On ne veut pas afficher ces sous-groupes AD ... donc on les enlève de la liste finale en vérifiant s'il y a une valeur dans le champ EmployeeId
                result.GestionGroupeADGroupMembersList = groupmembers                    
                    .Select(m => new GestionGroupeADMember { GestionGroupeADGroup = adGroupName, GestionGroupeADGroupID = selectedGroup, Employee = new TetraTechUser(m) }).Where(p => p.Employee.EmployeeId.Length > 0).ToList();
            }

            return result;
        }

        public List<GestionGroupeADEmployeeItem> GetListEmployeeForGestionGroupeAD(string selectedGroup)
        {
            try
            {
                var query = _entities.usp_getGestionGroupeAD_GroupMembers(Int32.Parse(selectedGroup));

                var aList = from p in query
                            select new GestionGroupeADEmployeeItem
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

        public OperationStatus AddEmployeeToGroupGestionGroupeAD(string TTAccount, string discGroupID)
        {
            var discGroupName = _entities.GestionGroupeAD_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == discGroupID)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();
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
    }
}