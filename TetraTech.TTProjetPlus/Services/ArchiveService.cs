using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Services
{
    public class ArchiveService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();
        private readonly HomeService _HomeService = new HomeService();

        public List<string> ArchiveProject(List<string> listProjects, List<string> pathPartageList)
        {
            //String strPathServer = "";
            String strPathProject = "";
            String strPathArchiveProject = "";
            int projctHas10Confidentiel = 0;
            List<string> errorList = new List<string>();
            for(int i=0; i< listProjects.Count; i++)
            {
                    try
                    {

                    //1)
                    //Récupérer le path partage du serveur où se retrouve le projet
                      var server = returnPathPartage2(listProjects[i]);
                    //strPathServer = @"\\tts349test03\Prj_Reg\"; //pathPartage
                    strPathProject = server + "\\" + listProjects[i];
                    strPathArchiveProject = server + "\\" + @"_Ferme\" + listProjects[i];

                    //strPathProject = @"C:\test1";
                    //strPathArchiveProject = @"C:\test2\test.txt";
                    //marche bien avec ces parametres mais pas avec les parametres veritables a cause de problémes de permission


                    if (Directory.Exists(strPathProject))
                    {
                            if (Directory.Exists(strPathProject + @"\DOC-PROJ\10\10CONFIDENTIEL"))
                            {
                            projctHas10Confidentiel = 1;
                            }
                            //2)
                            //BPR Projet: Update eta_pro_id = 2
                            //TT Projet + +: Date archivage de la structure ?
                            //on essaye de deplacer le projet vers le folder _ferme
                            var moveProjectFolder = MoveProjectFolder(strPathProject, strPathArchiveProject, listProjects[i]);
                                    if (moveProjectFolder == "0")
                                    {
                                            //3)
                                            //mettre a jour le statut
                                            var updateProjectState = UpdateProjectState(listProjects[i]);
                                            if (updateProjectState == "0")
                                            {
                                            //4)
                                            //Refaire la sécurité            
                                            var secureArchiveFolder = SecureArchiveFolder(listProjects[i], projctHas10Confidentiel);
                                            if (secureArchiveFolder == "0")
                                                {
                                       errorList.Add( "Succés pour " + listProjects[i]);
                                                }
                                                else
                                                {
                                    //une erreur a été loguée a cause d un probleme dans SecureArchiveFolder
                                    errorList.Add("Échec pour " + listProjects[i] + "( voir error log dans BD )");
                                                }

                                            }
                                            else
                                            {
                                //une erreur a été loguée a cause d un probleme dans UpdateProjectState
                                errorList.Add("Échec pour " + listProjects[i] + "( voir error log dans BD )");

                                            }

                                    }

                                    else
                                    {
                                    //une erreur a été loguée a cause d un probleme dans MoveProjectFolder
                                    if (moveProjectFolder == "-1")
                                errorList.Add("Échec pour " + listProjects[i] + "( dossier non déplacé - voir error log dans BD )");
                                    else
                                errorList.Add("Échec pour " + listProjects[i] + "( Windows Explorer ou une autre application pointe vers la structure de répertoires demandée - voir error log dans BD )");
                                    }
                    }

                    else
                    {
                        //une erreur a été loguée a cause d un probleme dans MoveProjectFolder
                        errorList.Add("Échec pour " + listProjects[i] + "( Le projet demandé n'existe pas sur le serveur. Pour plus de détails, veuillez communiquer avec l'administrateur - voir error log dans BD )");
                       
                    }

                    }
                    catch (Exception e)
                    {
                    _HomeService.addErrorLog(e, "ArchiveService", "ArchiveProject", listProjects[i]);
                    errorList.Add("Échec pour " + listProjects[i] + " ( voir error log dans BD )");

                }

             }
            return errorList;
}

        public string ArchiveProject2(string strProjectNo, string pathPartage)
        {
            String strPathServer = "";
            String strPathProject = "";
            String strPathArchiveProject = "";
            int projctHas10Confidentiel = 0;

            try
            {
                //1)
                //Récupérer le path partage du serveur où se retrouve le projet
                var server = returnPathPartage2(strProjectNo);
                strPathServer = @"\\tts349test03\Prj_Reg\"; //pathPartage, cette variable n est pas utilisée
                strPathProject = server + "\\" + strProjectNo;
                strPathArchiveProject = server + "\\" + @"_Ferme\" + strProjectNo;

                if (Directory.Exists(strPathProject))
                {
                    if (Directory.Exists(strPathProject + @"\DOC-PROJ\10\10CONFIDENTIEL"))
                    {
                        projctHas10Confidentiel = 1;
                    }
                    //2)
                    //BPR Projet: Update eta_pro_id = 2
                    //TT Projet + +: Date archivage de la structure ?
                    //on essaye de deplacer le projet vers le folder _ferme
                    var moveProjectFolder = MoveProjectFolder(strPathProject, strPathArchiveProject, strProjectNo);
                    if (moveProjectFolder == "0")
                    {
                        //3)
                        //mettre a jour le statut
                        var updateProjectState = UpdateProjectState(strProjectNo);
                        if (updateProjectState == "0")
                        {
                            //4)
                            //Refaire la sécurité            
                            var secureArchiveFolder = SecureArchiveFolder(strProjectNo, projctHas10Confidentiel);
                            if (secureArchiveFolder == "0")
                            {
                                return "success";
                            }
                            else
                            {
                                //une erreur a été loguée a cause d un probleme dans SecureArchiveFolder
                                return "fail";
                            }
                        }
                        else
                        {
                            //une erreur a été loguée a cause d un probleme dans UpdateProjectState
                            return "fail";
                        }
                    }
                    else
                    {
                        //une erreur a été loguée a cause d un probleme dans MoveProjectFolder
                        if (moveProjectFolder == "-1")
                            return "failMoveFolder";
                        else
                            return "openedWindows";
                    }
                }
                else
                {
                    //une erreur a été loguée a cause d un probleme dans MoveProjectFolder
                    return "failExistOnServer";

                }
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "ArchiveService", "ArchiveProject", strProjectNo);
                return "fail";
            }
        }

        public string UpdateProjectState(string strProjectNo)
        {
           
            try
            {
                //ETA_PRO_Id	ETA_PRO_Nom	               ETA_PRO_Description
                //   2	          Archivé        	Projet non disponible pour modification
                var result = _entitiesBPR.usp_ArchiverProjet(strProjectNo);
                return "0";
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "ArchiveService", "UpdateProjectState", strProjectNo);
                return "-1";
            }

        }

        public string MoveProjectFolder(string strPathProject, string strPathArchiveProject, string strProjectNo)
        {
            try
            {

                //  System.IO.File.Copy(@"C:\TTProjetPlus\TQEDocs\test.txt", @"\\tts372fs1\prj_reg\06792\test.txt");
                //  Directory.Move(strPathProject, strPathArchiveProject);
                

                DirectoryInfo diIndiv = new DirectoryInfo(strPathProject);
              //  IsFileLocked(strPathProject);
                DirectoryInfo diArchive = new DirectoryInfo(strPathArchiveProject);
              //  string strPath = string.Concat(diArchive.FullName, "");
                diIndiv.MoveTo(diArchive.FullName);

               // System.IO.File.Copy(strPathProject, strPathArchiveProject);

                return "0";
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "ArchiveService", "MoveProjectFolder", strProjectNo);
                if (e.InnerException.ToString().Contains("Access to the path") && e.InnerException.ToString().Contains("is denied"))
                {
                    return "-2";
                }
                return "-1";

            }
        }


        //pour s assurer que le dossier n est pas utilisé
        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (Stream stream = new FileStream("1.docx", FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public string SecureArchiveFolder(string strProjectNo, int projctHas10Confidentiel)
        {
            var db = _entities;
            try
            {
                byte bytDoDelBatFile = 0;
                byte bytProjctHas10Confidentiel = 0;
                if (projctHas10Confidentiel == 1) { bytProjctHas10Confidentiel = 1; }

                var result = _entitiesBPR.usp_TT_Security_Archive_TTProjetPlus(strProjectNo, bytProjctHas10Confidentiel, bytDoDelBatFile);
                return "0";
            }
            catch (Exception e)
            {
                _HomeService.addErrorLog(e, "ArchiveService", "SecureArchiveFolder", strProjectNo);
                return "-1";

            }
        }

        public List<string> returnPathPartage (List<string> projectIdLList)
        { var pathPartageList = new List<string>();
            foreach (string item in projectIdLList)
            {
                    try
                    {
                        var serveurId = _entitiesBPR.Projets.Where(p => p.PRO_Id == item).Select(p => p.SER_Id).FirstOrDefault();
                        pathPartageList.Add(_entitiesBPR.usp_ObtenirServeur(serveurId).Select(p => p.SER_Chemin_Partage).FirstOrDefault());
                    }
                    catch (Exception e)
                    {
                        pathPartageList.Add("");
                    }

            }
            return pathPartageList;
        }

        public string returnPathPartage2(string projectId)
        {
            var serveurId = _entitiesBPR.Projets.Where(p => p.PRO_Id == projectId).Select(p => p.SER_Id).FirstOrDefault();
            var pathPartage = _entitiesBPR.usp_ObtenirServeur(serveurId).Select(p => p.SER_Chemin_Partage).FirstOrDefault();
            return pathPartage;
        }
    }
}