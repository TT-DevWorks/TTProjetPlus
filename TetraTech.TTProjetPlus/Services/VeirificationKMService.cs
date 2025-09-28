using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Exchange.WebServices.Data;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class VeirificationKMService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();


        public List<Gestion10ConfEmployeeItem> GetListEmployeeForGestion10Conf(string selectedGroup)
        {
            try
            {
                var query = _entities.usp_getGestion10ConfADGroupMembers(Int32.Parse(selectedGroup));

                var aList = from p in query
                            select new Gestion10ConfEmployeeItem
                            {
                                Full_Name = p.displayName,
                                TTAccount = p.sAMAccountName,
                                discDocProjId = selectedGroup
                            };
             
                
                  foreach (Gestion10ConfEmployeeItem item in aList)
                {
                   item.Employee_Number = _entitiesSuiviMandat.EmployeeActif_Inactive
                                            .Where(p => p.FULL_NAME == item.Full_Name)
                                            .Select(p => p.EMPLOYEE_NUMBER)
                                            .FirstOrDefault();
                }
              
                return aList.ToList();

            }
            catch (Exception e)
            {
                return null;
            }
        }






        public List< TableVerificationKM> returnVerificationKMList()
        {
            var list = _entities.TableVerificationKMs.ToList();
            return list;
        }

        public List<usp_ReturnTableVerificationKM_ColumnsName_Result> returnVerificationKM_columnNames()
        {
            var list = _entities.usp_ReturnTableVerificationKM_ColumnsName().ToList();
            return list;
        }


        public List<KM_EmailModel> performValidation(List<List<string>> ListOf_liste_empOrgNot )
        {
            //Nicolas Paquet - 532334#Opération#715 BAT Ouest Efficacité énergétique

            //# projet = obj.project_number
            //Organisation du projet = acc_center
            //Nom de l’employé = liste[2].Split('#')[0]
            //Rôle du membre clé = roleEnglish
            //Titre = titre

            List<KM_EmailModel> listeAllCorrection = new List<KM_EmailModel>();

            foreach (List<string> liste  in ListOf_liste_empOrgNot)
            {
               
                for (int i = 0; i< liste.Count; i++)
                {
                    if (i>= 3)
                    {
                        List<usp_TT_Verif_KM_role_Result> item = new List<usp_TT_Verif_KM_role_Result>();
                        var role = liste[0].Split('#')[0];
                        var roleEnglish = "";
                        switch (role)
                        {
                            case "Administrateur de projet":
                                roleEnglish = "Project Administrator";
                                break;
                            case "Directeur d'opération": //a valider avec sylvie
                                roleEnglish = "Ops Mgr / Director";
                                break;
                        }
                        try
                        {
                            var acc_center = liste[i].Split('#')[2];
                            var empNum = Int32.Parse(liste[i].Split('#')[0].Split('-')[1].Trim());
                            var titre = liste[i].Split('#')[1];
                            item =    _entitiesSuiviMandat.usp_TT_Verif_KM_role(acc_center, roleEnglish, empNum).ToList();
                            foreach(usp_TT_Verif_KM_role_Result obj in item)
                            {

                                KM_EmailModel model = new KM_EmailModel();
                                model.projetNum = obj.project_number;
                                model.Organisation_projet = acc_center;
                                model.empName = liste[2].Split('#')[0];
                                model.role = roleEnglish;
                                model.titre = titre;
                                listeAllCorrection.Add(model);

        // listeAllCorrection.Add(liste[2].Split('#')[0] + "( titre: "+ titre + " ) n'est pas membre clé en tant que "+ role + " ( "+ roleEnglish + " ) dans le projet " + obj.project_number + " de l'ORG " + acc_center);

                            }
                        }
                        catch(Exception e)
                        {
                            continue;
                        }
                       
                    }

             

                }
            }

            return listeAllCorrection;
        }


        public bool sendEmail(List<KM_EmailModel> listeAllCorrection)
        {
            try
            {
                string folderPath = @"C:\TTRapport\FilesForTTProjetPlus\";
                string filePath = Path.Combine(folderPath, "rapportExcelKM.xlsx");

                // 1. Vider le dossier (supprimer tous les fichiers)
                if (Directory.Exists(folderPath))
                {
                    foreach (string file in Directory.GetFiles(folderPath))
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                }

                //on envoie un courriel avec un excel en attachement
                //creation de l excel:
                using (SLDocument sl = new SLDocument())
                {
                    // Define headers
                    sl.SetCellValue("A1", "# projet");
                    sl.SetCellValue("B1", "Organisation du projet");
                    sl.SetCellValue("C1", "Nom de l’employé");
                    sl.SetCellValue("D1", "Rôle du membre clé");
                    sl.SetCellValue("E1", "Titre");

                    // Header style: bold + yellow background
                    var headerStyle = sl.CreateStyle();
                    headerStyle.SetFontBold(true);
                    headerStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Yellow, System.Drawing.Color.Yellow);
                    sl.SetCellStyle("A1", "E1", headerStyle);

                    // Write data rows
                    int row = 2;
                    foreach (var item in listeAllCorrection)
                    {
                        sl.SetCellValue(row, 1, item.projetNum);
                        sl.SetCellValue(row, 2, item.Organisation_projet);
                        sl.SetCellValue(row, 3, item.empName);
                        sl.SetCellValue(row, 4, item.role);
                        sl.SetCellValue(row, 5, item.titre);
                        row++;
                    }

                    sl.Filter("A1", "E1");

                    // Optional: Set column widths for better display
                    sl.SetColumnWidth(1, 15);
                    sl.SetColumnWidth(2, 30);
                    sl.SetColumnWidth(3, 25);
                    sl.SetColumnWidth(4, 25);
                    sl.SetColumnWidth(5, 20);

                    // Save the file
                    sl.SaveAs(filePath);
                }

                //envoie du courriel
                SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
                MailMessage NewEmail = new MailMessage();
                string emailMessage = "";
                emailMessage += "<html><head>";
                emailMessage += "</head><body>";
                emailMessage += "</br>";
                emailMessage += "<p>Bonjour, </p>";
                emailMessage += "<p>Vous trouverez en fichier joint un fichier excel qui liste des projets pour lesquels le KM doit etre ajusté</p>";
                emailMessage += "</br>";
                emailMessage += "<div>Merci.</div>";

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                System.Net.Mail.Attachment data = new System.Net.Mail.Attachment(filePath, contentType);
                NewEmail.Attachments.Add(data);

                //prod:                                    
                NewEmail.From = new MailAddress("rapports@tetratech.com");
                NewEmail.To.Add("sylvie.lambert@tetratech.com");
                NewEmail.CC.Add("rapports@tetratech.com");

                NewEmail.Subject = "Rapport de vérification KM " ;
                NewEmail.IsBodyHtml = true;
                NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);

                ServicePointManager.ServerCertificateValidationCallback =
               delegate (
                   object s,
                   System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                   System.Security.Cryptography.X509Certificates.X509Chain chain,
                   System.Net.Security.SslPolicyErrors sslPolicyErrors
               )
               {
                   return true;
               };

                SmtpServer.Send(NewEmail);
                data.Dispose();
                emailMessage = "";

                return true;

            }

            catch (Exception e)
            {


                return false;
            }
        }
    }
}