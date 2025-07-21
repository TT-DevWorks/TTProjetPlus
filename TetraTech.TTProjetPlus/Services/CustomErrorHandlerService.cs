using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TetraTech.TTProjetPlus.Services
{
    public class CustomErrorHandlerService
    {
        public void InsertError(String TTAccount, String ImpersonationName, String ControllerName, String ControllerFunction, String InnerException, String Url)
        {
            //TODO: stored proc

            //String strConnection = ConfigurationManager.ConnectionStrings["SuiviMandat"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(strConnection))              
            //{
            //    using (SqlCommand cmd = new SqlCommand("usp_TTLinxPlus_LogError", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@TTAccount", SqlDbType.VarChar).Value = TTAccount.Trim();
            //        cmd.Parameters.Add("@ImpersonationName", SqlDbType.VarChar).Value = ImpersonationName.Trim();
            //        cmd.Parameters.Add("@ControllerName", SqlDbType.VarChar).Value = ControllerName.Trim();
            //        cmd.Parameters.Add("@ControllerFunction", SqlDbType.VarChar).Value = ControllerFunction.Trim();
            //        cmd.Parameters.Add("@InnerException", SqlDbType.VarChar).Value = InnerException.Trim();
            //        cmd.Parameters.Add("@Url", SqlDbType.VarChar).Value = Url.Trim();

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }

    }
}