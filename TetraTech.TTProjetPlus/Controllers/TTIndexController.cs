
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;


namespace TetraTech.TTProjetPlus.Controllers
{
    public class TTIndexController : Controller
    {
        //public  ConnectionToEs _connectionToEs;
        private readonly TTIndexService _TTIndexService = new TTIndexService();
        public string sFilePath = HttpRuntime.AppDomainAppPath;
        public static List<string> listProjects = new List<string>();
        public  int position = 0;
        public string hasChanged = "non";
        public ActionResult Index(string menuVisible = "true")
        {
            listProjects = _TTIndexService.getFilesExtensions();


            ViewBag.MenuVisible = menuVisible;
            return View();
        }

        public ActionResult submitSearch(string pathKeyword = "", string fileKeyword = "", string[] extensionsArray = null, string folder ="", string dateCreation = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //les intervales de dates:
          //  var listProjects = _TTIndexService.returnListProjets(pathKeyword, fileKeyword, dateCreation, folder); //extensionsArray,
                                                                                                                  // var lineCount = System.IO.File.ReadLines(@"C:\Users\Frederic.Ohnona\Desktop\TestList2.txt").Count();
            var listProjet2 = _TTIndexService.returnListProjets2(pathKeyword, fileKeyword, dateCreation, folder);

            ResultModel model = new ResultModel();
            // model.result = listProjects;
            model.result2 = listProjet2;
            try
            {
                model.elapsedTime = (int)(stopwatch.ElapsedMilliseconds / 1000);
                return PartialView("_searchResultsPartial", model);
            }


            catch (Exception e)
            {
                return null;
            }

            

        }
        //version avec ES
        [HttpGet]
        public ActionResult SearchES()
        {
            //ElasticSearch (ES)
            

            return View("_searchResultsPartialES");
        }

        //elasticsearch
        //public JsonResult DataSearch(string queryfol, string queryfil, string hasChanged="non")
        //{
        //    try
        //    {
        //        _connectionToEs = new ConnectionToEs();
        //        if (!string.IsNullOrEmpty(queryfol) && !string.IsNullOrEmpty(queryfil))
        //        {
        //            var responsedata = _connectionToEs.EsClient().Search<ttindexsearch>(s => s
        //                             .Index("ttindexsearch")
        //                             .Size(50)
        //                             .Query(q => q
        //                                 .Match(m => m
        //                                     .Field(f => f.parentfolder)
        //                                     .Query(queryfol)
        //                                 )
        //                                 && q
        //                                 .Match(m => m
        //                                     .Field(f => f.filename)
        //                                     .Query(queryfil)
        //                             ))
        //                         );

        //            var datasend = (from hits in responsedata.Hits
        //                            select hits.Source).ToList();

        //            return Json(new { datasend, responsedata.Took }, behavior: JsonRequestBehavior.AllowGet);

        //        }
        //        else if (!string.IsNullOrEmpty(queryfol))
        //        {
                   
        //            var responsedata = _connectionToEs.EsClient().Search<ttindexsearch>(s => s
        //                        .Index("ttindexsearch")
        //                        .From(position)
        //                        .Size(5000)
        //                        .Query(q => q
        //                                .Match(m => m
        //                                    .Field(f => f.parentfolder)
        //                                    .Query(queryfol)
        //                                ))
                                       
        //                                );


        //            var datasend = (from hits in responsedata.Hits
        //                            select hits.Source).ToList();

        //            //////////
        //            //
        //            var total = responsedata.Hits.Count;

        //            return Json(new { datasend, responsedata.Took , total, }, behavior: JsonRequestBehavior.AllowGet);
        //        }
        //        else if (!string.IsNullOrEmpty(queryfil))
        //        {
        //            var responsedata = _connectionToEs.EsClient().Search<ttindexsearch>(s => s
        //                            .Index("ttindexsearch")
        //                            .Size(50)
        //                            .Query(q => q
        //                                .Match(m => m
        //                                    .Field(f => f.filename)
        //                                    .Query(queryfil)
        //                                )));
        //            var datasend = (from hits in responsedata.Hits
        //                            select hits.Source).ToList();

        //            return Json(new { datasend, responsedata.Took }, behavior: JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(data: null, behavior: JsonRequestBehavior.AllowGet);
        //    }

        //    catch (Exception e)
        //    {
        //        return null;
        //    }

           

        //}

        public ActionResult checkPermission(string pathToCheck)
        {
            //LA VALDIATION DES PERMISSIONS: SUSPENDUE POUR LE MOMENT
            var pathFinal = pathToCheck.Replace(@"\", @"//");
            pathFinal = pathFinal.Replace("\n", "");

            DirectoryInfo dirInfo = null;
            DirectorySecurity dirSec = null;

            //aller lire dans la table la colonne [Security]
            try
            {
                dirInfo = new DirectoryInfo(pathFinal);
                dirSec = dirInfo.GetAccessControl();
            }

            catch (Exception e)
            {
                return Json(new { result = "false" }, JsonRequestBehavior.AllowGet);
            }
            List<string> listGroupsOnFolder = new List<string>();
            foreach (FileSystemAccessRule rule in dirSec.GetAccessRules(true, true, typeof(NTAccount)))
            {

                var d = rule.AccessControlType == AccessControlType.Allow ? "grants" : "denies";
                var t = rule.FileSystemRights;
                var u = rule.IdentityReference.ToString();
                listGroupsOnFolder.Add(u.Replace("TT\\", ""));
            }
            var userGroups = _TTIndexService.getUserGroups(UserService.CurrentUser.EmployeeId, listGroupsOnFolder);


            if (userGroups > 0)
            {
                return Json(new { result = "true" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { result = "false" }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult checkIfDirectoryOnDisk(string path)
        {
            if (Directory.Exists(path))
            {
                return Json(new { exist = "yes" }, JsonRequestBehavior.AllowGet);



            }
            return Json(new { exist = "no" }, JsonRequestBehavior.AllowGet);
        }


        //arbre""///////////////////////////////////////

        public ActionResult returnTree(string path, bool levelUp = false)
        {
            try
            {


                var codeHTML = "";
                List<string> listItem = new List<string>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                if (levelUp == true)
                {
                    int idy = path.LastIndexOf('\\');
                    path = path.Substring(0, idy);
                }


                int idx = path.LastIndexOf('\\');
                //var newName = path.Replace('\\', '_');
                //if(path.Substring(idx + 1)== "DOC-PROJ")
                //{
                // //si l'arbre existe dans la bd , il va etre restitué a present:
                //    var tree = _TTIndexService.getTreeFromDB(newName);
                //    if(tree!=null && tree !="")
                //    {
                //        var jsonResult0 = Json(new { result = tree, newPath = path }, JsonRequestBehavior.AllowGet);
                //        jsonResult0.MaxJsonLength = int.MaxValue;
                //        return jsonResult0;
                //    }
                //}
                //sinon, on va le créer et l'inserer dans la bd:
                listItem = GetSubDirectories(path);
                codeHTML += "<ul id='myUL'>";
                codeHTML += "<li><span class='caret2' id='" + path + "'>" + path.Substring(idx + 1) + "</span>";
                foreach (string item in listItem)
                {
                    int idy = item.LastIndexOf('\\');
                    codeHTML += item;
                }
                codeHTML += "</li>";
                codeHTML += "</ul>";

                //insertion dans la bd:
                //if (path.Substring(idx + 1) == "DOC-PROJ")
                //{
                //    var inserted = _TTIndexService.insertTreeInDB(newName, codeHTML);

                //}
                stopwatch.Stop();
                var jsonResult = Json(new { result = codeHTML, newPath = path }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<string> GetSubDirectories(string root)

        {
            List<string> listItem = new List<string>();
            listItem.Add("<ul class='nested'>");

            // Get all subdirectories
            List<string> subdirectoryEntries = Directory.GetDirectories(root).ToList();

            subdirectoryEntries.Sort((s1, s2) =>
            {
                string pattern = "([A-Za-z])([0-9]+)";
                string h1 = Regex.Match(s1, pattern).Groups[1].Value;
                string h2 = Regex.Match(s2, pattern).Groups[1].Value;
                if (h1 != h2)
                    return h1.CompareTo(h2);
                string t1 = Regex.Match(s1, pattern).Groups[2].Value;
                string t2 = Regex.Match(s2, pattern).Groups[2].Value;
                return int.Parse(t1).CompareTo(int.Parse(t2));
            });

            foreach (string subdirectory in subdirectoryEntries)
            {
                //on ne montre pas les  fichiers des sous repertoires...
                //List<string> filesEntries = Directory.GetFiles(subdirectory).ToList();
                //filesEntries.Sort((s1, s2) =>
                //{
                //    string pattern = "([A-Za-z])([0-9]+)";
                //    string h1 = Regex.Match(s1, pattern).Groups[1].Value;
                //    string h2 = Regex.Match(s2, pattern).Groups[1].Value;
                //    if (h1 != h2)
                //        return h1.CompareTo(h2);
                //    string t1 = Regex.Match(s1, pattern).Groups[2].Value;
                //    string t2 = Regex.Match(s2, pattern).Groups[2].Value;
                //    return int.Parse(t1).CompareTo(int.Parse(t2));
                //});

                int idx = 0;
                idx = subdirectory.LastIndexOf('\\');
                listItem.Add("<li><span class='caret2' id='" + subdirectory + "' onclick='displaySubDirContent(this.id)'>" + subdirectory.Substring(idx + 1) + "</span>");
                listItem.Add("<ul class='nested'>");
                //if (filesEntries.Count > 0)
                //{
                //    foreach (string file in filesEntries)
                //    {
                //        idx = file.LastIndexOf('\\');
                //        listItem.Add("<li><a href='#' onclick='window.open(\"" + file + "\")' >" + file.Substring(idx + 1) + "</a></li>");

                //    }
                //}

                //on ne va pas chercher les sous reperoir non plus
                // listItem.AddRange(LoadSubDirs(subdirectory));
                listItem.Add("</ul>");
                listItem.Add("</li>");
            }

            //...on ne montre que les fichiers a la recine de ce sous dossier
            List<string> filesEntries2 = Directory.GetFiles(root).ToList();
            filesEntries2.Sort((s1, s2) =>
            {
                string pattern = "([A-Za-z])([0-9]+)";
                string h1 = Regex.Match(s1, pattern).Groups[1].Value;
                string h2 = Regex.Match(s2, pattern).Groups[1].Value;
                if (h1 != h2)
                    return h1.CompareTo(h2);
                string t1 = Regex.Match(s1, pattern).Groups[2].Value;
                string t2 = Regex.Match(s2, pattern).Groups[2].Value;
                return int.Parse(t1).CompareTo(int.Parse(t2));
            });
            // listItem.Add("<ul class='nested'>");
            foreach (string file in filesEntries2)

            {

                int idw = 0;
                idw = root.LastIndexOf('\\');
                idw = file.LastIndexOf('\\');
              //  listItem.Add("<li><a  onclick='displayDoc(\"" + file.Replace('\\', '|') + "\")' >" + file.Substring(idw + 1) + "</a></li>");
                listItem.Add("<li><a  href='" + file.Replace("\\", "\\\\") + "'  target='_blank'>" + file.Substring(idw + 1) + "</a></li>");

            }

            // listItem.Add("</ul>");

            return listItem;
        }

        public static List<string> LoadSubDirs(string dir)

        {
            List<string> listItem = new List<string>();
            // Console.WriteLine(dir);

            List<string> subdirectoryEntries = Directory.GetDirectories(dir).ToList();
            subdirectoryEntries.Sort((s1, s2) =>
            {
                string pattern = "([A-Za-z])([0-9]+)";
                string h1 = Regex.Match(s1, pattern).Groups[1].Value;
                string h2 = Regex.Match(s2, pattern).Groups[1].Value;
                if (h1 != h2)
                    return h1.CompareTo(h2);
                string t1 = Regex.Match(s1, pattern).Groups[2].Value;
                string t2 = Regex.Match(s2, pattern).Groups[2].Value;
                return int.Parse(t1).CompareTo(int.Parse(t2));
            });
            int idx = 0;
            foreach (string subdirectory in subdirectoryEntries)

            {
                idx = subdirectory.LastIndexOf('\\');
                listItem.Add("<li><span class='caret2' id='" + subdirectory + "' onclick='displaySubDirContent(this.id)'>" + subdirectory.Substring(idx + 1) + "</span>");
                listItem.Add("<ul class='nested'>");


                ///////
                List<string> filesEntries = Directory.GetFiles(subdirectory).ToList();
                filesEntries.Sort((s1, s2) =>
                {
                    string pattern = "([A-Za-z])([0-9]+)";
                    string h1 = Regex.Match(s1, pattern).Groups[1].Value;
                    string h2 = Regex.Match(s2, pattern).Groups[1].Value;
                    if (h1 != h2)
                        return h1.CompareTo(h2);
                    string t1 = Regex.Match(s1, pattern).Groups[2].Value;
                    string t2 = Regex.Match(s2, pattern).Groups[2].Value;
                    return int.Parse(t1).CompareTo(int.Parse(t2));
                });

                int idy = 0;
                idx = subdirectory.LastIndexOf('\\');
                {
                    foreach (string file in filesEntries)
                    {
                        idy = file.LastIndexOf('\\');
                        listItem.Add("<li><a href='" + file + "' target='_blank'>" + file.Substring(idy + 1) + "</a></li>");

                    }
                }
                //////
                listItem.AddRange(LoadSubDirs(subdirectory));
                listItem.Add("</ul>");
                listItem.Add("</li>");


            }
            return listItem;
        }

       public ActionResult downloadDoc(string path= "\\\\tts349arc1\\Projets\\Tts354fs1\\65677M227\\DOC-PROJ\\10\\10FH\\15042712.pdf")
        {
            
            try
            {
               int idw = path.LastIndexOf('\\');
                WebClient webClient = new WebClient();
              // webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                webClient.DownloadFile(new Uri(path), @"C:\Users\Frederic.Ohnona\Documents\"+ path.Substring(idw + 1));
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message , details2 = e.InnerException  }, JsonRequestBehavior.AllowGet);

            }
        }

       
    }
}

//ligne de commande pour creer fichier dictionnaire: dir \\tts349arc1\e$\Projets\TTS359FS1\ /b /s /A-D /o:gn>>C:\Users\Frederic.Ohnona\Desktop\TestList2.txt
//bulk insert dans la bd : bcp [TTProjetPlus].[dbo].[TTIndex_FilesList] in c:\Users\Frederic.Ohnona\Desktop\TestList2.txt -c -C 65001 -r \n -T -S tts349test02
//formatfile: bcp TTProjetPlus.[dbo].TTIndex_FilesList format nul -c -f c:\Users\Frederic.Ohnona\Desktop\TTIndex.fmt -t, -T  -S tts349test02
//pour arbre, voir: https://forums.asp.net/t/2054929.aspx?populate+treeview+with+network+drive+folder+structure+using+populateOndemand
//https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/creating-an-explorer-style-interface-with-the-listview-and-treeview?view=netframeworkdesktop-4.8
//pour une mise a jour du fichier d index avec les derneirs dossiers ajoutés a l archive voir ici:/https://docs.microsoft.com/en-US/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree
/*
 * using System;
using System.Collections.Generic;
using System.Data.OleDb;
//https://docs.microsoft.com/en-US/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree
namespace usingWindowsSearchService
 */



