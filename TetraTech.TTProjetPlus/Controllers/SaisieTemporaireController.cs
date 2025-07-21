using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class SaisieTemporaireController : Controller
    {
        private readonly STService _STService = new STService();

        // GET: SaisieTemporaire
        public ActionResult Index(string menuVisible = "true")
        {
            ViewBag.MenuVisible = menuVisible;
            ViewBag.GestionnairesNumberArray = GestionnairesArrayFromEntity();
            var listeType = _STService.GetReportType_ST().ToList();
            return View(listeType);
        }


        public string[] GestionnairesArrayFromEntity()
        {
            var rtnlist = new List<string>();
            rtnlist = _STService.GetGestionnairesNames715();

            return rtnlist.ToArray();
        }



        public ActionResult returnSendingTypeForEmp(string nom)
        {
            //on va resuperer les type d'envoie que l'employe designé a déja
            // de maniere a les cocher a l'avance dans le multiselect
            var sendTypesForEmp = _STService.returnSendingTypeForEmp(nom);
            string json = JsonConvert.SerializeObject(sendTypesForEmp);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnSendingType()
        {
            var sendTypes = _STService.GetReportType_ST2().ToList();
            string json = JsonConvert.SerializeObject(sendTypes);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult addTypesToEmp(string[] typesListe, string empNom)
        {
            // on verifie si l emp est deja dans la table des envoie:
            var isInSendTable = _STService.isInSendTable(empNom);
            if (isInSendTable == false)
            {
                //on va créer une nouvelle entrée dans la table:
                var inserted = _STService.insertIntoSendListTable(typesListe, empNom);
                return Json(inserted.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                //deletion de la personne (pour la remettre avec les nouvelles permissions
                var removed = _STService.removeSendListTableForEmp(empNom);
                if (typesListe[0] == "[]")
                {
                    return Json("true", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var inserted = _STService.insertIntoSendListTable(typesListe, empNom);
                    return Json(inserted.ToString(), JsonRequestBehavior.AllowGet);
                }

            }
        }


    }
}