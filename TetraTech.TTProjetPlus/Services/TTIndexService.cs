using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class TTIndexService
    {

        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();

        public int getUserGroups(string userId, List<string> listGroupsOnFolder)
        {
            //var groups = _entities.usp_Get_ADGroups_ForUser(userId).Where(p=>p.cn in )Select(p=>p.cn );
            var query = from p in _entities.usp_Get_ADGroups_ForUser(userId)
                        where listGroupsOnFolder.Contains(p.cn)
                        select p.cn;

            return query.Distinct().ToList().Count();
        }

        //public string getTreeFromDB(string newName)

        //{
        //    return _entities.TTIndex_Trees_Doc_Proj.Where(p => p.TreeName == newName).Select(p => p.Tree).FirstOrDefault();
        //}

        //public bool insertTreeInDB(string newName, string tree)

        //{
        //    try
        //    {
        //        TTIndex_Trees_Doc_Proj itemToInsert = new TTIndex_Trees_Doc_Proj();
        //        itemToInsert.TreeName = newName;
        //        itemToInsert.Tree = tree;
        //        _entities.TTIndex_Trees_Doc_Proj.Add(itemToInsert);
        //        _entities.SaveChanges();
        //        return true;

        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public List<string> getFilesExtensions()
        {

            DataSet ds = new DataSet();
            List<string> results = new List<string>();
            var con = new SqlConnection("data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework");

            var com = new SqlCommand();
            com.Connection = con;
            //com.CommandTimeout = 360;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "TTIndex_getProjectsList";
            var adapt = new SqlDataAdapter();
            adapt.SelectCommand = com;
            adapt.Fill(ds);


            con.Close();
            com.Dispose();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    results.Add(row["serverName"].ToString().TrimEnd().TrimStart() + " - " + row["folderName"].ToString().TrimEnd().TrimStart());
                }
            }
            return results;
        }

        public List<usp_TTIndex_getFiles_Result> returnListProjets(string pathKeyword, string fileKeyword, string dateCreation, string folder)// string[] extensionsArray,
        {
            //les dates
            var startDate = "";
            var endDate = "";

            if (dateCreation != "")
            {
                string[] dates = dateCreation.TrimStart().TrimEnd().Split(' ');
                startDate = dates[0];//.Remove(10);                  
                endDate = dates[2];//.Remove(0, 1);  
            }

            DataSet ds = new DataSet();
            List<usp_TTIndex_getFiles_Result> results = new List<usp_TTIndex_getFiles_Result>();
            var con = new SqlConnection("data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework");

            var com = new SqlCommand();
            com.Connection = con;
            var folderT = "";
            //com.CommandTimeout = 360;
            if (folder.Split('-').Count() > 2)
            {
                for (int i = 1; i < folder.Split('-').Count(); i++)
                {
                    folderT += folder.Split('-')[i] + "-";
                }
                folderT = folderT.Remove(folderT.Length - 1).TrimEnd().TrimStart();
            }


            else folderT = folder.Split('-')[1].TrimEnd().TrimStart();
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "usp_TTIndex_getFiles";
            com.Parameters.Add("@folder", SqlDbType.VarChar).Value = folderT;
            com.Parameters.Add("@folderKW", SqlDbType.VarChar).Value = pathKeyword;
            com.Parameters.Add("@fileKW", SqlDbType.VarChar).Value = fileKeyword;
            com.Parameters.Add("@dateStart", SqlDbType.VarChar).Value = startDate;
            com.Parameters.Add("@dateEnd", SqlDbType.VarChar).Value = endDate;
            var adapt = new SqlDataAdapter();
            adapt.SelectCommand = com;
            adapt.Fill(ds);


            con.Close();
            com.Dispose();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    usp_TTIndex_getFiles_Result record = new usp_TTIndex_getFiles_Result();
                    record.parentFolder = row["parentFolder"].ToString().TrimEnd().TrimStart();
                    record.fileName = row["fileName"].ToString().TrimEnd().TrimStart();
                    //string path = @"D:\Sites\Boomerang\files\phototheque\" + row["id_fiche"].ToString().TrimEnd().TrimStart();
                    if (folder != "")
                    {
                        try
                        {
                            if (record.parentFolder.Split('\\')[6] == folderT || record.parentFolder.Split('\\')[7] == folderT)
                            {
                                results.Add(record);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                    }
                    else
                        results.Add(record);
                }
            }
            return results.Distinct().ToList();
        }


        //public List<usp_TTIndex_getFiles2_Result> returnListProjets2(string pathKeyword="",string fileKeyword="",string dateCreation="",string folder="")
        //{
        //    //les dates
        //    var startDate = "";
        //    var endDate = "";

        //    if (dateCreation != "")
        //    {
        //        string[] dates = dateCreation.TrimStart().TrimEnd().Split(' ');
        //        startDate = dates[0];//.Remove(10);                  
        //        endDate = dates[2];//.Remove(0, 1);  
        //    }

        //    var folderT = "";
        //    //com.CommandTimeout = 360;
        //    if (folder.Split('-').Count() > 2)
        //    {
        //        for (int i = 1; i < folder.Split('-').Count(); i++)
        //        {
        //            folderT += folder.Split('-')[i] + "-";
        //        }
        //        folderT = folderT.Remove(folderT.Length - 1).TrimEnd().TrimStart();
        //    }
        //    else folderT = folder.Split('-')[1].TrimEnd().TrimStart();
        //    var liste = _entities.usp_TTIndex_getFiles2(folderT, pathKeyword, fileKeyword, startDate, endDate).ToList();
        //    return liste;
        //}

        public List<string> getSearchResults(string keyword, int startIndex, int endIndex)
        {
        //  var  list = _entities.TTIndex_FilesListDetailsTest.Where(p => p.filePath.Contains(keyword) && p.ItemID >= startIndex && p.ItemID < endIndex).Select(p => p.filePath).ToList();

           
            return null;
        }
    }
}