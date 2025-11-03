using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Exchange.WebServices.Data;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class emailCPService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();
   
        public string sendEmail(List<string> listCP )
        {
            List<string> listName = new List<string>();
            List<string> listNumFinal = new List<string>();
            getProjetAvecDateDepasseModel totalListModel = new getProjetAvecDateDepasseModel();
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "10.5.12.52";
                client.UseDefaultCredentials = false;
                NetworkCredential basicCredential = new NetworkCredential("BPR.SVC.MailMgmt", "The factor is there!");
                client.Credentials = basicCredential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (listCP == null )
                {

                    totalListModel = returnProjectDatePasseListe(null);

                }
                else
                {
                    var cpNum = listCP[0];
                    var cpNumEmp = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(x => x.FULL_NAME == cpNum).Select(x => x.EMPLOYEE_NUMBER).FirstOrDefault();
                    totalListModel = returnProjectDatePasseListe(cpNumEmp);
                    //listNumFinal.Add(listCP[0]);
                    
                }
               
                    foreach (CP item in totalListModel.listeCP)
                    { 
                        MailMessage NewEmail = new System.Net.Mail.MailMessage();
                        string emailMessage = "";
                        emailMessage += "<html><head>";
                        emailMessage += "</head><body>";
                        emailMessage += "</br>";
                        emailMessage += "<p> Merci de remplir les informations dans le tableau, a l'dresse suivante :</p> ";
                        emailMessage += "<div>Bonjour "+ item.CP_name +"</div>";
                        emailMessage += "<div>Nous vous invitons à prendre action dans les dix prochains jours ouvrables concernant les projets dont la date de fin est dépassée ou arriveront à échéance au cours des 30 prochains jours.</div>";
                        emailMessage += "<div>Veuillez consulter la liste des projets concernés en cliquant sur le lien ci - dessous : </div>";
                        emailMessage += "<a href='http://ttprojetplusdev.tetratech.com//emailCP/Index?empNum=" + item.CP_num + "'>Projets échus ou qui arrivent a échéance</a>";
                        emailMessage += "<div>Pour chacun de ces projets, merci de bien vouloir:</div>";
                        emailMessage += "<p>•	Indiquer la nouvelle date de fin prévue, si le projet se poursuit</p>";
                        emailMessage += "<p>Ou</p>";
                        emailMessage += "<p>•	Transmettre une directive de fermeture, si le projet est complété</p>";
                        emailMessage += "<div>Un champ commentaire est également disponible pour toute précision supplémentaire.</div>";
                        emailMessage += "<div>Un suivi sera effectué auprès des gestionnaires n’ayant pas répondu dans le délai indiqué.</div>";
                        emailMessage += "<div>Nous comptons sur votre collaboration et votre engagement afin de maintenir l’exactitude des informations relatives à l’état d’avancement de vos projets. Nous vous remercions d’avance pour votre diligence.</div>";
                        emailMessage += "</br>";
                        emailMessage += "<div>Cordialement,</div>";
                        emailMessage += "<div>Votre technicienne a la facturation</div>";//"+ item.+ "
                        emailMessage += "</body>";
                        ///////////////////////////////////////////////////////////////////////
                        //NewEmail.Attachments.AddFileAttachment(FileNameForEmp);
                        System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                        contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    //A: chargé de projet
                    //: agent de facturation
                    var emailCP = _entitiesSuiviMandat.EmployeeActif_Inactive.Where(x => x.EMPLOYEE_NUMBER == item.CP_num).Select(x => x.EMAIL_ADDRESS).FirstOrDefault();

                    //a decommenter
                   // NewEmail.To.Add(emailCP);//mail to sylvie.lambert@tetratech.com
                    NewEmail.To.Add("rapports@tetratech.com");//mail to sylvie.lambert@tetratech.com
                 //   NewEmail.CC.Add("rapports@tetratech.com");//mail CC to sylvie.lambert@tetratech.com
                    
                    NewEmail.From = new System.Net.Mail.MailAddress("rapport@tetratech.com");
                    NewEmail.Subject = "Projets - Date de fin est dépassée ou échéance dans les 30 prochains jours (" + item.CP_name + ") ";
                    NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);
                    NewEmail.IsBodyHtml = true;

                        // SmtpServer.Send(NewEmail);

                        //DECOMMENTER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        client.Send(NewEmail);
                        emailMessage = "";


                }
                //envoie du rapport:
                            MailMessage NewEmail2 = new System.Net.Mail.MailMessage();
                            string emailMessage2 = "";
                            emailMessage2 += "<html><head>";
                            emailMessage2 += "</head><body>";
                            emailMessage2 += "</br>";

                            emailMessage2 += "<p> voici la liste des chargés de projets qui ont recu un courriel de rappel :</p> ";
                            emailMessage2 += "</br>";
                            emailMessage2 += "<table style='border-style: solid;width: 100%;'>";
                            emailMessage2 += "<thead>";
                            emailMessage2 += "<tr style='font-family: arial; font-size:14px;background-color: #aab1b1;' >";
                            emailMessage2 += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;' > Numéro de CP </th>";
                            emailMessage2 += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;' > Nom cP </th>";
                            emailMessage2 += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;' > Liste des projets concernés</th>";
                            emailMessage2 += " </tr>";
                            emailMessage2 += "</thead>";
                            emailMessage2 += "<tbody>";
                        foreach (CP itemCP in totalListModel.listeCP)
                            {
                            emailMessage2 += "<tr style='font-family: arial; font-size:14px;;'>";
                            emailMessage2 += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + itemCP.CP_num + "</td>";
                            emailMessage2 += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + itemCP.CP_name + "</td>";
                    emailMessage2 += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>";
                            emailMessage2 += "<ul>";

                    var listPRojetForCP = totalListModel.listeProjects.Where(x => x.project_mgr_num == itemCP.CP_num).ToList();


                    foreach (usp_TT_getProjetAvecDateDepasse_Result project in listPRojetForCP)
                            {
                                    emailMessage2 += "<li>";
                                    emailMessage2 += project.C__Projet;
                                    emailMessage2 += "</li>";

                            }
                        emailMessage2 += "</ul>";
                        emailMessage2 += "</td>";

                    emailMessage2 += "</tr>";
                            }
                            emailMessage2 += "</tbody>";
                            emailMessage2 += "</table>";
                            emailMessage2 += "</html></body>";
                ///////////////////////////////////////////////////////////////////////
                //NewEmail.Attachments.AddFileAttachment(FileNameForEmp);
                System.Net.Mime.ContentType contentType2 = new System.Net.Mime.ContentType();
                        contentType2.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;



                        NewEmail2.To.Add("rapports@tetratech.com");//mail sylvie.lambert@tetratech.com

                        NewEmail2.From = new System.Net.Mail.MailAddress("rapport@tetratech.com");
                        NewEmail2.Subject = "Rapport d'envoie aux CP test";
                        NewEmail2.Body = new MessageBody(BodyType.HTML, emailMessage2);
                        NewEmail2.IsBodyHtml = true;

                        // SmtpServer.Send(NewEmail);

                        //DECOMMENTER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        client.Send(NewEmail2);
                        emailMessage2 = "";

                //ici on garde la trace des cp qui ont été relancé pour determiner l etat des projets considérés:
                save_EmailSent_ToCP(totalListModel);

                return "success";
            }
            catch (Exception e)
            {
                return "failed";
            }

        }


        public void save_EmailSent_ToCP(getProjetAvecDateDepasseModel totalListModel)
        {
          //DECOMMNETER AVANT MISE EN PROD
          //  string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
          //POUR DEV
            string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";


            var dateToday = DateTime.Now.ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                for (int i = 0; i < totalListModel.listeProjects.Count; i++)
                {
                    string sql = @"INSERT INTO [TT_Suivi_ProjetsEchus] 
                               ([cpNumber], [cpName],  [projectNum] , [DateDeRappel]) 
                               VALUES (@cpNumber, @cpName,@projectNum,  @dateToday)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cpNumber", totalListModel.listeProjects[i].project_mgr_num);
                        cmd.Parameters.AddWithValue("@cpName", totalListModel.listeProjects[i].project_mgr_name);
                        cmd.Parameters.AddWithValue("@projectNum", totalListModel.listeProjects[i].C__Projet);
                        cmd.Parameters.AddWithValue("@dateToday", dateToday);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    




        public getProjetAvecDateDepasseModel returnProjectDatePasseListe(string empNum = null)
        {
            getProjetAvecDateDepasseModel model = new getProjetAvecDateDepasseModel();
            var list = _entitiesSuiviMandat.usp_TT_getProjetAvecDateDepasse(empNum).ToList();

            var listCP = list.GroupBy(x => new { x.project_mgr_num, x.project_mgr_name })
                .Select(x => new CP
            {
                CP_num = x.Key.project_mgr_num,
                CP_name = x.Key.project_mgr_name
            })
            .ToList();

            model.listeProjects = list;
            model.listeCP = listCP;
            return model;
        }


        public string  sendCPResponse(List<LigneData> liste_ligneData, string empNum)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "10.5.12.52";
                client.UseDefaultCredentials = false;
                NetworkCredential basicCredential = new NetworkCredential("BPR.SVC.MailMgmt", "The factor is there!");
                client.Credentials = basicCredential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage NewEmail = new System.Net.Mail.MailMessage();
                string emailMessage = "";
                emailMessage += "<html><head>";
                emailMessage += "</head><body>";
                emailMessage += "</br>";

                emailMessage += "<p> Bonjour, </p> ";
                emailMessage += "<p> Voici les informations complémentaires pour les projets suivant qui sont échus ou en voie de l'etre:</p> ";
                emailMessage += "<br/>";
                emailMessage += "<table style='border-style: solid;width: 100%;'>";
                emailMessage += "<thead>";
                emailMessage += "<tr style='font-family: arial; font-size:14px;background-color: #aab1b1;' >";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;' > Numéro de projet </th>";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;' > Nom cP </th>";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;'># Projet</th>";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;'>Nouvelle date</th>";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;'>A fermer</th>";
                emailMessage += "<th style='width:8%;border: 1px solid black; border-collapse: collapse;text-align: left; padding: 4px;'>Commentaire</th>";
                emailMessage += " </tr>";
                emailMessage += "</thead>";
                emailMessage += "<tbody>";
                foreach(LigneData item in liste_ligneData)
                {
                    emailMessage += "<tr style='font-family: arial; font-size:14px;;'>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.cpName + "</td>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.cpNum + "</td>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.ProjetNum + "</td>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.NouvelleDate + "</td>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.AFermer + "</td>";
                    emailMessage += "<td  style='border: 1px solid black; border-collapse: collapse;vertical-align: top;text-align: left; padding: 4px;'>" + item.Commentaire + "</td>";
                    emailMessage += "</tr>";
               
                
                
                }
                emailMessage += "</tbody>";
                emailMessage += "</table>";

                ///////////////////////////////////////////////////////////////////////
                //NewEmail.Attachments.AddFileAttachment(FileNameForEmp);
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;



                NewEmail.To.Add("rapports@tetratech.com");//mail sylvie.lambert@tetratech.com

                NewEmail.CC.Add("rapports@tetratech.com");//mail


                NewEmail.From = new System.Net.Mail.MailAddress("rapport@tetratech.com");
                NewEmail.Subject = "réponse CP ("+ liste_ligneData[0].cpNum +")"  ;
                NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);
                NewEmail.IsBodyHtml = true;

                // SmtpServer.Send(NewEmail);

                //DECOMMENTER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                client.Send(NewEmail);
                emailMessage = "";


                //ic on va mnettre a jour la table [TT_Suivi_ProjetsEchus] pour ajouter la date de reponse

                //DECOMMNETER POUR MISE EN PROD
               // string connectionString = @"data source=TQCS349SQL1\BPRSQL;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";
              
                //DEV  
                string connectionString = @"data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework";



                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var dateToday = DateTime.Now.ToString("yyy-MM-dd");

                    foreach (LigneData item in liste_ligneData)
                    {
                      string sqlQuery = "UPDATE [TTProjetPlus].[dbo].[TT_Suivi_ProjetsEchus] ";
                    sqlQuery += " SET [DateDeReponseCP]= '" + dateToday + "' ";
                    sqlQuery += " WHERE [cpNumber] = '" + empNum + "' and [projectNum] = '" + item.ProjetNum + "' "; 


                        using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }


                    }

                }
                return "success";
            }
            catch (Exception e)
            {
                return "failed";
            }

        }


        public List<TT_Suivi_ProjetsEchus> getResponsesTable()
        {
            var list = _entities.TT_Suivi_ProjetsEchus.ToList();
            return list;
        }

    }
}