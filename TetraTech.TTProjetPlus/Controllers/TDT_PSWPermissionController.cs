using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class TDT_PSWPermissionController : Controller
    {

        private readonly TDT_PSWPermissionService _TDT_PSWPermissionService = new TDT_PSWPermissionService();
        private readonly HomeService _HomeService = new HomeService();

        // GET: TDT_PSWPermission
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            SLDocument sl = new SLDocument(@"\\tts440nf1\CAVolume2\Legacy\tts349fs2\SuiviMandat\QIB\Table de taux - Alteryx\test.xlsm", "Feuil1");
            var password = sl.GetCellValueAsString(13, 4); //moyen
            var titre = "Table de taux moyenne";

            //introduire la liste des chgts; cette liste n'est pertinente que pour les listes de taux moyen
            var listeChgts = _TDT_PSWPermissionService.returnListeChgts_new();
            //insertion des chgts dans la table des permissions:
            if(listeChgts != null && listeChgts.Count > 0)
            {
            foreach(string item in listeChgts)
            {
                
                var emp = _TDT_PSWPermissionService.getEmpData(item.Split('#')[0]);
                var info = _HomeService.returnInfo(emp.FULL_NAME);
                //insertIntoTable(string nomEmp, string numEmp, string email, string titreRH, string division, string comment, string type, string nom_app, string divMark, string autorisation)
                //on ne fait l'insertion des nouveaux KM que si il ne sont pas deja dans la table:
                if (item.Split('#')[4] == "ajouté(e)" && _TDT_PSWPermissionService.isInTableMoyenne(emp.EMPLOYEE_NUMBER) == false)
                {
                  //a decommenter:
                  _HomeService.insertIntoTable(emp.FULL_NAME, emp.EMPLOYEE_NUMBER, emp.EMAIL_ADDRESS, "", info.Split('#')[2], "", "moyenne", "Sylvie LAmbert","", "");

                    //a present on envoie un courriel a france lamothe pour demander l'attribution des permissions:
                //decommenter:
                  _TDT_PSWPermissionService.sendInsertionMail(emp.FULL_NAME, emp.EMPLOYEE_NUMBER, titre, password);

                }
                if(item.Split('#')[4] == "retiré(e)" && _TDT_PSWPermissionService.isInTableMoyenne(emp.EMPLOYEE_NUMBER) == true)
                {
                    deletEmpPerm(item.Split('#')[0], "moyenne");
                }

            }
            }

            //suite a la liste des chgts, il faut ou bien donner ou bien retirer des permissions.
            //ceci se fait en avisant France Lamothe par courriel
            ViewBag.GestionnairesNumberArray = GestionnairesArrayFromEntity();
            ViewBag.TitreHRArray = TitreHRArrayFromEntity();
            ViewBag.Type = "";

            return View(listeChgts);
        }


        public ActionResult displayInfo(string typeListe)
        {
            HttpContext context = System.Web.HttpContext.Current;
            var lang = context.Session["lang"].ToString();
            ViewBag.lang = lang;
            var listePermissions = _TDT_PSWPermissionService.returnListe(typeListe);
            return PartialView("_PartialTDTpswListAccess", listePermissions);
        }

        public ActionResult addTolist(string typeListe)
        {
            HttpContext context = System.Web.HttpContext.Current;
            var lang = context.Session["lang"].ToString();
            ViewBag.lang = lang;
            var listePermissions = _TDT_PSWPermissionService.returnListe(typeListe);
            return PartialView("_PartialTDTpswListAccess", listePermissions);
        }

        public ActionResult returnEmpInfo(string nomEmp)
        {
            var num = _HomeService.returnInfo(nomEmp);
            return Json(num, JsonRequestBehavior.AllowGet);
        }



        public string[] GestionnairesArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.GetGestionnairesNames715();

            return rtnlist.ToArray();
        }


        public string[] TitreHRArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _HomeService.returnTitreRH();
            return rtnlist.ToArray();
        }

        public string insertIntoTable(string nomEmp, string numEmp, string email, string titreRH, string division, string comment, string type, string nom_app, string divMark, string autorisation)
        {

            var success = _HomeService.insertIntoTable(nomEmp, numEmp, email, titreRH, division, comment, type, nom_app, divMark, autorisation);

            if (success == "success")

            {

                // les mots de passe 
                SLDocument sl = new SLDocument(@"\\tts440nf1\CAVolume2\Legacy\tts349fs2\SuiviMandat\QIB\Table de taux - Alteryx\test.xlsm", "Feuil1");
                var password = "";
                var titre = "";
                if (type == "moyenne")
                {
                    password = sl.GetCellValueAsString(13, 4); //moyen
                    titre = "Table de taux moyenne";

                }
                else if (type == "partielle")
                {
                    password = sl.GetCellValueAsString(12, 4); //partiel
                    titre = "Table de taux partielle";

                }
                else if (type == "complete")
                {
                    password = sl.GetCellValueAsString(11, 4); //complet
                    titre = "Table de taux compléte";

                }
                List<string> listeEnvoie = new List<string>();
                // _TDT_PSWPermissionService.sendPswEmail(type, email, titre, password);
                _TDT_PSWPermissionService.sendInsertionMail(nomEmp, numEmp, titre, password);

            }
            return success;


        }

        public ActionResult deletEmpPerm(string numEmp, string type)
        {
            var success = _HomeService.removeFromTable(numEmp, type);
            if (success == "success")

            {
                var nomEmp = _TDT_PSWPermissionService.getEmpName(numEmp);
                _TDT_PSWPermissionService.sendDeleteMail(nomEmp, numEmp, type);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}