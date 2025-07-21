using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Exchange.WebServices.Data;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using System.Reflection;
using System.Data.SqlClient;

namespace TetraTech.TTProjetPlus.Services
{
    public class STService
    {
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();



        public string getEmpName(string numEmp)
        {
            return _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.EMPLOYEE_NUMBER == numEmp).Select(p => p.FULL_NAME).FirstOrDefault();
        }

        public List<string> GetGestionnairesNames715()
        {
            DateTime dt = DateTime.Now.Date;
            var listGest = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.EFFECTIVE_START_DATE <= dt && p.EFFECTIVE_END_DATE >= dt && p.ORG.Contains("715")).Select(p => p.FULL_NAME);// + " / " + p.EMPLOYEE_NUMBER);
            return listGest.Distinct().ToList();

        }


        public List<string> GetReportType_ST2()
        {
            var Type_ST = _entitiesSuiviMandat.ST_descriptions_listeEnvoie_types.Select(x => x.code).ToList();// + " / " + p.EMPLOYEE_NUMBER);
            return Type_ST.Distinct().ToList();

        }

        public List<ST_descriptions_listeEnvoie_types> GetReportType_ST()
        {
            var Type_ST = _entitiesSuiviMandat.ST_descriptions_listeEnvoie_types.ToList();// + " / " + p.EMPLOYEE_NUMBER);
            return Type_ST.Distinct().ToList();

        }




        public ST_liste_envois returnSendingTypeForEmp(string nom)
        {
            //recuperation du courriel:
            try
            {
                var email = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.FULL_NAME == nom).Select(p => p.EMAIL_ADDRESS).FirstOrDefault();
                var Type_ST = _entitiesSuiviMandat.ST_liste_envois.Where(p => p.Nom == email).ToList()[0];
                return Type_ST;

            }

            catch (Exception e)
            {
                return null;
            }
        }

        public bool isInSendTable(string empNom)
        {
            var mail = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.FULL_NAME == empNom).Select(x => x.EMAIL_ADDRESS).FirstOrDefault();
            var emp = _entitiesSuiviMandat.ST_liste_envois.Where(p => p.Nom == mail).FirstOrDefault();
            return emp == null ? false : true;
        }

        public string insertIntoSendListTable(string[] typesListe, string empNom)
        {
            try
            {
                var listeReports = GetReportType_ST();
                var mail = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.FULL_NAME == empNom).Select(x => x.EMAIL_ADDRESS).FirstOrDefault();

                //changer pour mise en prod
                 string connectionString = @"Data Source=TQCS349sql1\bprsql;Initial Catalog=SuiviMandat;persist security info=True;user id=TLX_USER;password=tlxuser!@#prd";

               // string connectionString = @"Data Source=TTS349TEST02;Initial Catalog=SuiviMandat;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //il faut fournir le courriel de l employé:
                    string sql = @"INSERT INTO [SuiviMandat].[dbo].[ST_liste_envois] ( [Nom], ";
                    var values = " values ( '" + mail + "' , ";


                    var arrayColumn = typesListe[0].Split(',');
                    for (int i = 0; i < arrayColumn.Length; i++)
                    {
                        var item2 = arrayColumn[i].Replace("\"", "").Replace("[", "").Replace("]", "");

                        if (i == (arrayColumn.Length - 1))
                        {
                            sql += "[" + item2 + "]";
                            values += " 'oui' ";

                        }
                        else
                        {
                            sql += "[" + item2 + "]" + ", ";
                            values += " 'oui' ,";

                        }

                    }
                    sql += " ) ";
                    values += " ) ";
                    var finalSql = sql + values;

                    SqlCommand command = new SqlCommand(finalSql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                    return "true";



                }

            }
            catch (Exception e)
            {
                var nessage = e.Message;
                return "false";
            }

        }



        public string modifiySendListTableForEmp(string[] typesListe, string empNom)
        {
            try
            {
                var listeReports = GetReportType_ST();
                //changer pour mise en prod
                //  string connectionString = @"Data Source=TQCS349sql1\bprsql;Initial Catalog=SuiviMandat;persist security info=True;user id=TLX_USER;password=tlxuser!@#prd";

                string connectionString = @"Data Source=TTS349TEST02;Initial Catalog=SuiviMandat;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO [SuiviMandat].[dbo].[ST_liste_envois] ( [Nom], ";
                    var values = " values ( '" + empNom + "' , ";

                    var arrayColumn = typesListe[0].Split(',');
                    for (int i = 0; i < arrayColumn.Length; i++)
                    {
                        var item2 = arrayColumn[i].Replace("\"", "").Replace("[", "").Replace("]", "");

                        if (i == (arrayColumn.Length - 1))
                        {
                            sql += "[" + item2 + "]";
                            values += " 'oui' ";

                        }
                        else
                        {
                            sql += "[" + item2 + "]" + ", ";
                            values += " 'oui' ,";

                        }

                    }
                    sql += " ) ";
                    values += " ) ";
                    var finalSql = sql + values;

                    SqlCommand command = new SqlCommand(finalSql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                    return "true";
                }

            }
            catch (Exception e)
            {
                var nessage = e.Message;
                return "false";
            }

        }



        public string removeSendListTableForEmp(string empNom)
        {
            try
            {
                //changer pour connection string prod:
               string connectionString = @"Data Source=TQCS349sql1\bprsql;Initial Catalog=SuiviMandat;persist security info=True;user id=TLX_USER;password=tlxuser!@#prd";
           //     string connectionString = @"Data Source=TTS349TEST02;Initial Catalog=SuiviMandat;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var mail = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.FULL_NAME == empNom).Select(x => x.EMAIL_ADDRESS).FirstOrDefault();
                    string sql = @"DELETE FROM  [SuiviMandat].[dbo].[ST_liste_envois] where Nom = '" + mail + "'";

                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                    return "true";
                }

            }
            catch (Exception e)
            {
                var nessage = e.Message;
                return "false";
            }

        }

    }
}