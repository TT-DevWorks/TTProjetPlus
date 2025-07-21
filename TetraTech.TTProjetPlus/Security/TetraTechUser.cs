using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Security
{
    public class TetraTechUser
    {
        public TetraTechUser(string firstName, string lastName, string employeeID, string ttAccount)
        {
            FirstName = firstName;
            LastName = lastName;
            EmployeeId = employeeID;
            TTAccount = ttAccount;
        }

        public TetraTechUser(IUserPrincipal principal) : this(principal.GivenName, principal.Surname, principal.EmployeeId, principal.TTAccount) { }

        public string FirstName { get; }

        public string LastName { get; }

        public string EmployeeId { get; }

        public string TTAccount { get; }

        public string DisplayName
        {
            get
            {
                return $"{LastName}, {FirstName} ";
            }
        }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}