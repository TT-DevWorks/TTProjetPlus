using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using static TetraTech.TTProjetPlus.Models.verificationKMModel;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class VerificationKMController : Controller
    {
        VeirificationKMService _verificationKMService = new VeirificationKMService();
        public DataTable TableVerificationKM { get; set; }
        Dictionary<int, string> columnIndexNameDictionnay = new Dictionary<int, string>();
        public List<List<string>> ListOf_liste_empOrg = new List<List<string>>();


        // GET: VerificationKM
        public ActionResult Index(string menuVisible = "true")
        {


            ViewBag.MenuVisible = menuVisible;
            List<DataTable> liste_dt = new List<DataTable>();
            //    string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            string sqlQuery = "SELECT * FROM TableVerificationKM  order by 2 , 3";  // Remplacez par le nom réel de votre table
            string sqlQuery2 = "SELECT * FROM TableChgtKM";
            string sqlQuery3 = "SELECT * FROM TableRoles";
            string sqlQuery4 = "SELECT * FROM TableTitres";
            //  string sqlQuery5 = "SELECT FIRST_NAME+ ' '+ LAST_NAME + ' - ' + [EMPLOYEE_NUMBER] as empData FROM [TTProjetPlus].[dbo].[EmployeeActif_Inactive] where EFFECTIVE_END_DATE > GETDATE()";
            string sqlQuery5 = "  select distinct [Nom de l'employé] , [Role],[Titre de l'employé_] FROM [TTProjetPlus].[dbo].[TableVerificationKM]order by[Nom de l'employé]";
            var dt = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var adapter = new SqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dt);
                liste_dt.Add(dt);
                //}

                var dt2 = new DataTable();
                //using (var connection2 = new SqlConnection(connectionString))
                //{
                //    connection2.Open();
                var adapter2 = new SqlDataAdapter(sqlQuery2, connection);
                adapter2.Fill(dt2);
                liste_dt.Add(dt2);
                //}


                var dt3 = new DataTable();
                //using (var connection3 = new SqlConnection(connectionString))
                //{
                //connection3.Open();
                var adapter3 = new SqlDataAdapter(sqlQuery3, connection);
                adapter3.Fill(dt3);
                liste_dt.Add(dt3);


                var dt4 = new DataTable();
                //using (var connection3 = new SqlConnection(connectionString))
                //{
                //connection3.Open();
                var adapter4 = new SqlDataAdapter(sqlQuery4, connection);
                adapter4.Fill(dt4);
                liste_dt.Add(dt4);

                var dt5 = new DataTable();
                //using (var connection3 = new SqlConnection(connectionString))
                //{
                //connection3.Open();
                var adapter5 = new SqlDataAdapter(sqlQuery5, connection);
                adapter5.Fill(dt5);
                liste_dt.Add(dt5);

            }



            return View(liste_dt);
        }


        public ActionResult performValidation()
        {
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            string sqlQuery = "SELECT * FROM TableVerificationKM";  // Remplacez par le nom réel de votre table

            var dt = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var adapter = new SqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dt);
            }

            //creation dynamique d un dictionnaire des noms de colonnes
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                columnIndexNameDictionnay.Add(j, dt.Columns[j].ColumnName);
            }

            //creation d une liste de [liste de emp et leur organisation associées]
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                List<string> list_EmpOrg = new List<string>();
                for (int p = 0; p < dt.Rows[k].ItemArray.Count() - 1; p++)
                {

                    if (dt.Rows[k].ItemArray[p] != null && dt.Rows[k].ItemArray[p].ToString() != "")
                    {

                        list_EmpOrg.Add(dt.Rows[k].ItemArray[p].ToString() + "#" + dt.Rows[k].ItemArray[1].ToString() + "#" + columnIndexNameDictionnay.ElementAt(p).Value);

                    }
                }
                ListOf_liste_empOrg.Add(list_EmpOrg);

            }
            var result = _verificationKMService.performValidation(ListOf_liste_empOrg);

            //envoie de courriel
            var envoiCourriel = _verificationKMService.sendEmail(result);


            return Json("success", JsonRequestBehavior.AllowGet);

        }

        [System.Web.Http.HttpPost]
        public ActionResult SaveTableData([FromBody] List<TableChange> changes)
        {
            //string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (var change in changes)
                {
                    var employeeName = (change.NewValue == null || change.NewValue == "") ? change.OldValue : change.NewValue;
                    string sqlQuery = "UPDATE [TTProjetPlus].[dbo].[TableVerificationKM] ";
                    sqlQuery += " SET [" + change.ColumnName + "] = " + (change.NewValue == null ? "' '" : " '" + change.NewValue.TrimStart() + "' ");
                    sqlQuery += " WHERE [Nom de l'employé] = '" + employeeName.TrimStart() + "' ";

                    var dateDebut = "";
                    var dateFin = "";

                    if (change.NewValue == null)
                    {
                        dateDebut = "";
                        dateFin = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dateDebut = DateTime.Now.ToString("yyyy-MM-dd");
                        dateFin = "";
                    }
                    var dateChgt = DateTime.Now.ToString("yyyy-MM-dd");

                    int index = employeeName.IndexOf('-');

                    if (index >= 0)
                    {
                        employeeName = employeeName.Remove(index, 1).Insert(index, "_");
                    }
                    string sqlQuery2 = "INSERT INTO [TTProjetPlus].[dbo].[TableChgtKM]";
                    sqlQuery2 += "([DateChgt],[employeeNum],[Nom],[Prenom],[dateDebut],[dateFin],[Aouter/supprimer de l'Annexe A],[Org])";
                    sqlQuery2 += "VALUES ( '" + dateChgt + "' , '" + employeeName.TrimStart().Split('-')[1] + "' , '" + employeeName.TrimStart().Split('-')[0].Split(' ')[0] + "' , '" + employeeName.TrimStart().Split('-')[0].Split(' ')[1] + "' , '" + dateDebut + "' , '" + dateFin + "' , '' , '' )";


                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd2 = new SqlCommand(sqlQuery2, conn))
                    {
                        int rowsAffected = cmd2.ExecuteNonQuery();
                    }


                }
            }
            return Json(new { success = true });
        }

        public class TableChange
        {
            public int RowIndex { get; set; }
            public string ColumnName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public string empName { get; set; }
            public string role { get; set; }
            public string titre { get; set; }

        }


        public ActionResult AddColumnToTable(string columnName)
        {
            var tableName = "TableVerificationKM";
            var columnType = "NVARCHAR(255)";
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            string sql = $"ALTER TABLE [{tableName}] ADD [{columnName}] {columnType}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return Json("L'organisation " + columnName + "a été ajoutée a la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }



        public ActionResult RemoveColumnFromTable(string columnName)
        {
            var tableName = "TableVerificationKM";
            //string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";



            string sql = $"ALTER TABLE [{tableName}] DROP COLUMN [{columnName}]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return Json("L'organisation " + columnName + "a été retirée de la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult AddKMToTable(string KMName, string KMRole, string KMTitre)
        {
            //string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            string sql = $"INSERT INTO [TableVerificationKM] ([Role], [Titre de l'employé_], [Nom de l'employé]) VALUES ( '" + KMRole + "' , '" + KMTitre + "' , '" + KMName + "' )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return Json(KMName + "a été ajouté a la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult RemoveKMFromTable(string KMName, List<string> selectedValues, bool flag)
        {
            //string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            string sql = "";
            string emp = KMName.Split('/')[0].TrimStart().TrimEnd();

            if (flag == true)
            {
                sql = $"delete from  [TableVerificationKM] where [Nom de l'employé] = '" +emp + "' and [Role]= '" + KMName.Split('/')[1].TrimStart().TrimEnd() + "' and [Titre de l'employé_] = '" + KMName.Split('/')[2].TrimStart().TrimEnd() + "' ";
 
            }
            else
            {
        // UPDATE table_name  SET column1 = value1, column2 = value2, ...WHERE condition;

                sql = $"update [TableVerificationKM] set";
            
            for(int i= 0; i < selectedValues.Count; i++)
            {
                if (i == selectedValues.Count - 1)
                {
                sql += " [" + selectedValues[i] + "] = NULL  where [Nom de l'employé] = '" + emp + "' and [Role]= '" + KMName.Split('/')[1].TrimStart().TrimEnd() + "' and [Titre de l'employé_] = '" + KMName.Split('/')[2].TrimStart().TrimEnd() + "'"; ;
                }
                else
                {
                sql += " [" + selectedValues[i]+ "] = NULL , "; 
                }


            }
       }
        
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return Json(KMName + " a été retirée de la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult SaveTableDataTab2(List<Dictionary<string, string>> changedRows)
        {
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var row in changedRows)
                {

                    var dateChgt = DateTime.Now.ToString("yyyy-MM-dd");
                    string employeeNum = row.Values.ElementAt(1);
                    string nom = row.Values.ElementAt(2);
                    string prenom = row.Values.ElementAt(32);
                    string dateDebut = row.Values.ElementAt(4);
                    string dateFin = row.Values.ElementAt(5);
                    string addRemove = row.Values.ElementAt(6);
                    string org = row.Values.ElementAt(7);
                    string id = row.Values.ElementAt(8);
                    string sqlQuery = "UPDATE [TTProjetPlus].[dbo].[TableChgtKM] ";
                    sqlQuery += " SET  [DateChgt] = '" + dateChgt + "' , [employeeNum] = '" + employeeNum + "' ,[Nom] = '" + nom + "' , [Prenom] = '" + prenom + "' ,[dateDebut] = '" + dateDebut + "' , [dateFin] ='" + dateFin + "' , [Aouter/supprimer de l'Annexe A] = '" + addRemove + "' , [Org] = '" + org + "' ";
                    sqlQuery += " WHERE [id] = " + Int32.Parse(id);

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.ExecuteNonQuery();
                }

            }
            return Json("Modifications sauvegardées !", JsonRequestBehavior.AllowGet);

        }



        public ActionResult AddRole(string role)
        {
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 1. Insert the role
                    string insertSql = "INSERT INTO [TableRoles] ([role]) VALUES (@role)";
                    using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@role", role);
                        insertCmd.ExecuteNonQuery();
                    }

                    // 2. Retrieve updated list
                    List<string> roles = new List<string>();
                    string selectSql = "SELECT [role] FROM [TableRoles] ";
                    using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(0));
                        }
                    }

                    // 3. Return updated list
                    return Json(roles, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { error = "Une erreur s'est produite. Veuillez aviser l'administrateur" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult AddTitre(string titre)
        {
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 1. Insert the role
                    string insertSql = "INSERT INTO [TableTitres] ([Titres]) VALUES (@titre)";
                    using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@titre", titre);
                        insertCmd.ExecuteNonQuery();
                    }

                    // 2. Retrieve updated list
                    List<string> roles = new List<string>();
                    string selectSql = "SELECT [Titres] FROM [TableTitres] ";
                    using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(0));
                        }
                    }

                    // 3. Return updated list
                    return Json(roles, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { error = "Une erreur s'est produite. Veuillez aviser l'administrateur" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult AddKMToTable2(string KMName, string KMRole, string KMTitre, List<string> KMDepartments)
        {
            // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            //ATTENTION CHNAGER POUR LA MISE EN PROD!!!
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            string sql = "";
            try
            {
                if (KMDepartments.Count == 1 && KMDepartments[0] == "Toutes")
                {
                    string sqlColumnCount = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TableVerificationKM' ";
                    int columnCount = 0;
                    using (var connection = new SqlConnection(connectionString))
                    using (var command = new SqlCommand(sqlColumnCount, connection))
                    {
                        connection.Open();
                        columnCount = (int)command.ExecuteScalar();
                    }

                    string fieldIteration = "";
                    for (int i = 3; i < columnCount - 1; i++)
                    {
                        if (i == columnCount - 2)
                        {
                            fieldIteration += "'" + KMName + "'";
                        }
                        else
                        {
                            fieldIteration += "'" + KMName + "'" + ", ";
                        }

                    }
                    sql = @"INSERT INTO [TableVerificationKM]   VALUES ( '" + KMRole + "' , '" + KMTitre + "' , '" + KMName + "' , " + fieldIteration + ");";

                    using (var connection = new SqlConnection(connectionString))
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }

                }


                else
                {

                    // 1️⃣ Insert main KM record
                    string sql2 = "";
                    string sqlDept = "";
                    string fieldIteration2 = "";
                    for (int p = 0; p < KMDepartments.Count; p++)
                    {

                        if (p == KMDepartments.Count - 1)
                        {
                            sqlDept += " [" + KMDepartments[p] + "] ";
                            fieldIteration2 += " '" + KMName + "' ";
                        }
                        else
                        {
                            sqlDept += " [" + KMDepartments[p] + "] " + ", ";
                            fieldIteration2 += " '" + KMName + "' " + ", ";
                        }


                    }


                    sql2 = @"INSERT INTO [TableVerificationKM]  ( [Role], [Titre de l'employé_], [Nom de l'employé], " + sqlDept + " )   VALUES  ( '" + KMRole + "' , '" + KMTitre + "' , '" + KMName + "' , " + fieldIteration2 + ");";


                    using (var connection = new SqlConnection(connectionString))
                    using (var command = new SqlCommand(sql2, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }

                }

                return Json(KMName + " a été ajouté à la table.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // TODO: log ex for debugging
                return Json(new { success = false, message = "Une erreur s'est produite. Veuillez aviser l'administrateur." }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult GetSecondList(string searchValue)
        {

            var searchName = searchValue.Split('/')[0].Trim();
            var searchRole = searchValue.Split('/')[1].Trim();
            var searchTitre = searchValue.Split('/')[2].Trim();

            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            List<string> matchingColumns = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.SearchColumnsForValue", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add all three parameters
                    command.Parameters.AddWithValue("@SearchValue", searchName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SearchRole", searchRole ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SearchTitre", searchTitre ?? (object)DBNull.Value);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                matchingColumns.Add(reader["ColumnName"].ToString());
                            }
                        }

                        return Json(matchingColumns, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        // Optionally log ex.Message
                        return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                    }
                }
            }

        }
    }

}