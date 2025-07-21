using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Services
{
    public class DisciplinesService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();

        private readonly DirectoryServiceForAD _directoryServiceForAD = new DirectoryServiceForAD();


        public List<usp_getDisciplineAdminList_Result> GetDisciplineAdminList()
        {
            var query = _entities.usp_getDisciplineAdminList().Distinct().ToList();

            return query;
        }

        public DisciplineDocProj GetDisciplineList(string empNum)
        {

            DisciplineDocProj model = new DisciplineDocProj();
            var items = (from d in _entities.Discipline_Admin
                         join e in _entities.Disciplines on d.Disc_ID equals e.Disc_ID
                         where d.Employee_Number.ToString() == empNum
                         select new
                         {
                             DisciplineName = e.Name,
                             Disc_ID = d.Disc_ID
                         });

            List<disciplineItem> di = items.Select(i =>
            new disciplineItem
            {
                DisciplineName = i.DisciplineName,
                DiscId = i.Disc_ID
            }).ToList();
            
            var itemDocProj = GetDocProjList(empNum);
            model.disciplineItem = di;
            model.docProjItem = itemDocProj;
            return model;
        }

        public List<docProjItem> GetDocProjList(string empNum)
        {
            var items = (from d in _entities.DocProj_Admin
                         join e in _entities.DocProjs on d.DocProj_ID equals e.DocProj_ID
                         where d.Employee_Number.ToString() == empNum
                         select new
                         {
                             DocProj_Name = e.Name,
                             DocProj_ID = d.DocProj_ID
                         });
            List<docProjItem> dp = items.Select(i =>
            new docProjItem
            {
                DocProj_Name = i.DocProj_Name,
                DocProj_ID = i.DocProj_ID
            }).ToList();
            return dp;
        }

        public CommonModel GetDetailsForSelectedDiscipline(string selectedDiscipline)
        {
            var adGroupName = _entities.Discipline_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == selectedDiscipline)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();

            var availableToEmployeesMembersOfTmp = _entities.Discipline_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == selectedDiscipline)
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
                        default:

                            break;
                    }
                }
            }    
            else
            {
                availableToEmployeesMembersOf = "";
            }

            CommonModel result = new CommonModel
            {
                SelectedDiscipline = adGroupName,                
                AvailableToEmployeesMembersOf = availableToEmployeesMembersOf,
                DisciplineGroupMembersList = new List<DisciplineMember>()
            };

            if (!string.IsNullOrEmpty(adGroupName))
            {
                var groupmembers = _directoryServiceForAD.GetADGroupMembers(adGroupName);

                // Dans certains cas comme lors de l'appel de la discipline "EMISSIONS (TQE)", la récupération du groupe AD "BPR.GG_FIC_EMISSIONS" contient des comptes TT et également d'autres groupes AD.
                // On ne veut pas afficher ces sous-groupes AD ... donc on les enlève de la liste finale en vérifiant s'il y a une valeur dans le champ EmployeeId
                result.DisciplineGroupMembersList = groupmembers
                    .Select(m => new DisciplineMember { DiscGroup = adGroupName, DiscGroupID = selectedDiscipline, Employee = new TetraTechUser(m) }).Where(p => p.Employee.EmployeeId.Length>0).ToList();
            }

            return result;
        }

        public CommonModel GetDetailsForSelectedDocProj(string selectedDocProj)
        {
            var adGroupName = _entities.DocProj_ADGroup
                                .Where
                                    (p => p.DocProj_ID.ToString() == selectedDocProj)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();

            var availableToEmployeesMembersOfTmp = _entities.DocProj_ADGroup
                                .Where
                                    (p => p.DocProj_ID.ToString() == selectedDocProj)
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
                        default:

                            break;
                    }
                }
            }
            else
            {
                availableToEmployeesMembersOf = "";
            }

            CommonModel result = new CommonModel { SelectedDocProj = adGroupName,                
                AvailableToEmployeesMembersOf = availableToEmployeesMembersOf,
                DocProjGroupMembersList = new List<DocProjMember>() };

            if (!string.IsNullOrEmpty(adGroupName))
            {

            var groupmembers = _directoryServiceForAD.GetADGroupMembers(adGroupName);

                result.DocProjGroupMembersList = groupmembers
                    .Select(m => new DocProjMember { DocProjGroup = adGroupName, DocProjGroupID = selectedDocProj, EmployeeD = new TetraTechUser(m) }).ToList();
            }

            return result;
        }

        public OperationStatus AddEmployeeToDisciplineGroup(string TTAccount, string discGroupID)
        {
            var discGroupName = _entities.Discipline_ADGroup
                                .Where
                                    (p => p.Disc_ID.ToString() == discGroupID)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();
            LogAction("Discipline", "Add", TTAccount, Int32.Parse(discGroupID));
            return _directoryServiceForAD.AddEmployeeToADGroup(TTAccount, discGroupName);
        }

        public OperationStatus AddEmployeeToDocProjGroup(string TTAccount, string docProjGroupID)
        {
            var docProjGroupName = _entities.DocProj_ADGroup
                                .Where
                                    (p => p.DocProj_ID.ToString() == docProjGroupID)
                                .Select
                                    (p => p.ADGroup_Write).FirstOrDefault();
            LogAction("DocProj","Add",TTAccount,Int32.Parse(docProjGroupID));
            return _directoryServiceForAD.AddEmployeeToADGroup(TTAccount, docProjGroupName);
        }

        public void LogAction(string groupSelected, string action, string TTAccount, int DiscOrDocProjID)
        {
            var entity = new Discipline_Logs();

            entity.SubmitBy = UserService.AuthenticatedUser.TTAccount;
            entity.Action = action;
            entity.ForEmployeeTTAccount = TTAccount;
            if (groupSelected == "DocProj")
            {
                entity.DocProj_ID = DiscOrDocProjID;
            }
            else
            {                
                entity.Disc_ID = DiscOrDocProjID;
            }
                        
            entity.UTCDateTime = DateTime.Now;
            _entities.Discipline_Logs.Add(entity);
            _entities.SaveChanges();
        }

        public OperationStatus RemoveEmployeeFromGroup(string SelectedGroup, string SelectedGroupToRemove, string SelectedEmployeeNumber, string DiscOrDocProjID)
        {                        
            string TTAccount = ObtenirCompteTT(SelectedEmployeeNumber);

            LogAction(SelectedGroup, "Delete", TTAccount, Int32.Parse(DiscOrDocProjID));
            return _directoryServiceForAD.RemoveEmployeeFromADGroup(SelectedEmployeeNumber, SelectedGroupToRemove);
        }

        public string ObtenirCompteTT(string employeeid)
        {
            var directoryService = new DirectoryService();
            var strTTAccount = "";
            var principal = directoryService.FindUserByEmployeeId(employeeid);

            if (principal != null)
            {
                strTTAccount = principal.TTAccount.Replace(@"TT\","");
            }
            return strTTAccount;
        }

        public List<DisciplineEmployeeItem> GetListEmployeeForDiscipline(string selectedDiscipline)
        {
            try
            {
                var query = _entities.usp_getDisciplineADGroupMembers(Int32.Parse(selectedDiscipline));

                var aList = from p in query
                            select new DisciplineEmployeeItem
                            {
                                Full_Name = p.displayName,
                                TTAccount = p.sAMAccountName,
                                discDocProjId = selectedDiscipline
                            };             
                return aList.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<DisciplineEmployeeItem> GetListEmployeeForDocProj(string selectedDocProj)
        {
            try
            {
                var query = _entities.usp_getDocProjADGroupMembers(Int32.Parse(selectedDocProj));
                
                var aList = from p in query
                            select new DisciplineEmployeeItem
                            {
                                Full_Name = p.displayName,
                                TTAccount = p.sAMAccountName,
                                discDocProjId = selectedDocProj
                            };
                return aList.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}