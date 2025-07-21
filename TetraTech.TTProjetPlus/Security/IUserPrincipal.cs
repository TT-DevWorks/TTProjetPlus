using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraTech.TTProjetPlus.Security
{
    public interface IUserPrincipal
    {
        string GivenName { get; }

        string Surname { get; }

        string EmployeeId { get; }

        string TTAccount { get; }
    }
}
