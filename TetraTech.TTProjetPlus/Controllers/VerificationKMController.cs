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
        public  List<List<string>> ListOf_liste_empOrg = new List<List<string>>();


        // GET: VerificationKM
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            //verificationKMModel model = new verificationKMModel();

            //var Liste_TableVerificationKM = _verificationKMService.returnVerificationKMList();
            //model.liste_TableVerificationKM = Liste_TableVerificationKM;
            //var Liste_ColumnsNames = _verificationKMService.returnVerificationKM_columnNames();
            //model.liste_Columns = Liste_ColumnsNames;
            List<DataTable> liste_dt = new List<DataTable>();

            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            string sqlQuery = "SELECT * FROM TableVerificationKM";  // Remplacez par le nom réel de votre table
            string sqlQuery2 = "SELECT * FROM TableChgtKM";

            var dt = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var adapter = new SqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dt);
                liste_dt.Add(dt);
            }

            var dt2 = new DataTable();
            using (var connection2 = new SqlConnection(connectionString))
            {
                connection2.Open();
                var adapter2 = new SqlDataAdapter(sqlQuery2, connection2);
                adapter2.Fill(dt2);
                liste_dt.Add(dt2);
            }

            ////creation dynamique d un dictionnaire des noms de colonnes
            //for(int j = 0; j< dt.Columns.Count;j++)
            //    {
            //      columnIndexNameDictionnay.Add(j, dt.Columns[j].ColumnName);
            //    }

            ////creation d une liste de [liste de emp et leur organisation associées]
            //for(int k = 0; k < dt.Rows.Count; k++)
            //    {
            //      List<string> list_EmpOrg = new List<string>();
            //                    for (int p = 0; p <dt.Rows[k].ItemArray.Count()-1; p++)
            //                    {

            //                        if(dt.Rows[k].ItemArray[p]!= null && dt.Rows[k].ItemArray[p].ToString() !="")
            //                        { 

            //                       list_EmpOrg.Add(dt.Rows[k].ItemArray[p].ToString() + "#" + columnIndexNameDictionnay.ElementAt(p).Value);

            //                        }
            //                    }
            //        ListOf_liste_empOrg.Add(list_EmpOrg);

            //    }


            return View(liste_dt);
        }


        public ActionResult performValidation()
        {
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

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
        public ActionResult SaveTableData([FromBody]  List<TableChange> changes)
        {
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
            conn.Open();
            foreach (var change in changes)
                {
                var employeeName = (change.NewValue == null || change.NewValue == "") ? change.OldValue : change.NewValue;
                string sqlQuery = "UPDATE [TTProjetPlus].[dbo].[TableVerificationKM] ";
                sqlQuery += " SET [" + change.ColumnName + "] = " +  (change.NewValue == null ? "' '" : " '" + change.NewValue.TrimStart() + "' ") ;
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


                    string sqlQuery2 = "INSERT INTO [TTProjetPlus].[dbo].[TableChgtKM]";
                    sqlQuery2 += "([DateChgt],[employeeNum],[Nom],[Prenom],[dateDebut],[dateFin],[Aouter/supprimer de l'Annexe A],[Org])";
                    sqlQuery2 += "VALUES ( '"+ dateChgt + "' , '" + employeeName.TrimStart().Split('-')[1] + "' , '" + employeeName.TrimStart().Split('-')[0].Split(' ')[0] + "' , '" + employeeName.TrimStart().Split('-')[0].Split(' ')[1] + "' , '" + dateDebut + "' , '" + dateFin + "' , '' , '' )";


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
           public string  role { get; set; }
            public string titre { get; set; }

        }


        public ActionResult AddColumnToTable(string columnName)
        {
            var tableName = "TableVerificationKM";
            var columnType = "NVARCHAR(255)";
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            string sql = $"ALTER TABLE [{tableName}] ADD [{columnName}] {columnType}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                   return  Json( "L'organisation "+  columnName + "a été ajoutée a la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                  return   Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }



        public ActionResult RemoveColumnFromTable(string columnName)
        {
            var tableName = "TableVerificationKM";
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
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
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            string sql = $"INSERT INTO [TableVerificationKM] ([Role], [Titre de l'employé_], [Nom de l'employé]) VALUES ( '" + KMRole + "' , '" + KMTitre + "' , '"+ KMName + "' )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return Json(KMName  +  "a été ajouté a la table.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Une erreur s'est produite. Veuillez aviser l'administrateur", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult RemoveKMFromTable(string KMName)
        {
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
            string sql = $"delete from  [TableVerificationKM] where [Nom de l'employé] = '" + KMName +"' ";

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
            string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";

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
                sqlQuery += " SET  [DateChgt] = '"+ dateChgt + "' , [employeeNum] = '" + employeeNum + "' ,[Nom] = '" +nom + "' , [Prenom] = '" + prenom  + "' ,[dateDebut] = '" + dateDebut + "' , [dateFin] ='" + dateFin + "' , [Aouter/supprimer de l'Annexe A] = '" + addRemove + "' , [Org] = '"+ org + "' ";
                sqlQuery += " WHERE [id] = " + Int32.Parse(id);

                   SqlCommand command = new SqlCommand(sqlQuery, connection); 
                   command.ExecuteNonQuery();
                }

            }
            return Json("Modifications sauvegardées !", JsonRequestBehavior.AllowGet);

        }

    }
}