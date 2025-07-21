using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Exchange.WebServices.Data;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace TetraTech.TTProjetPlus.Services
{
    public class TDT_PSWPermissionService
    {
        private readonly SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();


        public List<pswPermissionModel> returnListe(string listType)
        {
            List<pswPermissionModel> listePermission = new List<pswPermissionModel>();
            switch (listType)
            {
                case "partielle":
                    var listePartielle = _entitiesSuiviMandat.TDT_Liste_de_taux_partielle.ToList();
                    foreach (TDT_Liste_de_taux_partielle item in listePartielle)
                    {
                        pswPermissionModel model = new pswPermissionModel();
                        model.Commentaire = item.Commentaire;
                        model.Division = item.Division;
                        model.email = item.Adresse_courriel;
                        model.Fait = item.Fait;
                        model.Nom_employee = item.Nom_de_l_employé;
                        model.Num_employee = item.Numéro_employé_;
                        model.Titre = item.Titre;
                        model.listeType = listType;
                        listePermission.Add(model);
                    }
                    break;
                case "moyenne":
                    var listeMoyenne = _entitiesSuiviMandat.usp_getProject_ProgramManager("both").ToList();// _entitiesSuiviMandat.TDT_Liste_de_taux_moyen.ToList();
                    foreach (usp_getProject_ProgramManager_Result item in listeMoyenne)
                    {
                        try
                        {
                        pswPermissionModel model = new pswPermissionModel();
                        model.Commentaire = "";// item.Commentaire;
                        model.Division = item.ORG.Split(' ')[1];
                        model.email = item.Courriel;
                        model.Fait = "";// item.Fait;
                        model.Nom_employee = item.Employé_e_;
                        model.Num_employee = item.EMPLOYEE_NUMBER;
                        model.Titre = item.role;
                        model.Retrait = "";// item.Retrait;
                        model.listeType = listType;

                        listePermission.Add(model);

                        }
                        catch(Exception e)
                        {
                            continue;
                        }
                    }

                    break;
                case "complete":
                    var listeComplete = _entitiesSuiviMandat.TDT_Liste_de_taux_complet.ToList();
                    foreach (TDT_Liste_de_taux_complet item in listeComplete)
                    {
                        pswPermissionModel model = new pswPermissionModel();
                        model.Commentaire = item.Commentaire;
                        model.Division = item.Division;
                        model.email = item.Adresse_courriel;
                        model.Fait = item.Fait;
                        model.Nom_employee = item.Nom_de_l_employé;
                        model.Num_employee = item.Numéro_employé_;
                        model.Titre = item.Titre;
                        model.listeType = listType;

                        listePermission.Add(model);
                    }

                    break;

                case "approbateurs":
                    var listeApprobateurs = _entitiesSuiviMandat.TDT_Approbateurs.ToList();
                    foreach (TDT_Approbateurs item in listeApprobateurs)
                    {
                        pswPermissionModel model = new pswPermissionModel();
                        model.Nom_approbateur = item.Nom;
                        model.Nom_Division_market = item.Division_de_marché;
                        model.autorisation = item.autorisation;
                        model.listeType = listType;

                        listePermission.Add(model);
                    }

                    break;

            }


            return listePermission;
        }


        public List<string> returnListe_lastWeek() // retourne la liste des key members qui ont été retirés dans la present pwd 
        {
            List<string> listeKM_removed = new List<string>();
            var precedentPWD = ((DateTime)(_entitiesSuiviMandat.usp_TDT_getPrecedentPWD().Select(x => x.predecentPDWEND).First())).ToString("yyyy-MM-dd");
            var listLastWeek = _entitiesSuiviMandat.usp_TDT__getProject_ProgramManager_Precedent_vs_Current_PeriodWeekDate().ToList();
            if (listLastWeek.Count >0)
            {
            foreach (usp_TDT__getProject_ProgramManager_Precedent_vs_Current_PeriodWeekDate_Result item in listLastWeek)
            {
                    try
                    {
                        var model = "";
                        model+= Int32.Parse(item.EMPLOYEE_NUMBER)+ "#";
                        model+= item.Courriel + "#"; 
                        model+= item.role + "#"; ;
                        model += precedentPWD;

                        listeKM_removed.Add(model);

                    }
                    catch(Exception e)
                    {
                        continue;
                    }
            }

            return listeKM_removed;

            }
            else
            {
                return null;
            }
        }


        public List<string> returnListe_CurrentWeek() // retourne la liste des key members qui se sont ajouté dans la presente pwd
        {
            List<string> listeKM_added = new List<string>();
            var actualPWD = ((DateTime)(_entitiesSuiviMandat.usp_TDT_getPrecedentPWD().Select(x => x.actualPWD).First())).ToString("yyyy-MM-dd");
            var listcurrentWeek = _entitiesSuiviMandat.usp_TDT_getProject_ProgramManager_Current_vs_Precedent_PeriodWeekDate().ToList();
            if (listcurrentWeek.Count > 0)
            {
            foreach (usp_TDT_getProject_ProgramManager_Current_vs_Precedent_PeriodWeekDate_Result item in listcurrentWeek)
            {
                    try
                    {
                    var model = "";
                    model += Int32.Parse(item.EMPLOYEE_NUMBER) + "#";
                    model += item.Courriel + "#";
                    model += item.role + "#"; ;
                    model += actualPWD;

                    listeKM_added.Add(model);

                    }
                    catch(Exception e)
                    {
                        continue;
                    }
            }

            return listeKM_added;

            }
            else
            {
                return null;
            }
        }


        //public List<List<pswPermissionModel>> retunListDifferences()
        //{
        //    var lastPWD = returnListe_lastWeek();
        //    var currentPWD = returnListe("both");
        //    var infirstNotInSecond = list1.Except(list2).ToList();
        //    var secondNotFirst = list2.Except(list1).ToList();
        //} 

        public string getEmpName(string numEmp)
        {
            return _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.EMPLOYEE_NUMBER == numEmp).Select(p => p.FULL_NAME).FirstOrDefault();
        }

        public EmployeeActif_Inactive getEmpData(string numEmp)
        {
            var item =  _entitiesSuiviMandat.EmployeeActif_Inactive.Where(p => p.EMPLOYEE_NUMBER == numEmp).FirstOrDefault();
            return item;
        }


        public List<TDT_Approbateurs> returnListeResponsables()
        {
            return _entitiesSuiviMandat.TDT_Approbateurs.ToList();
        }

        //public List<TDT_Changements> returnListeChgts()
        //{
        //    // _entitiesSuiviMandat.usp_get;

        //    //changer pour mise en prod
        //     string connectionString = @"Data Source=TQCS349sql1\bprsql;Initial Catalog=SuiviMandat;persist security info=True;user id=TLX_USER;password=tlxuser!@#prd";

        //  //  string connectionString = @"Data Source=TTS349TEST02;Initial Catalog=SuiviMandat;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_getPM_Changes", connection);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@forceRefresh ", 1);

        //        connection.Open();
        //        int rowAffected = cmd.ExecuteNonQuery();
        //        connection.Close();

        //    }

        //    return _entitiesSuiviMandat.TDT_Changements.ToList();
        //}


        public List<string>  returnListeChgts_new()
        {
            List<string> allListe = new List<string>();
            var liste_removed = returnListe_lastWeek(); // removed
            var liste_added = returnListe_CurrentWeek(); //added
            if (liste_removed != null && liste_removed.Count > 0 )
            {
                foreach (string item in liste_removed)
                {
                    allListe.Add(item + "#" + "retiré(e)");
                }

            }
            if (liste_added != null && liste_added.Count > 0)
            {
                foreach (string item in liste_added)
               
                {
                    allListe.Add(item + "#" + "ajouté(e)");
                }

            }

            if (allListe.Count > 0)
            {
                return allListe;
            }
            else
            {
                return null;
            }
        }

        public bool isInTableMoyenne(string empNum)
            
        {
            var emp = _entitiesSuiviMandat.TDT_Liste_de_taux_moyen.Where(x => x.Numéro_employé_ == empNum).ToList();
       if(emp!= null && emp.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string sendInsertionMail(string nomEmp, string numEmp, string type, string psw)
        {
            String emailMessage = "";

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;



            //EmailMessage NewEmail = new EmailMessage(serviceInstance);
            MailMessage NewEmail = new MailMessage();

            emailMessage += "<html><head>";
            emailMessage += "</head><body>";
            emailMessage += "</br>";
            emailMessage += "<div style='width: 100 %; font-family: Arial; font-size: 14.00pt; '>";
            emailMessage += "<div>Bonjour,</div>";							
            emailMessage += "<div>Merci de  donner l’accès à la " + type + " à " + nomEmp + "(" + numEmp + ")" + "</div>";
            emailMessage += "<div>Le mot de passe est : "+ psw +"</div>";
            emailMessage += "<div></div>";
            emailMessage += "<div>Sylvie Lambert</div>";
            emailMessage += "</br>";

            //NF
            emailMessage += "<div>";
            emailMessage += "</div></br>";


            ///////////////////////////////////////////////////////////////////////

            //NewEmail.Attachments.AddFileAttachment(filename);
            // Create  the file attachment
            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;

            NewEmail.From = new MailAddress("rapports@tetratech.com");

            NewEmail.To.Add("France.Lamothe@tetratech.com");//srarequest@tetratech.com
            NewEmail.CC.Add("sylvie.lambert@tetratech.com");
            NewEmail.CC.Add("rapports@tetratech.com");
            //  NewEmail.CC.Add("sylvie.lambert@tetratech.com");


            NewEmail.Subject = "Requéte: accord de permissions pour " + nomEmp;
            NewEmail.IsBodyHtml = true;
            NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);

            //NewEmail.SendAndSaveCopy();
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
            //decommenter
            SmtpServer.Send(NewEmail);
            emailMessage = "";

            return "";
        }

        public string sendDeleteMail(string nomEmp, string numEmp, string type)
        {
            String emailMessage = "";

            SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
            // SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;



            //EmailMessage NewEmail = new EmailMessage(serviceInstance);
            MailMessage NewEmail = new MailMessage();

            emailMessage += "<html><head>";
            emailMessage += "</head><body>";
            emailMessage += "</br>";
            emailMessage += @"
                            <div style='width: 100 %; font-family: Arial; font-size: 14.00pt; '>
                            <div>Bonjour,</div>							
                            <div>Merci de retirer l’accès à la table de taux " + type + " à " + nomEmp + " ("+ numEmp + ")";
            emailMessage += "</br>";
            emailMessage += "<div>Sylvie LAmbert</div>";

            //NF
            emailMessage += "<div>";
            emailMessage += "</div></br>";


            ///////////////////////////////////////////////////////////////////////

            //NewEmail.Attachments.AddFileAttachment(filename);
            // Create  the file attachment
            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;

            NewEmail.From = new MailAddress("rapports@tetratech.com");

            NewEmail.To.Add("France.Lamothe@tetratech.com");//srarequest@tetratech.com
            NewEmail.CC.Add("sylvie.lambert@tetratech.com");
            NewEmail.CC.Add("rapports@tetratech.com");


            NewEmail.Subject = "Requéte: retrait de permissions pour " + nomEmp;
            NewEmail.IsBodyHtml = true;
            NewEmail.Body = new MessageBody(BodyType.HTML, emailMessage);

            //NewEmail.SendAndSaveCopy();
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
//decommenter
          SmtpServer.Send(NewEmail);
            emailMessage = "";

            return "";

        }

        public string sendPswEmail(string type, string email, string titre, string password)
        {

            //il faut recuperer la liste des personnes (email) qui doivent recevoir chaque type de password:
            // Bonjour,
            //À partir de demain, le nouveau mot de passe sera Crayon8364.
            //Bonne journée
            string htmlString = "";
            htmlString += "<div>Bonjour,</div>";
            htmlString += "<div>Le mot de passe pour la  '" + titre + "' , est présentement: " + password + "</div>";
            htmlString += "<div>Bonne journée</div>";
            htmlString += "<br>";
            htmlString += "<div> Sylvie Lambert</div>";

            try
            {

                SmtpClient SmtpServer = new SmtpClient("smtp.tetratech.com", 25);
                // SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                ///////////////////////////////

                MailMessage NewEmail = new MailMessage();

                if (type == "test" || type == "")
                {
                    NewEmail.To.Add("rapports@tetratech.com");
                }
                else
                {
                    NewEmail.Bcc.Add(email);
                    NewEmail.To.Add("rapports@tetratech.com");
                    NewEmail.To.Add("sylvie.lambert@tetratech.com");
                }

                NewEmail.From = new MailAddress("rapports@tetratech.com");

                NewEmail.Subject = titre;
                NewEmail.IsBodyHtml = true;
                NewEmail.Body = new MessageBody(BodyType.HTML, htmlString);

                ServicePointManager.ServerCertificateValidationCallback =
                                    delegate (
                                        object s,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors
                                    ) {
                                        return true;
                                    };

                SmtpServer.Send(NewEmail);
                NewEmail.Dispose();
                SmtpServer.Dispose();
                //File.AppendAllText(@"C:\TTRapport\TableDeTaux\TdTLog.txt",DateTime.Now + @" - Courriel '"+ titre +"' : succés" + Environment.NewLine);

                return "success";
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(@"C:\TTRapport\TableDeTaux\log\TdTlog.txt", DateTime.Now + "=> service TdT , " + " : " + e.Message + " " + e.InnerException.Message + Environment.NewLine);

                return "failed";
            }


        }

        public List<TDT_TablePSW> getPSWList()
        {
            var listDates =  _entitiesSuiviMandat.TDT_TablePSW.OrderBy(x=>x.id).ToList();
            //foreach (TDT_TablePSW item in listDates)
            //{
            //    DateTime date = DateTime.ParseExact(item.dateChgt, "MMM dd yyyy h:mmtt", CultureInfo.InvariantCulture);

            //    // Format the DateTime into the desired format
            //    string formattedDate = date.ToString("yyyy-MM-dd");
            //    item.dateChgt = formattedDate;
            //}
            return listDates;
        }

        public string insertIntoTablePSW(string moyen, string partiel, string complet, string dateChgt)
        {
            try
            {
                TDT_TablePSW model = new TDT_TablePSW();
                model.Moyen = moyen;
                model.Partiel = partiel;
                model.Complet = complet;
                model.dateChgt = dateChgt;
                _entitiesSuiviMandat.TDT_TablePSW.Add(model);
                _entitiesSuiviMandat.SaveChanges();

                return "success";

            }

            catch (Exception e)
            {
                return "failed";
            }
        }



    }
}