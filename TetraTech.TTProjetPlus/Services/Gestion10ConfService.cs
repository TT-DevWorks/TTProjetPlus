using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class Gestion10ConfService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();

        private readonly DirectoryServiceForAD _directoryServiceForAD = new DirectoryServiceForAD();


        public Gestion10ConfList GetGestion10ConfList(string empNum)
        {

            Gestion10ConfList model = new Gestion10ConfList();
            var items = (from d in _entities.Gestion10Conf_Admin
                         join e in _entities.Gestion10Conf on d.Disc_ID equals e.Disc_ID
                         where d.Employee_Number.ToString() == empNum
                         select new
                         {
                             DisciplineName = e.Name,
                             Disc_ID = d.Disc_ID
                         });

            List<Gestion10ConfItem> di = items.Select(i =>
            new Gestion10ConfItem
            {
                DisciplineName = i.DisciplineName,
                DiscId = i.Disc_ID
            }).ToList();
                        
            model.disciplineItem = di;            
            return model;
        }


        public CommonModelGestion10Conf GetDetailsForSelectedGroup(string selectedGroup)
        {
            var adGroupName = _entities.Gestion10Conf_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == selectedGroup)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();

            var availableToEmployeesMembersOfTmp = _entities.Gestion10Conf_ADGroup
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

            CommonModelGestion10Conf result = new CommonModelGestion10Conf
            {
                SelectedGestion10Conf = adGroupName,
                AvailableToEmployeesMembersOf = availableToEmployeesMembersOf,
                Gestion10ConfGroupMembersList = new List<Gestion10ConfMember>()
            };

            if (!string.IsNullOrEmpty(adGroupName))
            {
                var groupmembers = _directoryServiceForAD.GetADGroupMembers(adGroupName);

                // Dans certains cas comme lors de l'appel de la discipline "EMISSIONS (TQE)", la récupération du groupe AD "BPR.GG_FIC_EMISSIONS" contient des comptes TT et également d'autres groupes AD.
                // On ne veut pas afficher ces sous-groupes AD ... donc on les enlève de la liste finale en vérifiant s'il y a une valeur dans le champ EmployeeId
                result.Gestion10ConfGroupMembersList = groupmembers                    
                    .Select(m => new Gestion10ConfMember { Gestion10ConfGroup = adGroupName, Gestion10ConfGroupID = selectedGroup, Employee = new TetraTechUser(m) }).Where(p => p.Employee.EmployeeId.Length > 0).ToList();
            }

            return result;
        }

        public List<Gestion10ConfEmployeeItem> GetListEmployeeForGestion10Conf(string selectedGroup)
        {
            try
            {
                var query = _entities.usp_getGestion10ConfADGroupMembers(Int32.Parse(selectedGroup));

                var aList = from p in query
                            select new Gestion10ConfEmployeeItem
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

        public OperationStatus AddEmployeeToGroupGestion10Conf(string TTAccount, string discGroupID)
        {
            var discGroupName = _entities.Gestion10Conf_ADGroup
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