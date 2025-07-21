using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Web;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Views;

namespace TetraTech.TTProjetPlus.Security
{
    public class DirectoryService
    {
        public IUserPrincipal GetUser(WindowsIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));
            var context = new PrincipalContext(ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, identity.User.Value);


            if (principal == null)
                throw new Exception($"User '{identity.Name}' is not found.");

            return new TetraTechUserPrincipal(principal);
        }

        public IUserPrincipal FindUserByEmployeeId(string employeeId)
        {
            if (employeeId == null)
                throw new ArgumentNullException(nameof(employeeId));

            var context = new PrincipalContext(ContextType.Domain);
            var principal = new UserPrincipal(context)
            {
                EmployeeId = employeeId
            };

            var searcher = new PrincipalSearcher(principal);
            var result = searcher.FindOne() as UserPrincipal;

            return result != null ? new TetraTechUserPrincipal(result) : null;
        }

        public bool isAllowedAdvanced()
        {
            //GG_GES_REP_SUPER
            //var allowed = false;
            var identity = HttpContext.Current.User.Identity as WindowsIdentity;
            var context = new PrincipalContext(ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, identity.User.Value);



            string strIdentityName = "";
            //strIdentityName = identity.Name.Replace("TT\\", "");
            strIdentityName = UserService.CurrentUser.TTAccount.Replace("TT\\", "");
            var wi = new WindowsIdentity(strIdentityName);
            var wp = new WindowsPrincipal(wi);

            bool inRole = wp.IsInRole("BPR.GG_GES_REP_SUPER");
            return inRole;

        }

        public bool isAllowedAdministration()
        {
            //GG_GES_REP_ADMIN
            // var allowed = false;
            var identity = HttpContext.Current.User.Identity as WindowsIdentity;
            var context = new PrincipalContext(ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, identity.User.Value);

            string strIdentityName = "";
            //strIdentityName = identity.Name.Replace("TT\\", "");
            strIdentityName = UserService.CurrentUser.TTAccount.Replace("TT\\", "");
            var wi = new WindowsIdentity(strIdentityName);
            var wp = new WindowsPrincipal(wi);

            bool inRole = wp.IsInRole("BPR.GG_GES_REP_ADMIN");


            return inRole;


        }
    }
    public class DirectoryServiceForAD { 

        #region "ADConfig" 
        public PrincipalContext DomainGroupContext => ADContext("ADConfig_TTGroups");
        public PrincipalContext DomainTTUsersContext => ADContext("ADConfig_TTUsers");        

        private ADConfig GetADConfig(string keyname)
        {
            if (string.IsNullOrEmpty(keyname)) //no keyname
                return null;

            string config = ConfigurationManager.AppSettings[keyname]; //key not found in web.config
            if (string.IsNullOrEmpty(config))
                throw new Exception(Views.ManageADGroupMembers.ManageADRessources.ConfigKeyNotFound);

            ADConfig result = ADConfig.ParseADConfigString(config, keyname);
            return result;
        }

        public PrincipalContext ADContext(string keyname)
        {
            if (string.IsNullOrEmpty(keyname))
                return null;

            ADConfig adConfig = GetADConfig(keyname);
            PrincipalContext context = (adConfig != null) ?
                   new PrincipalContext(ContextType.Domain, adConfig.URL, adConfig.Directory, adConfig.UserId, adConfig.Password)
                 : new PrincipalContext(ContextType.Domain);

            return context;
        }


        public IUserPrincipal FindUserByEmployeeId(string employeeId, PrincipalContext context)
        {
            if (employeeId == null)
                throw new ArgumentNullException(nameof(employeeId));            

            var principal = new UserPrincipal(context)
            {
                EmployeeId = employeeId
            };

            var searcher = new PrincipalSearcher(principal);
            var result = searcher.FindOne() as UserPrincipal;

            return result != null ? new TetraTechUserPrincipal(result) : null;
        }

        public IUserPrincipal FindUserByTTAccount(string TTAccount, PrincipalContext context)
        {
            if (TTAccount == null)
                throw new ArgumentNullException(nameof(TTAccount));            

            var principal = new UserPrincipal(context)
            {
                SamAccountName = TTAccount
            };

            var searcher = new PrincipalSearcher(principal);
            var result = searcher.FindOne() as UserPrincipal;

            return result != null ? new TetraTechUserPrincipal(result) : null;
        }

        public List<TetraTechUserPrincipal> GetADGroupMembers(string ADGroup)
        {
            PrincipalContext context = DomainGroupContext;
            List<TetraTechUserPrincipal> members;
            var group = GroupPrincipal.FindByIdentity(context, ADGroup);

            if (group != null)
            {
                members = group.GetMembers(false).Select(p => new TetraTechUserPrincipal(p as UserPrincipal)).ToList();
            }
            else
            {
                members = new List<TetraTechUserPrincipal>().ToList();
            }
            
            return members;
        }

        public OperationStatus AddEmployeeToADGroup(string TTAccount, string groupName)
        {
            try
            {
                PrincipalContext context = DomainGroupContext;

                GroupPrincipal grp = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);

                if (grp == null)
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.GroupNotFound);


                //TetraTechUserPrincipal userprincipal = FindUserByEmployeeId(employeeid, DomainTTUsersContext) as TetraTechUserPrincipal;
                TetraTechUserPrincipal userprincipal = FindUserByTTAccount(TTAccount, DomainTTUsersContext) as TetraTechUserPrincipal;

                if (userprincipal == null)
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.UserNotFound);

                var user = userprincipal.Principal;

                if (grp.Members.Contains(user))
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.UserExistsInAdGroup);
                else
                {
                    grp.Members.Add(user);
                    grp.Save();
                    return new OperationStatus
                    {
                        Status = true,
                        Message = string.Format(Views.ManageADGroupMembers.ManageADRessources.SuccessAddUserToGroup, user.DisplayName)
                    };
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("denied"))
                    throw new Exception(Resources.TTProjetPlusResource.AccessDenied);
                else
                    throw (ex);
            }
        }

        public OperationStatus RemoveEmployeeFromADGroup(string employeeid, string groupName)
        {
            try
            {
                PrincipalContext context = DomainGroupContext;

                GroupPrincipal grp = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);

                if (grp == null)
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.GroupNotFound);


                TetraTechUserPrincipal userprincipal = FindUserByEmployeeId(employeeid, DomainTTUsersContext) as TetraTechUserPrincipal;

                if (userprincipal == null)
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.UserNotFound);

                var user = userprincipal.Principal;

                if (!grp.Members.Contains(user))
                    throw new Exception(Views.ManageADGroupMembers.ManageADRessources.UserDoesntExistsInAdGroup);
                else
                {
                    grp.Members.Remove(user);
                    grp.Save();
                    return new OperationStatus
                    {
                        Status = true,
                        Message = string.Format(Views.ManageADGroupMembers.ManageADRessources.SuccessRemoveEmployeeFromADGroup, user.DisplayName)
                    };
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("denied"))
                    throw new Exception(Resources.TTProjetPlusResource.AccessDenied);
                else
                    throw (ex);
            }
        }

        #endregion



    }
}