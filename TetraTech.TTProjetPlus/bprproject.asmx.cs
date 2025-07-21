using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TetraTech.TTProjetPlus.Data;
using System.Data;
using System.Data.SqlClient;
//using BPR.BPRProjet.Model;

namespace TetraTech.TTProjetPlus
{
    /// <summary>
    /// Summary description for bprproject
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class bprproject : System.Web.Services.WebService
    {

        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public bool ProjectExist(string pProjectId)
        {
            var db = _entitiesBPR;
            var con = new SqlConnection(db.Database.Connection.ConnectionString);
            Boolean blnProjetExist = false;

            DataSet ds = new System.Data.DataSet();

            SqlParameter parameter = new SqlParameter();
            var cmd = new SqlCommand("[dbo].[usp_IsProjectExist]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            parameter = new SqlParameter();
            parameter.ParameterName = "@ProjetId";
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Size = 50;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = pProjectId;
            cmd.Parameters.Add(parameter);

            // en secondes donc 5 minutes
            cmd.CommandTimeout = 300;

            string strProjet = "";
            con.Open();
            cmd.ExecuteNonQuery();
            strProjet = parameter.Value.ToString();

            if (strProjet != null) {
                if (strProjet.Length > 0) { blnProjetExist = true; }
                    }

            con.Close();

            return blnProjetExist;
        }

        [WebMethod]
        public BPR.BPRProjet.Model.ServeurInfo GetServerInfo(string pProjectId)
        {
            BPR.BPRProjet.Model.ServeurInfo serveurInfo = new BPR.BPRProjet.Model.ServeurInfo();
            var db = _entitiesBPR;
            var con = new SqlConnection(db.Database.Connection.ConnectionString);

            SqlParameter parameter = new SqlParameter();
            var cmd = new SqlCommand("[dbo].[usp_ObtenirServeurParProjet]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            parameter = new SqlParameter();
            parameter.ParameterName = "@ProjetId";
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Size = 50;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = pProjectId;
            cmd.Parameters.Add(parameter);

            // en secondes donc 5 minutes
            cmd.CommandTimeout = 300;

            con.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    serveurInfo.Id = Int32.Parse(dataReader["SER_Id"].ToString());
                    serveurInfo.Nom = dataReader["SER_Nom"].ToString();
                    serveurInfo.Ip = dataReader["SER_Ip"].ToString();
                    serveurInfo.Description = dataReader["SER_Description"].ToString();
                    serveurInfo.Compagnie = Int32.Parse(dataReader["SER_Compagnie".ToString()].ToString());
                    serveurInfo.Domaine = dataReader["SER_Domaine"].ToString();
                    serveurInfo.StartingPath = dataReader["SER_Chemin_Local"].ToString();
                    serveurInfo.RemotePath = dataReader["SER_Chemin_Partage"].ToString();
                    serveurInfo.ImpersonateUser = dataReader["SER_Utilisateur"].ToString();
                    serveurInfo.MapperSeulement = false;
                }
            }

            con.Close();

            return serveurInfo;
        }
    }
}
