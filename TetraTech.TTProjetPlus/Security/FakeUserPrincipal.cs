using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TetraTech.TTProjetPlus.Security
{
    public class FakeUserPrincipal : IUserPrincipal
    {
        public FakeUserPrincipal(string givenName, string surname, string employeeId, string ttAccount)
        {
            GivenName = givenName;
            Surname = surname;
            EmployeeId = employeeId;
            TTAccount = ttAccount;
        }

        public string GivenName { get; }

        public string Surname { get; }

        public string EmployeeId { get; }

        public string TTAccount { get; }
    }
}