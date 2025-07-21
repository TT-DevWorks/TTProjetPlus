using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Security
{
    public class TetraTechUserPrincipal : IUserPrincipal
    {
        public TetraTechUserPrincipal(UserPrincipal principal)
        {
            Principal = principal;
        }

        public UserPrincipal Principal { get; private set; }


        public string GivenName
        {
            get
            {
                if (Principal == null)
                {
                    return "";
                }
                else return Principal.GivenName;
            }
        }

        public string Surname
        {
            get
            {
                if (Principal == null)
                {
                    return "";
                }
                else return Principal.Surname;
            }
        }

        public string EmployeeId
        {
            get
            {
                if (Principal == null)
                {
                    return "";
                }
                else return Principal.EmployeeId;
            }
        }

        public string TTAccount
        {
            get
            {
                if (Principal == null)
                {
                    return "";
                }
                else return "TT\\" + Principal.SamAccountName;
            }
        }
    }
}