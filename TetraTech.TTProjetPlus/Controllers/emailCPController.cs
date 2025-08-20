using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class emailCPController : Controller
    {

        private readonly emailCPService _emailCPService = new emailCPService();
        private readonly TTProjetMenuService _TTMService = new TTProjetMenuService();
       


        // GET: emailCP
        public ActionResult Index(string empNum, string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            ViewBag.isAdminInfo = _TTMService.returnAdditionalInfoPermission(UserService.CurrentUser.EmployeeId);

            var result = _emailCPService.returnProjectDatePasseListe (empNum);
            
            return View(result);
        }

        public ActionResult sendEmail_ToCP(string empNum)
        {
            List<string> listCP = new List<string>();
            
            if(empNum == "")
            {
                
                _emailCPService.sendEmail(null);
            }
            else
            {
                 listCP.Add(empNum);
                _emailCPService.sendEmail(listCP);
            }
            

            //la liste des chargés de gestionnaires de projets concernés (numero, nom)
            var message = empNum == null ? "Courriel envoyé avec succès." : "Les courriels ont été envoyé avec succés.";
            return Json(message, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public ActionResult sendEmail_ByCP(string allData)
        {
            if (string.IsNullOrEmpty(allData))  return new HttpStatusCodeResult(400, "Aucune donnée reçue");
                

            // Séparer les lignes
            var lignes = allData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var empNum = "";
            List<LigneData> liste_ligneData = new List<LigneData>();  
            foreach (var ligne in lignes)
            {
                LigneData model = new LigneData();
                // Séparer les valeurs avec le séparateur "|"
                model.cpNum = ligne.Split('|')[0];
                if (empNum == "")
                {
                    empNum = model.cpNum;
                }
                model.cpName = ligne.Split('|')[1];
                model.ProjetNum = ligne.Split('|')[2];
                model.NouvelleDate = ligne.Split('|')[3];
                model.AFermer = ligne.Split('|')[4];
                model.Commentaire = ligne.Split('|')[5];

                liste_ligneData.Add(model);
                
            }
                _emailCPService.sendCPResponse(liste_ligneData, empNum);

            return Json(new { success = true });
        }



        public ActionResult responsesStatus()

        {

            // onnrecupere les lignes de la tables des reponses
            var responses = _emailCPService.getResponsesTable();

            // Return partial view with the list
            return PartialView("_ResponsesStatusPartial", responses);
        }
    }
}