using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Services;
using TetraTech.TTProjetPlus.Security;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class LecteurReseauController : ControllerBase
    {
        // GET: LecteurReseau
        public static List<structureItem> ListeServers = new List<structureItem>();
        public static List<structureItem> ListeServersSpecial = new List<structureItem>();
        private readonly LecteurReseauService _LecteurReseau = new LecteurReseauService();
        public string sFilePath = HttpRuntime.AppDomainAppPath;

        public ActionResult Index(string displayRetour = "true", string menuVisible = "true")
        {
            //ListeServers = _LecteurReseau.getServersName();
            //ListeServersSpecial = _LecteurReseau.getServersNameSpecial();
            ViewBag.displayRetour = displayRetour;
            ViewBag.MenuVisible = menuVisible;
            string cheminRepertoire = UserService.LecteurReseau();//@"\\etss349arc1.tt.local\Trans\Raccourcis_Projets";
            string[] fichiersEtSousRepertoires = Directory.GetFileSystemEntries(cheminRepertoire);

            return View(fichiersEtSousRepertoires);
            //return View();
        }


        public class MonModeleLecteurSpecial
        {
            public string LecteurSpecialMontrealPrj_Usine_3D { get; set; }
            public string LecteurSpecialQuebecTTIPrj_Usine_3D { get; set; }
            public string LecteurSpecialMontrealSuncor { get; set; }            
            public string LecteurSpecialJonquierePrj_Usine_3D { get; set; }
        }

        public ActionResult IndexSpecial(string displayRetourS = "true", string menuVisible = "true")
        {
            //ListeServers = _LecteurReseau.getServersName();
            //ListeServersSpecial = _LecteurReseau.getServersNameSpecial();
            ViewBag.displayRetourS = displayRetourS;
            ViewBag.MenuVisible = menuVisible;

            MonModeleLecteurSpecial modele = new MonModeleLecteurSpecial
            {
                LecteurSpecialMontrealPrj_Usine_3D = UserService.LecteurSpecialMontrealPrj_Usine_3D(),
                LecteurSpecialQuebecTTIPrj_Usine_3D = UserService.LecteurSpecialQuebecTTIPrj_Usine_3D(),
                LecteurSpecialMontrealSuncor = UserService.LecteurSpecialMontrealSuncor(),
                LecteurSpecialJonquierePrj_Usine_3D = UserService.LecteurSpecialJonquierePrj_Usine_3D()
            };
            return View(modele);
        }



        public ActionResult createBatchFile(string serverAddress)
        {
            if (serverAddress.Contains('*') == true)
            {
                var address = serverAddress.Split('*')[0];
                var drive = serverAddress.Split('*')[1];
                try
                {
                    var sFileName = "TTMapDrive.bat";

                    // Set a variable to the Documents path.
                    //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


                    using (StreamWriter sw = new StreamWriter(Path.Combine(sFilePath, sFileName)))
                    {
                        //Nouveau fichier BAT
                        sw.WriteLine("@echo off");
                        sw.WriteLine("");
                        sw.WriteLine("REM - Vérification si le lecteur est mappé");
                        sw.WriteLine("REM - Si il n'est pas mappé on map le lecteur sur le serveur");
                        sw.WriteLine(String.Format("net use {0}:", drive));
                        sw.WriteLine("if  errorlevel  1  goto  Map");
                        sw.WriteLine("");
                        sw.WriteLine("REM - On essaye de supprimer le map du lecteur");
                        sw.WriteLine("REM - Si il y a un erreur on avertis l'utilisateur de fermer le dossier");
                        sw.WriteLine(String.Format("net use {0}: /delete", drive));
                        sw.WriteLine("if  errorlevel  1  goto  Cannot");
                        sw.WriteLine("");
                        sw.WriteLine("REM - Si le delete se passe bien, on map le lecteur sur le serveur");
                        sw.WriteLine("GoTo map");
                        sw.WriteLine("goto  end");
                        sw.WriteLine("");
                        sw.WriteLine(":error");
                        sw.WriteLine("REM - Message d'erreur générique");
                        sw.WriteLine("cls");
                        sw.WriteLine("echo  Contacter le CSTI / Contact CSTI (1-866-208-5234)");
                        sw.WriteLine("pause");
                        sw.WriteLine("goto end");
                        sw.WriteLine("");
                        sw.WriteLine(":Cannot");
                        sw.WriteLine("REM - Message d'erreur");
                        sw.WriteLine("cls");
                        sw.WriteLine(String.Format("echo	Veuillez S.V.P. fermer tous vos fichiers et dossiers sur le lecteur {0}: , afin que vous puissiez vous connecter à un autre serveur.", drive));
                        sw.WriteLine("echo	-- --");
                        sw.WriteLine(String.Format("echo	Please close all your documents and folders on the {0}: drive, so that you can connect to another server.", drive));
                        sw.WriteLine("pause");
                        sw.WriteLine("goto end");
                        sw.WriteLine("");
                        sw.WriteLine(":Map");
                        sw.WriteLine("REM - Map le lecteur sur le serveur");
                        sw.WriteLine(String.Format("net use {1}: {0} /persistent:yes", address, drive));
                        sw.WriteLine("if  errorlevel  1  goto  error");
                        sw.WriteLine("goto end");
                        sw.WriteLine(":End");
                        sw.WriteLine("");
                        sw.Close();

                    }

                    string contentType = "text/plain";
                    return  File(sFilePath + "\\" + sFileName, contentType, "TTMapDrive.bat");
                    
                }
                catch (Exception e)
                {
                    return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                try
                {

                    var sFileName = "TTMapDrive.bat";

                    // Set a variable to the Documents path.
                    //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


                    using (StreamWriter sw = new StreamWriter(Path.Combine(sFilePath, sFileName)))
                    {
                        //Nouveau fichier BAT
                        sw.WriteLine("@echo off");
                        sw.WriteLine("");
                        sw.WriteLine("REM - Vérification si le lecteur est mappé");
                        sw.WriteLine("REM - Si il n'est pas mappé on map le lecteur sur le serveur");
                        sw.WriteLine("net use O:");
                        sw.WriteLine("if  errorlevel  1  goto  Map");
                        sw.WriteLine("");
                        sw.WriteLine("REM - On essaye de supprimer le map du lecteur");
                        sw.WriteLine("REM - Si il y a un erreur on avertis l'utilisateur de fermer le dossier");
                        sw.WriteLine("net use O: /delete");
                        sw.WriteLine("if  errorlevel  1  goto  Cannot");
                        sw.WriteLine("");
                        sw.WriteLine("REM - Si le delete se passe bien, on map le lecteur sur le serveur");
                        sw.WriteLine("GoTo map");
                        sw.WriteLine("goto  end");
                        sw.WriteLine("");
                        sw.WriteLine(":error");
                        sw.WriteLine("REM - Message d'erreur générique");
                        sw.WriteLine("cls");
                        sw.WriteLine("echo  Contacter le CSTI / Contact CSTI (1-866-208-5234)");
                        sw.WriteLine("pause");
                        sw.WriteLine("goto end");
                        sw.WriteLine("");
                        sw.WriteLine(":Cannot");
                        sw.WriteLine("REM - Message d'erreur");
                        sw.WriteLine("cls");
                        sw.WriteLine("echo	Veuillez S.V.P. fermer tous vos fichiers et dossiers sur le lecteur O: , afin que vous puissiez vous connecter à un autre serveur.");
                        sw.WriteLine("echo	-- --");
                        sw.WriteLine("echo	Please close all your documents and folders on the O: drive, so that you can connect to another server.");
                        sw.WriteLine("pause");
                        sw.WriteLine("goto end");
                        sw.WriteLine("");
                        sw.WriteLine(":Map");
                        sw.WriteLine("REM - Map le lecteur sur le serveur");
                        sw.WriteLine(String.Format("net use O: {0} /persistent:yes", serverAddress));
                        sw.WriteLine("if  errorlevel  1  goto  error");
                        sw.WriteLine("goto end");
                        sw.WriteLine(":End");
                        sw.WriteLine("");
                        sw.Close();

                    }

                    string contentType = "text/plain";
                    return File(sFilePath + "\\" + sFileName, contentType, "TTMapDrive.bat");

                }
                catch (Exception e)
                {
                    return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public ActionResult connectToDrive()
        {
            // Ouverture et execution du fichier
            Process proc = new Process();
            var process = Process.Start(sFilePath + "\\TTMapDrive.bat");
            process.WaitForExit();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

    }
}
