using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;
using System.Data;
using System.Data.SqlClient;

namespace TetraTech.TTProjetPlus.Controllers
{
    public class BoomerangController : Controller
    {
        private readonly InfoProjetService _InfoProjetService = new InfoProjetService();
        private readonly BoomerangService _BoomerangService = new BoomerangService();
        public BoomerangSearchModel currentCreateModel = new BoomerangSearchModel();

        public object PhotoEditor { get; private set; }

        // GET: Boomerang

        public ActionResult Home(string menuVisible = "true")

        {
            //pour line: $id_panier = (@$_SESSION['current_panier']['id_panier'] > 0) ? $_SESSION['current_panier']['id_panier'] : 0;
            Session["img"] = "";
            Session["current_panier"] = 0;
            Session["ADMIN"] = "yes";
            //a completer: qui est admin??

            //on recupere le nom de l utilisateur:
            var currentUser = UserService.CurrentUser;
            var nom = currentUser.FullName.Split(' ')[1];
            var prenom = currentUser.FullName.Split(' ')[0];
            decimal id_utilisateur = _BoomerangService.returnIdUtilisateur(prenom + "." + nom);
            Session["id_utilisateur"] = id_utilisateur;
            ViewBag.Date1 = Session["lang"].ToString() == "fr" ? "dddd dd MMMM yyyy" : "dddd MMMM, dd, yyyy";
            ViewBag.Date2 = Session["lang"].ToString() == "fr" ? "fr-FR" : "en-US";
            ViewBag.MenuVisible = menuVisible;
            BoomerangHomeModel model = new BoomerangHomeModel();
            model.listePanier = _BoomerangService.returnListePanier(nom, prenom);
            model.listeFichesDrafts = _BoomerangService.returnListeDrafts(2882);//pour test seuleemnt (int)id_utilisateur
                                                                                //   model.listeFichesDraftsInValidation = _BoomerangService.returnListeDraftsInValidation(2633);//pour test seuleemnt (int)id_utilisateur
            model.listeResp = _BoomerangService.returnUtilisateurListe();//_BoomerangService.returnRespList();
            return View(model);
        }


        public ActionResult CreateFactSheet(string menuVisible = "true")

        {
            //pour line: $id_panier = (@$_SESSION['current_panier']['id_panier'] > 0) ? $_SESSION['current_panier']['id_panier'] : 0;

            Session["currentCreateModel"] = new BoomerangSearchModel();
            Session["current_panier"] = 0;
            Session["ADMIN"] = "yes";
            //a completer: qui est admin??

            //if (code_affaire == 0)
            //    { }
            //    else
            //    {
            //        model.ficheDetails = _BoomerangService.getFicheLang("", "", ficheM_id.ToString(), "");
            //    }

            //pour test seulement:
            //ficheM = _BoomerangService.returnFiche();

            //on recupere le nom de l utilisateur:
            var currentUser = UserService.CurrentUser;
            var nom = currentUser.FullName.Split(' ')[1];
            var prenom = currentUser.FullName.Split(' ')[0];
            FicheModel model = new FicheModel();
            model.ficheDetails = new Boomerang_tempFicheLang();
            
            //if (code_affaire == "0")
            //{ 
                model.ficheDetails.id_fiche = 0;
                model.ficheDetails.duree = 0;
                model.ficheDetails.unite_1 = 0;
                model.ficheDetails.unite_2 = 0;
                model.ficheDetails.montant_operation = 0;
                model.ficheDetails.montant_equipe = 0;
                model.ficheDetails.montant_societe = 0;
                model.ficheDetails.montant_estimation = 0;
                model.ficheDetails.montant_soumission = 0;
                model.ficheDetails.montant_final = 0;
                model.ficheDetails.montant_changement_bpr = 0;
                model.ficheDetails.montant_changement_client = 0;
                model.ficheDetails.structure_prevu = 0;
                model.ficheDetails.structure_final = 0;
                model.ficheDetails.amenagement_prevu = 0;
                model.ficheDetails.amenagement_final = 0;
                model.ficheDetails.mecanique_prevu = 0;
                model.ficheDetails.mecanique_final = 0;
                model.ficheDetails.electricite_prevu = 0;
                model.ficheDetails.electricite_final = 0;
                model.ficheDetails.infrastructure_prevu = 0;
                model.ficheDetails.infrastructure_final = 0;
                model.ficheDetails.gestion_prevu = 0;
                model.ficheDetails.gestion_final = 0;
                model.ficheDetails.autre_prevu = 0;
                model.ficheDetails.autre_final = 0;
                model.ficheDetails.structure_prevu_bpr = 0;
                model.ficheDetails.structure_final_bpr = 0;
                model.ficheDetails.amenagement_prevu_bpr = 0;
                model.ficheDetails.amenagement_final_bpr = 0;
                model.ficheDetails.mecanique_prevu_bpr = 0;
                model.ficheDetails.mecanique_final_bpr = 0;
                model.ficheDetails.electricite_prevu_bpr = 0;
                model.ficheDetails.electricite_final_bpr = 0;
                model.ficheDetails.infrastructure_prevu_bpr = 0;
                model.ficheDetails.infrastructure_final_bpr = 0;
                model.ficheDetails.gestion_prevu_bpr = 0;
                model.ficheDetails.gestion_final_bpr = 0;
                model.ficheDetails.eco_energie_prevu = 0;
                model.ficheDetails.eco_energie_reel = 0;
                model.ficheDetails.eco_monetaire_prevu = 0;
                model.ficheDetails.eco_monetaire_reel = 0;
                model.ficheDetails.pri_prevu = 0;
                model.ficheDetails.pri_reel = 0;
                model.ficheDetails.conso_prevu = 0;
                model.ficheDetails.conso_reel = 0;
                model.ficheDetails.appui_financier_prevu = 0;
                model.ficheDetails.appui_financier_prevu2 = 0;
                model.ficheDetails.appui_financier_prevu3 = 0;
                model.ficheDetails.appui_financier_reel = 0;
                model.ficheDetails.appui_financier_reel2 = 0;
                model.ficheDetails.appui_financier_reel3 = 0;
                model.ficheDetails.montant_equipe_fin = 0;
                model.ficheDetails.montant_societe_fin = 0;
                model.ficheDetails.id_pole = 0;
                model.ficheDetails.id_createur = 0;
                model.ficheDetails.id_activite = 0;
                model.ficheDetails.id_type_unite_1 = 0;
                model.ficheDetails.id_type_unite_2 = 0;
                model.ficheDetails.id_agence = 0;
                model.ficheDetails.conception_prevue_mois_debut = 0;
                model.ficheDetails.conception_prevue_annee_debut = 0;
                model.ficheDetails.conception_prevue_mois_fin = 0;
                model.ficheDetails.conception_prevue_annee_fin = 0;
                model.ficheDetails.conception_finale_mois_debut = 0;
                model.ficheDetails.conception_finale_annee_debut = 0;
                model.ficheDetails.conception_finale_mois_fin = 0;
                model.ficheDetails.conception_finale_annee_fin = 0;
                model.ficheDetails.plan_prevu_mois_debut = 0;
                model.ficheDetails.plan_prevu_annee_debut = 0;
                model.ficheDetails.plan_prevu_mois_fin = 0;
                model.ficheDetails.plan_prevu_annee_fin = 0;
                model.ficheDetails.plan_final_mois_debut = 0;
                model.ficheDetails.plan_final_annee_debut = 0;
                model.ficheDetails.plan_final_mois_fin = 0;
                model.ficheDetails.plan_final_annee_fin = 0;
                model.ficheDetails.construction_prevue_mois_debut = 0;
                model.ficheDetails.construction_prevue_annee_debut = 0;
                model.ficheDetails.construction_prevue_mois_fin = 0;
                model.ficheDetails.construction_prevue_annee_fin = 0;
                model.ficheDetails.construction_finale_mois_debut = 0;
                model.ficheDetails.construction_finale_annee_debut = 0;
                model.ficheDetails.construction_finale_mois_fin = 0;
                model.ficheDetails.construction_finale_annee_fin = 0;
                model.ficheDetails.changement_bpr = 0;
                model.ficheDetails.changement_client = 0;
                model.ficheDetails.lastmod_time = null;
                model.ficheDetails.created_time = null;
                model.ficheDetails.code_affaire = "";
                model.ficheDetails.localisation = "";
                model.ficheDetails.id_province = "";
                model.ficheDetails.autre_lieu = "";
                model.ficheDetails.id_pays = "";
                model.ficheDetails.intitule = "";
                model.ficheDetails.intitule_court = "";
                model.ficheDetails.description = "";
                model.ficheDetails.id_type_mission = "";
                model.ficheDetails.mission = "";
                model.ficheDetails.particularite = "";
                model.ficheDetails.mois_debut = "";
                model.ficheDetails.annee_debut = "";
                model.ficheDetails.mois_fin = "";
                model.ficheDetails.annee_fin = "";
                model.ficheDetails.type_client = "";
                model.ficheDetails.client_nom = "";
                model.ficheDetails.client_adresse = "";
                model.ficheDetails.client_telephone = "";
                model.ficheDetails.client_mail = "";
                model.ficheDetails.contact_nom = "";
                model.ficheDetails.num_ref = "";
                model.ficheDetails.architecte = "";
                model.ficheDetails.conducteur_operation = "";
                model.ficheDetails.equipe = "";
                model.ficheDetails.equipe_interne = "";
                model.ficheDetails.id_charge_affaire = "";
                model.ficheDetails.proprio = "";
                model.ficheDetails.argumentaire_com = "";
                model.ficheDetails.code_qualification_opqibi = "";
                model.ficheDetails.lastmod_user = "";
                model.ficheDetails.lastmod_opqibi = "";
                model.ficheDetails.award = "";
                model.ficheDetails.nom_award = "";
                model.ficheDetails.lastmod_award = "";
                model.ficheDetails.imgForFiche = "";
                model.ficheDetails.etat = "";
                model.ficheDetails.active = "";
                model.ficheDetails.ancien_ref = "";
                model.ficheDetails.filiale = "";
                model.ficheDetails.ecart_periode_real = "";
                model.ficheDetails.ecart_cout = "";
                model.ficheDetails.nature_changement_bpr = "";
                model.ficheDetails.nature_changement_client = "";
                model.ficheDetails.ecart_structure = "";
                model.ficheDetails.ecart_amenagement = "";
                model.ficheDetails.ecart_mecanique = "";
                model.ficheDetails.ecart_electricite = "";
                model.ficheDetails.ecart_infrastructure = "";
                model.ficheDetails.ecart_gestion = "";
                model.ficheDetails.autre_nom = "";
                model.ficheDetails.ecart_autre = "";
                model.ficheDetails.ecart_structure_bpr = "";
                model.ficheDetails.ecart_amenagement_bpr = "";
                model.ficheDetails.ecart_mecanique_bpr = "";
                model.ficheDetails.ecart_electricite_bpr = "";
                model.ficheDetails.ecart_infrastructure_bpr = "";
                model.ficheDetails.ecart_gestion_bpr = "";
                model.ficheDetails.autre_nom_bpr = "";
                model.ficheDetails.autre_prevu_bpr = "0";
                model.ficheDetails.autre_final_bpr = "0";
                model.ficheDetails.ecart_autre_bpr = "0";
                model.ficheDetails.mesurage_eco = "";
                model.ficheDetails.appui_financier_organisme = "";
                model.ficheDetails.appui_financier_organisme2 = "";
                model.ficheDetails.appui_financier_organisme3 = "";
                model.ficheDetails.respect_echeancier_budget = "";
                model.ficheDetails.respect_echeancier_budget_note = "";
                model.ficheDetails.numero_structure = "";
                model.ficheDetails.mois_debut_fin = "";
                model.ficheDetails.annee_debut_fin = "";
                model.ficheDetails.mois_fin_fin = "";
                model.ficheDetails.annee_fin_fin = "";
                model.ficheDetails.ecart_montant_equipe = "";
                model.ficheDetails.ecart_montant_societe = "";
                model.ficheDetails.annee_sort = "";
                model.ficheDetails.TLinxProjectNumber = "";
                model.ficheDetails.ProjectURLSharepoint = "";
                model.ficheDetails.libelle_createur = "";
                model.ficheDetails.libelle_charge_affaire = "";
                model.ficheDetails.libelle_activite = "";
                model.ficheDetails.libelle_type_unite_1 = "";
                model.ficheDetails.libelle_type_unite_2 = "";
                model.ficheDetails.libelle_unite_1 = "";
                model.ficheDetails.libelle_unite_2 = "";
                model.ficheDetails.libelle_agence = "";
                model.ficheDetails.libelle_pays = "";
                model.ficheDetails.libelle_province = "";
                model.ficheDetails.libelle_pole = "";
                model.ficheDetails.couleur = "";
            //}
            //else
            //{
            //    var id_fiche = _BoomerangService.returnIdFiche(code_affaire);
            //    model.ficheDetails = _BoomerangService.getFicheLang("", "", id_fiche.ToString(), "");
            //}


            BoomerangSearchModel modelB = new BoomerangSearchModel();
            modelB.fiche = new FicheModel();
            modelB.fiche.ficheDetails = model.ficheDetails;

            var absolutePath2 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\images\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".png");

            if (System.IO.File.Exists(absolutePath2))
            {
                modelB.fiche.image = @"/Content/images/TTProjetPlus_Boomerang_images/files/images/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".png";
            }
            else modelB.fiche.image = "";


            var absolutePath3 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\awards\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf");
            if (System.IO.File.Exists(absolutePath3))
            {
                modelB.fiche.awardPDF = @"/Content/images/TTProjetPlus_Boomerang_images/files/awards/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf";
            }


            var absolutePath4 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\OPQIBI\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf");
            if (System.IO.File.Exists(absolutePath3))
            {
                modelB.fiche.OPQIBI = @"/Content/images/TTProjetPlus_Boomerang_images/files/OPQIBI/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf";
            }


            modelB.listeExpertise = _BoomerangService.returnSearchCriterias();
            modelB.listePole = modelB.listeExpertise.Select(p => p.libelle_pole).Distinct().ToList();
            modelB.listeProvinces = _BoomerangService.returnProvinces();
            modelB.listePays = _BoomerangService.returnPays(Session["lang"].ToString().ToLower());
            modelB.listeTypeMission = _BoomerangService.returnTypeMission(Session["lang"].ToString().ToLower());
            modelB.listeTypeUnits = _BoomerangService.returnTypeUnits();
            modelB.listeAgences = _BoomerangService.returnAgences();
            ViewBag.MenuVisible = menuVisible;
            ViewBag.Lang = Session["lang"].ToString().ToLower();
            modelB.listItems = new List<ComplexItem>();


            foreach (string pole in modelB.listePole)
            {
                ComplexItem ci1 = new ComplexItem();
                ci1.id = modelB.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).id_pole;
                if (ci1.id == 18)
                {
                    continue;
                }
                ci1.libelle = "*" + pole;
                ci1.color = "#" + modelB.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).couleur;
                modelB.listItems.Add(ci1);
                foreach (usp_Boomerang_getExpertises_Result item in modelB.listeExpertise)
                {


                    if (item.libelle_pole == pole)
                    {
                        ComplexItem ci = new ComplexItem();
                        ci.id = item.id_activite;
                        ci.libelle = item.libelle_activite;
                        ci.color = "#" + item.couleur;
                        modelB.listItems.Add(ci);
                    }
                }
            }


            //pour les dates:
            modelB.fiche.periode_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.mois_debut, modelB.fiche.ficheDetails.annee_debut);
            modelB.fiche.periode_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.mois_fin, modelB.fiche.ficheDetails.annee_fin); ;

            modelB.fiche.periode_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.mois_debut_fin, modelB.fiche.ficheDetails.annee_debut_fin); ;
            modelB.fiche.periode_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.mois_fin_fin, modelB.fiche.ficheDetails.annee_fin_fin); ;
            //////////
            modelB.fiche.echeancier_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.conception_prevue_mois_debut.ToString(), modelB.fiche.ficheDetails.conception_prevue_annee_debut.ToString()); ;
            modelB.fiche.echeancier_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.conception_prevue_mois_fin.ToString(), modelB.fiche.ficheDetails.conception_prevue_annee_fin.ToString()); ;

            modelB.fiche.echeancier_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.conception_finale_mois_debut.ToString(), modelB.fiche.ficheDetails.conception_finale_annee_debut.ToString()); ;
            modelB.fiche.echeancier_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.conception_finale_mois_fin.ToString(), modelB.fiche.ficheDetails.conception_finale_annee_fin.ToString()); ;
            //////////////
            modelB.fiche.planDevis_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.plan_prevu_mois_debut.ToString(), modelB.fiche.ficheDetails.plan_prevu_annee_debut.ToString()); ;
            modelB.fiche.planDevis_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.plan_prevu_mois_fin.ToString(), modelB.fiche.ficheDetails.plan_prevu_annee_fin.ToString()); ;

            modelB.fiche.planDevis_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.plan_final_mois_debut.ToString(), modelB.fiche.ficheDetails.plan_final_annee_debut.ToString()); ;
            modelB.fiche.planDevis_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.plan_final_mois_fin.ToString(), modelB.fiche.ficheDetails.plan_final_annee_fin.ToString()); ;
            /////////////////////
            modelB.fiche.construction_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.construction_prevue_mois_debut.ToString(), modelB.fiche.ficheDetails.construction_prevue_annee_debut.ToString()); ;
            modelB.fiche.construction_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.construction_prevue_mois_fin.ToString(), modelB.fiche.ficheDetails.construction_prevue_annee_fin.ToString()); ;

            modelB.fiche.construction_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.construction_finale_mois_debut.ToString(), modelB.fiche.ficheDetails.construction_finale_annee_debut.ToString()); ;
            modelB.fiche.construction_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.construction_finale_mois_fin.ToString(), modelB.fiche.ficheDetails.construction_finale_annee_fin.ToString()); ;


            currentCreateModel = modelB;
            Session["currentCreateModel"] = modelB;
            return View("CreateFactSheetF", modelB);
        }


        //public ActionResult CreateFactSheet(int ficheM_id = 3842, string menuVisible = "true")

        //{
        //    //pour line: $id_panier = (@$_SESSION['current_panier']['id_panier'] > 0) ? $_SESSION['current_panier']['id_panier'] : 0;

        //    Session["currentCreateModel"] = new BoomerangSearchModel();
        //    Session["current_panier"] = 0;
        //    Session["ADMIN"] = "yes";
        //    //a completer: qui est admin??


        //    //pour test seulement:
        //    // ficheM = _BoomerangService.returnFiche();

        //    //on recupere le nom de l utilisateur:
        //    var currentUser = UserService.CurrentUser;
        //    var nom = currentUser.FullName.Split(' ')[1];
        //    var prenom = currentUser.FullName.Split(' ')[0];
        //    FicheModel model = new FicheModel();
        //    model.ficheDetails = new Boomerang_tempFicheLang();
        //    if (ficheM_id == 0)
        //    {

        //        model.ficheDetails.id_fiche = 0;
        //        model.ficheDetails.duree = 0;
        //        model.ficheDetails.unite_1 = 0;
        //        model.ficheDetails.unite_2 = 0;
        //        model.ficheDetails.montant_operation = 0;
        //        model.ficheDetails.montant_equipe = 0;
        //        model.ficheDetails.montant_societe = 0;
        //        model.ficheDetails.montant_estimation = 0;
        //        model.ficheDetails.montant_soumission = 0;
        //        model.ficheDetails.montant_final = 0;
        //        model.ficheDetails.montant_changement_bpr = 0;
        //        model.ficheDetails.montant_changement_client = 0;
        //        model.ficheDetails.structure_prevu = 0;
        //        model.ficheDetails.structure_final = 0;
        //        model.ficheDetails.amenagement_prevu = 0;
        //        model.ficheDetails.amenagement_final = 0;
        //        model.ficheDetails.mecanique_prevu = 0;
        //        model.ficheDetails.mecanique_final = 0;
        //        model.ficheDetails.electricite_prevu = 0;
        //        model.ficheDetails.electricite_final = 0;
        //        model.ficheDetails.infrastructure_prevu = 0;
        //        model.ficheDetails.infrastructure_final = 0;
        //        model.ficheDetails.gestion_prevu = 0;
        //        model.ficheDetails.gestion_final = 0;
        //        model.ficheDetails.autre_prevu = 0;
        //        model.ficheDetails.autre_final = 0;
        //        model.ficheDetails.structure_prevu_bpr = 0;
        //        model.ficheDetails.structure_final_bpr = 0;
        //        model.ficheDetails.amenagement_prevu_bpr = 0;
        //        model.ficheDetails.amenagement_final_bpr = 0;
        //        model.ficheDetails.mecanique_prevu_bpr = 0;
        //        model.ficheDetails.mecanique_final_bpr = 0;
        //        model.ficheDetails.electricite_prevu_bpr = 0;
        //        model.ficheDetails.electricite_final_bpr = 0;
        //        model.ficheDetails.infrastructure_prevu_bpr = 0;
        //        model.ficheDetails.infrastructure_final_bpr = 0;
        //        model.ficheDetails.gestion_prevu_bpr = 0;
        //        model.ficheDetails.gestion_final_bpr = 0;
        //        model.ficheDetails.eco_energie_prevu = 0;
        //        model.ficheDetails.eco_energie_reel = 0;
        //        model.ficheDetails.eco_monetaire_prevu = 0;
        //        model.ficheDetails.eco_monetaire_reel = 0;
        //        model.ficheDetails.pri_prevu = 0;
        //        model.ficheDetails.pri_reel = 0;
        //        model.ficheDetails.conso_prevu = 0;
        //        model.ficheDetails.conso_reel = 0;
        //        model.ficheDetails.appui_financier_prevu = 0;
        //        model.ficheDetails.appui_financier_prevu2 = 0;
        //        model.ficheDetails.appui_financier_prevu3 = 0;
        //        model.ficheDetails.appui_financier_reel = 0;
        //        model.ficheDetails.appui_financier_reel2 = 0;
        //        model.ficheDetails.appui_financier_reel3 = 0;
        //        model.ficheDetails.montant_equipe_fin = 0;
        //        model.ficheDetails.montant_societe_fin = 0;
        //        model.ficheDetails.id_pole = 0;
        //        model.ficheDetails.id_createur = 0;
        //        model.ficheDetails.id_activite = 0;
        //        model.ficheDetails.id_type_unite_1 = 0;
        //        model.ficheDetails.id_type_unite_2 = 0;
        //        model.ficheDetails.id_agence = 0;
        //        model.ficheDetails.conception_prevue_mois_debut = 0;
        //        model.ficheDetails.conception_prevue_annee_debut = 0;
        //        model.ficheDetails.conception_prevue_mois_fin = 0;
        //        model.ficheDetails.conception_prevue_annee_fin = 0;
        //        model.ficheDetails.conception_finale_mois_debut = 0;
        //        model.ficheDetails.conception_finale_annee_debut = 0;
        //        model.ficheDetails.conception_finale_mois_fin = 0;
        //        model.ficheDetails.conception_finale_annee_fin = 0;
        //        model.ficheDetails.plan_prevu_mois_debut = 0;
        //        model.ficheDetails.plan_prevu_annee_debut = 0;
        //        model.ficheDetails.plan_prevu_mois_fin = 0;
        //        model.ficheDetails.plan_prevu_annee_fin = 0;
        //        model.ficheDetails.plan_final_mois_debut = 0;
        //        model.ficheDetails.plan_final_annee_debut = 0;
        //        model.ficheDetails.plan_final_mois_fin = 0;
        //        model.ficheDetails.plan_final_annee_fin = 0;
        //        model.ficheDetails.construction_prevue_mois_debut = 0;
        //        model.ficheDetails.construction_prevue_annee_debut = 0;
        //        model.ficheDetails.construction_prevue_mois_fin = 0;
        //        model.ficheDetails.construction_prevue_annee_fin = 0;
        //        model.ficheDetails.construction_finale_mois_debut = 0;
        //        model.ficheDetails.construction_finale_annee_debut = 0;
        //        model.ficheDetails.construction_finale_mois_fin = 0;
        //        model.ficheDetails.construction_finale_annee_fin = 0;
        //        model.ficheDetails.changement_bpr = 0;
        //        model.ficheDetails.changement_client = 0;
        //        model.ficheDetails.lastmod_time = null;
        //        model.ficheDetails.created_time = null;
        //        model.ficheDetails.code_affaire = "";
        //        model.ficheDetails.localisation = "";
        //        model.ficheDetails.id_province = "";
        //        model.ficheDetails.autre_lieu = "";
        //        model.ficheDetails.id_pays = "";
        //        model.ficheDetails.intitule = "";
        //        model.ficheDetails.intitule_court = "";
        //        model.ficheDetails.description = "";
        //        model.ficheDetails.id_type_mission = "";
        //        model.ficheDetails.mission = "";
        //        model.ficheDetails.particularite = "";
        //        model.ficheDetails.mois_debut = "";
        //        model.ficheDetails.annee_debut = "";
        //        model.ficheDetails.mois_fin = "";
        //        model.ficheDetails.annee_fin = "";
        //        model.ficheDetails.type_client = "";
        //        model.ficheDetails.client_nom = "";
        //        model.ficheDetails.client_adresse = "";
        //        model.ficheDetails.client_telephone = "";
        //        model.ficheDetails.client_mail = "";
        //        model.ficheDetails.contact_nom = "";
        //        model.ficheDetails.num_ref = "";
        //        model.ficheDetails.architecte = "";
        //        model.ficheDetails.conducteur_operation = "";
        //        model.ficheDetails.equipe = "";
        //        model.ficheDetails.equipe_interne = "";
        //        model.ficheDetails.id_charge_affaire = "";
        //        model.ficheDetails.proprio = "";
        //        model.ficheDetails.argumentaire_com = "";
        //        model.ficheDetails.code_qualification_opqibi = "";
        //        model.ficheDetails.lastmod_user = "";
        //        model.ficheDetails.lastmod_opqibi = "";
        //        model.ficheDetails.award = "";
        //        model.ficheDetails.nom_award = "";
        //        model.ficheDetails.lastmod_award = "";
        //        model.ficheDetails.imgForFiche = "";
        //        model.ficheDetails.etat = "";
        //        model.ficheDetails.active = "";
        //        model.ficheDetails.ancien_ref = "";
        //        model.ficheDetails.filiale = "";
        //        model.ficheDetails.ecart_periode_real = "";
        //        model.ficheDetails.ecart_cout = "";
        //        model.ficheDetails.nature_changement_bpr = "";
        //        model.ficheDetails.nature_changement_client = "";
        //        model.ficheDetails.ecart_structure = "";
        //        model.ficheDetails.ecart_amenagement = "";
        //        model.ficheDetails.ecart_mecanique = "";
        //        model.ficheDetails.ecart_electricite = "";
        //        model.ficheDetails.ecart_infrastructure = "";
        //        model.ficheDetails.ecart_gestion = "";
        //        model.ficheDetails.autre_nom = "";
        //        model.ficheDetails.ecart_autre = "";
        //        model.ficheDetails.ecart_structure_bpr = "";
        //        model.ficheDetails.ecart_amenagement_bpr = "";
        //        model.ficheDetails.ecart_mecanique_bpr = "";
        //        model.ficheDetails.ecart_electricite_bpr = "";
        //        model.ficheDetails.ecart_infrastructure_bpr = "";
        //        model.ficheDetails.ecart_gestion_bpr = "";
        //        model.ficheDetails.autre_nom_bpr = "";
        //        model.ficheDetails.autre_prevu_bpr = "";
        //        model.ficheDetails.autre_final_bpr = "";
        //        model.ficheDetails.ecart_autre_bpr = "";
        //        model.ficheDetails.mesurage_eco = "";
        //        model.ficheDetails.appui_financier_organisme = "";
        //        model.ficheDetails.appui_financier_organisme2 = "";
        //        model.ficheDetails.appui_financier_organisme3 = "";
        //        model.ficheDetails.respect_echeancier_budget = "";
        //        model.ficheDetails.respect_echeancier_budget_note = "";
        //        model.ficheDetails.numero_structure = "";
        //        model.ficheDetails.mois_debut_fin = "";
        //        model.ficheDetails.annee_debut_fin = "";
        //        model.ficheDetails.mois_fin_fin = "";
        //        model.ficheDetails.annee_fin_fin = "";
        //        model.ficheDetails.ecart_montant_equipe = "";
        //        model.ficheDetails.ecart_montant_societe = "";
        //        model.ficheDetails.annee_sort = "";
        //        model.ficheDetails.TLinxProjectNumber = "";
        //        model.ficheDetails.ProjectURLSharepoint = "";
        //        model.ficheDetails.libelle_createur = "";
        //        model.ficheDetails.libelle_charge_affaire = "";
        //        model.ficheDetails.libelle_activite = "";
        //        model.ficheDetails.libelle_type_unite_1 = "";
        //        model.ficheDetails.libelle_type_unite_2 = "";
        //        model.ficheDetails.libelle_unite_1 = "";
        //        model.ficheDetails.libelle_unite_2 = "";
        //        model.ficheDetails.libelle_agence = "";
        //        model.ficheDetails.libelle_pays = "";
        //        model.ficheDetails.libelle_province = "";
        //        model.ficheDetails.libelle_pole = "";
        //        model.ficheDetails.couleur = "";
        //    }
        //    else
        //    {
        //        model.ficheDetails = _BoomerangService.getFicheLang("", "", ficheM_id.ToString(), "");
        //    }
        //    BoomerangSearchModel modelB = new BoomerangSearchModel();
        //    modelB.fiche = new FicheModel();
        //    modelB.fiche.ficheDetails = model.ficheDetails;
        //    if (modelB.fiche.ficheDetails.id_fiche != 0)
        //    {
        //        var iImgForFiche = _BoomerangService.returnImageName(model.ficheDetails.id_fiche);
        //        var absolutePath = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\phototheque\" + model.ficheDetails.id_fiche + @"\thumbs750\" + iImgForFiche + ".png");
        //        if (System.IO.File.Exists(absolutePath))
        //        {
        //            modelB.fiche.image = @"/Content/images/TTProjetPlus_Boomerang_images/files/phototheque/" + model.ficheDetails.id_fiche + @"/thumbs750/" + iImgForFiche + ".png"; ;
        //        }

        //    }
        //    else
        //    {
        //        var absolutePath2 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\images\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".png");

        //        if (System.IO.File.Exists(absolutePath2))
        //        {
        //            modelB.fiche.image = @"/Content/images/TTProjetPlus_Boomerang_images/files/images/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".png";
        //        }
        //        else modelB.fiche.image = "";
        //    }

        //    var absolutePath3 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\awards\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf");
        //    if (System.IO.File.Exists(absolutePath3))
        //    {
        //        modelB.fiche.awardPDF = @"/Content/images/TTProjetPlus_Boomerang_images/files/awards/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf";
        //    }


        //    var absolutePath4 = HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\OPQIBI\" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf");
        //    if (System.IO.File.Exists(absolutePath3))
        //    {
        //        modelB.fiche.OPQIBI = @"/Content/images/TTProjetPlus_Boomerang_images/files/OPQIBI/" + model.ficheDetails.id_fiche.ToString().PadLeft(10, '0') + ".pdf";
        //    }


        //    modelB.listeExpertise = _BoomerangService.returnSearchCriterias();
        //    modelB.listePole = modelB.listeExpertise.Select(p => p.libelle_pole).Distinct().ToList();
        //    modelB.listeProvinces = _BoomerangService.returnProvinces();
        //    modelB.listePays = _BoomerangService.returnPays(Session["lang"].ToString().ToLower());
        //    modelB.listeTypeMission = _BoomerangService.returnTypeMission(Session["lang"].ToString().ToLower());
        //    modelB.listeTypeUnits = _BoomerangService.returnTypeUnits();
        //    modelB.listeAgences = _BoomerangService.returnAgences();
        //    ViewBag.MenuVisible = menuVisible;
        //    ViewBag.Lang = Session["lang"].ToString().ToLower();
        //    modelB.listItems = new List<ComplexItem>();


        //    foreach (string pole in modelB.listePole)
        //    {
        //        ComplexItem ci1 = new ComplexItem();
        //        ci1.id = modelB.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).id_pole;
        //        if (ci1.id == 18)
        //        {
        //            continue;
        //        }
        //        ci1.libelle = "*" + pole;
        //        ci1.color = "#" + modelB.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).couleur;
        //        modelB.listItems.Add(ci1);
        //        foreach (usp_Boomerang_getExpertises_Result item in modelB.listeExpertise)
        //        {


        //            if (item.libelle_pole == pole)
        //            {
        //                ComplexItem ci = new ComplexItem();
        //                ci.id = item.id_activite;
        //                ci.libelle = item.libelle_activite;
        //                ci.color = "#" + item.couleur;
        //                modelB.listItems.Add(ci);
        //            }
        //        }
        //    }


        //    //pour les dates:
        //    modelB.fiche.periode_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.mois_debut, modelB.fiche.ficheDetails.annee_debut);
        //    modelB.fiche.periode_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.mois_fin, modelB.fiche.ficheDetails.annee_fin); ;

        //    modelB.fiche.periode_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.mois_debut_fin, modelB.fiche.ficheDetails.annee_debut_fin); ;
        //    modelB.fiche.periode_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.mois_fin_fin, modelB.fiche.ficheDetails.annee_fin_fin); ;
        //    //////////
        //    modelB.fiche.echeancier_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.conception_prevue_mois_debut.ToString(), modelB.fiche.ficheDetails.conception_prevue_annee_debut.ToString()); ;
        //    modelB.fiche.echeancier_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.conception_prevue_mois_fin.ToString(), modelB.fiche.ficheDetails.conception_prevue_annee_fin.ToString()); ;

        //    modelB.fiche.echeancier_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.conception_finale_mois_debut.ToString(), modelB.fiche.ficheDetails.conception_finale_annee_debut.ToString()); ;
        //    modelB.fiche.echeancier_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.conception_finale_mois_fin.ToString(), modelB.fiche.ficheDetails.conception_finale_annee_fin.ToString()); ;
        //    //////////////
        //    modelB.fiche.planDevis_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.plan_prevu_mois_debut.ToString(), modelB.fiche.ficheDetails.plan_prevu_annee_debut.ToString()); ;
        //    modelB.fiche.planDevis_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.plan_prevu_mois_fin.ToString(), modelB.fiche.ficheDetails.plan_prevu_annee_fin.ToString()); ;

        //    modelB.fiche.planDevis_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.plan_final_mois_debut.ToString(), modelB.fiche.ficheDetails.plan_final_annee_debut.ToString()); ;
        //    modelB.fiche.planDevis_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.plan_final_mois_fin.ToString(), modelB.fiche.ficheDetails.plan_final_annee_fin.ToString()); ;
        //    /////////////////////
        //    modelB.fiche.construction_mois_annee_debut_initial = returnDate(modelB.fiche.ficheDetails.construction_prevue_mois_debut.ToString(), modelB.fiche.ficheDetails.construction_prevue_annee_debut.ToString()); ;
        //    modelB.fiche.construction_mois_annee_fin_initial = returnDate(modelB.fiche.ficheDetails.construction_prevue_mois_fin.ToString(), modelB.fiche.ficheDetails.construction_prevue_annee_fin.ToString()); ;

        //    modelB.fiche.construction_mois_annee_debut_final = returnDate(modelB.fiche.ficheDetails.construction_finale_mois_debut.ToString(), modelB.fiche.ficheDetails.construction_finale_annee_debut.ToString()); ;
        //    modelB.fiche.construction_mois_annee_fin_final = returnDate(modelB.fiche.ficheDetails.construction_finale_mois_fin.ToString(), modelB.fiche.ficheDetails.construction_finale_annee_fin.ToString()); ;


        //    currentCreateModel = modelB;
        //    Session["currentCreateModel"] = modelB;
        //    return View("CreateFactSheetF", modelB);
        //}

        public string returnDate(string debut, string fin)
        {
            var date = "";
            if (string.IsNullOrEmpty(debut) && string.IsNullOrEmpty(fin))
            {
                date = "";
            }
            else if (string.IsNullOrEmpty(debut) && !string.IsNullOrEmpty(fin))
            {
                date = "01-" + fin;
            }
            else if (!string.IsNullOrEmpty(debut) && !string.IsNullOrEmpty(fin))
            {
                if (Int32.Parse(debut) < 10)
                {
                    debut = "0" + debut;
                }
                date = debut + "-" + fin;
            }
            return date;
        }
        public ActionResult Index(string menuVisible = "true")

        {
            //pour line: $id_panier = (@$_SESSION['current_panier']['id_panier'] > 0) ? $_SESSION['current_panier']['id_panier'] : 0;
            Session["current_panier"] = 0;
            Session["ADMIN"] = true;
            //a completer: qui est admin??

            //on recupere le nom de l utilisateur:
            var currentUser = UserService.CurrentUser;
            var nom = currentUser.FullName.Split(' ')[1];
            var prenom = currentUser.FullName.Split(' ')[0];


            ViewBag.MenuVisible = menuVisible;
            BoomerangSearchModel model = new BoomerangSearchModel();
            model.listePaniers = _BoomerangService.returnListePanier(nom, prenom);
            model.listeExpertise = _BoomerangService.returnSearchCriterias();
            model.listePole = model.listeExpertise.Select(p => p.libelle_pole).Distinct().ToList();
            model.listActivite = model.listeExpertise.Select(p => p.libelle_activite).Distinct().ToList();
            model.listItems = new List<ComplexItem>();


            foreach (string pole in model.listePole)
            {
                ComplexItem ci1 = new ComplexItem();
                ci1.id = model.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).id_pole;
                ci1.libelle = "*" + pole;
                ci1.color = "#" + model.listeExpertise.FirstOrDefault(p => p.libelle_pole == pole).couleur;
                model.listItems.Add(ci1);
                foreach (usp_Boomerang_getExpertises_Result item in model.listeExpertise)
                {


                    if (item.libelle_pole == pole)
                    {
                        ComplexItem ci = new ComplexItem();
                        ci.id = item.id_activite;
                        ci.libelle = item.libelle_activite;
                        ci.color = "#" + item.couleur;
                        model.listItems.Add(ci);
                    }
                }
            }

            model.listProjects = _BoomerangService.returnListProjects();
            model.listBusinessPlaces = _BoomerangService.returnListBusinessPlaces();
            model.typeUnities = _BoomerangService.returnTypeUnities();
            model.customers = _BoomerangService.returnCustomers();
            model.disciplines = _BoomerangService.returnDisciplines();

            return View("SearchPage", model);
        }
        public ActionResult submitSearch(bool fullExpression, string id_panier = null, string keywords = null, string id_activite = null, string etat = "", string code_affaire = null, int annee_debut = 0, int annee_fin = 0, string orderby = null,
          string id_agence = "", string[] id_type_mission = null, string supInf = null, int montant_operation = 0, string supInfUnite = null, int numUnite = 0, string unite = null,
           string langue = null, int fiche_depuis = 0, string customers = null, string disciplines = null)

        {
            string sWhere = etat == "" ? " and etat in ('ONLINE', 'OFFLINE', 'WAITING_DA', 'FORBIDDEN', 'DRAFT')" : " and f.etat='" + etat + "' ";
            string sWhereFinal = "";
            // string resultNumber = "";
            string sResults = "";
            try
            {
                //pour la recherche par mot clés, on commnece par recuperer tous les chmaps de la table fiche:
                //1. on met les mots clés dans une liste:
                keywords = keywords.TrimStart().TrimEnd();
                //si le champ keyword n est pas vide
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    List<string> keywordListe = new List<string>();
                    List<string> listKW = new List<string>();
                    if (fullExpression == true)
                    {
                        keywords = keywords.Replace("'", "''");
                        keywordListe.Add(keywords);
                    }
                    else
                    {
                        var kw = keywords.Replace(" ", ";");
                        kw = keywords.Replace("'", ";");
                        listKW = kw.Split(';').ToList();
                        foreach (string word in listKW)
                        {
                            if (word != "") keywordListe.Add(word);
                        }
                    }

                    var ficheFields = _BoomerangService.returnFicheTableFields();

                    foreach (string word in keywordListe)
                    {
                        if (word.Length >= 2)
                        {
                            sWhere += " AND ( ";
                            foreach (string field in ficheFields)
                            {
                                if (field != "lastmod_user" || field != "id_createur")
                                {
                                    sWhere += "f." + field + " like '%" + word + "%' or ";
                                }
                            }

                            sWhere += " f.localisation LIKE '_____" + word + "%' )";
                            sWhere += " and lastmod_user not LIKE '%" + word + "%'  ";
                        }

                        var sSQL = _BoomerangService.getUtilisateurId(word);
                        if (sSQL != 0)
                        {
                            sWhere += " AND f.id_createur not LIKE '%" + sSQL + "%' ";
                        }

                    }



                }
                if (!string.IsNullOrEmpty(id_activite) && !string.IsNullOrWhiteSpace(id_activite) && id_activite != "all")
                {
                    sWhere += " and f.id_activite =" + Int32.Parse(id_activite);
                }

                //
                //demander a line a propos de id_pole
                //if (is_numeric($aSearch['id_pole']))
                //{
                //$sWhere.= " AND a.`id_pole` = '{$aSearch['id_pole']}' ";
                //}


                if (!string.IsNullOrEmpty(id_agence) && !string.IsNullOrWhiteSpace(id_agence))
                {
                    sWhere += " and ag.id_agence =" + id_agence;
                }
                //
                if (id_type_mission != null && id_type_mission.Length > 0)
                {
                    foreach (string item in id_type_mission)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            sWhere += " AND (f.id_type_mission LIKE '%" + item + "%' OR f.id_type_mission LIKE '%" + id_type_mission + "')";
                        }
                    }
                }
                //
                if (annee_debut != 0 && annee_fin != 0)
                {
                    sWhere += " AND f.annee_debut BETWEEN " + annee_debut + " AND " + annee_fin + " AND f.annee_fin  BETWEEN " + annee_debut + " AND " + annee_fin + " ";
                }

                //
                if (fiche_depuis != 0)
                {
                    sWhere += " AND  f.created_time > DATEADD(month, -" + fiche_depuis + ", getdate())";
                }
                //
                if ((supInf == "<=" || supInf == ">=") && (montant_operation != 0))
                {
                    sWhere += " AND f.montant_operation " + supInf + " " + montant_operation;
                }

                //
                if ((supInfUnite == "<=" || supInfUnite == ">=") && numUnite != 0 && !string.IsNullOrEmpty(unite))
                {
                    sWhere += " AND (f.unite_1 " + supInfUnite + " " + numUnite + " AND f.unite_1 != 0 AND f.id_type_unite_1 =" + unite + ")"
                             + " OR( f.unite_2 " + supInfUnite + " " + numUnite + " AND f.unite_2 != 0 AND f.id_type_unite_2 = " + unite + ")";

                }

                //
                if (!string.IsNullOrEmpty(code_affaire))
                {
                    sWhere += " AND f.code_affaire LIKE '" + code_affaire + "%' ";
                }
                //
                if (customers != "" && customers != "all")
                {
                    sWhere += " AND f.client_nom LIKE '" + customers + "%'";
                }

                if (disciplines != "" && disciplines != "all")
                {
                    sWhere += " AND f.mission LIKE '%" + disciplines + "%'";
                }

                //ou ce parametre etat est il initialisé 
                //if (!empty($aSearch['etat']))
                //{
                //$sWhere.= " AND f.etat IN ('{$aSearch['etat']}') ";
                //}

                var langueQuery = !string.IsNullOrEmpty(langue) ? "_" + langue : "";

                sWhereFinal += "SELECT COUNT(id_fiche) AS numrows FROM fiche" + langueQuery + " as f "; ;
                sWhereFinal += " LEFT JOIN activite AS a ON a.id_activite = f.id_activite ";
                sWhereFinal += " LEFT JOIN agence AS ag ON ag.id_agence = f.id_agence ";
                sWhereFinal += " LEFT JOIN type_mission AS t ON t.id_type_mission = f.id_type_mission ";
                sWhereFinal += " LEFT JOIN type_unite AS tu1 ON tu1.id_type_unite = f.id_type_unite_1 ";
                sWhereFinal += " LEFT JOIN type_unite AS tu2 ON tu2.id_type_unite = f.id_type_unite_2 ";
                sWhereFinal += " WHERE f.active = '1' " + sWhere;




                //envoyer la query a la sp sous forme de string: https://stackoverflow.com/questions/30226688/passing-a-query-as-a-parameter-in-stored-proc
                //CREATE PROCEDURE RunThisThang(@InputStr)
                //AS
                //DECLARE @QUeryStr NVARCHAR(500)
                //SET @QueryStr = 'UPDATE [RevenueAccrual] SET Posted=1 Where [RevenueAccrual].RevenueAccrualID IN (' + @InputStr + ')'
                //EXEC(@QueryStr)
                //sWhereFinal = sWhereFinal.Replace("'", "''");
                //  sWhereFinal = "' " + sWhereFinal + " '";

                //on n a pas besoin de cette partie... c etait pour la pagination 
                //    resultNumber = _BoomerangService.queryResult(sWhereFinal);

                //if(Int32.Parse(resultNumber)>0)
                //{
                //line: $id_panier = (@$_SESSION['current_panier']['id_panier'] > 0) ? $_SESSION['current_panier']['id_panier'] : 0;
                int id_panierF = 0;
                if (id_panier != "")
                {
                    id_panierF = Int32.Parse(id_panier) > 0 ? Int32.Parse(id_panier) : 0;
                }
                Session["current_panier"] = id_panierF;
                sResults = " SELECT f.intitule_court,f.localisation,f.annee_fin,f.id_fiche ,p.couleur ,f.etat,f.nom_award,f.lastmod_opqibi";
                sResults += " ,iif(f.id_fiche IN (SELECT id_fiche  FROM fiche_en WHERE etat = 'ONLINE'), 1, 0) AS existInEn  ";
                sResults += " ,iif(f.id_fiche IN (SELECT id_fiche FROM fiche_esp WHERE etat = 'ONLINE'), 1, 0) AS existInEsp ";
                sResults += ",iif(f.id_fiche IN (SELECT id_fiche FROM panier_fiche WHERE id_panier = " + id_panierF + "), 1, 0) AS in_panier ";
                sResults += " ,iif(f.id_fiche IN (SELECT id_fiche FROM fiche WHERE argumentaire_com IS NOT NULL), 1, 0) AS argComExist ";
                sResults += " ,iif(f.id_fiche IN (SELECT id_fiche FROM fiche_en WHERE argumentaire_com IS NOT NULL), 1, 0) AS argComExistInEn ";
                sResults += " ,iif(f.id_fiche IN (SELECT id_fiche FROM fiche_esp WHERE argumentaire_com IS NOT NULL), 1, 0) AS argComExistInEsp ";
                sResults += " FROM fiche" + langueQuery + "  AS f ";
                sResults += " LEFT JOIN activite AS a ON a.id_activite = f.id_activite ";
                sResults += " LEFT JOIN pole AS p ON p.id_pole = a.id_pole ";
                sResults += " LEFT JOIN agence AS ag ON ag.id_agence = f.id_agence ";
                sResults += " LEFT JOIN type_mission AS t ON t.id_type_mission = f.id_type_mission ";
                sResults += " LEFT JOIN type_unite AS tu1 ON tu1.id_type_unite = f.id_type_unite_1 ";
                sResults += " LEFT JOIN type_unite AS tu2 ON tu2.id_type_unite = f.id_type_unite_2 ";                
                sResults += " WHERE f.active='1' " + sWhere;


                //pas besoin de la recherche avec le orderby: deja fourni par datatables:
                //    sResults += (orderby ==null || orderby =="")? "" : " ORDER BY f." + orderby;



                var ListFiches = _BoomerangService.ExecuteStoredProcedure(sResults, "researchResult");
                return PartialView("_searchFicheResultPartial", ListFiches);

                //  var ListFiches = _BoomerangService.GetDataTableFast(sResults);
                //return PartialView("_searchFicheResultPartialDT", ListFiches);

                // return PartialView("_searchFicheResultPartial_NoDatatables", ListFiches);

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public ActionResult returnFicheDetails(int idFiche, string sForcart = "", int iIdPanier = 0)
        {
            try
            {
                FicheModel finalModel = new FicheModel();
                //ici j ai mis la logique de l attribution de l'image (et OPQIBI et award) pour la fiche; cette logique est a validée avc Line:
                var sFileName = idFiche.ToString().PadLeft(10, '0');
                var iImgForFiche = _BoomerangService.GetImgForFiche(idFiche);

                if (iImgForFiche != "")
                {
                    if (System.IO.File.Exists("files/phototheque/" + idFiche.ToString() + "/thumbs750/" + iImgForFiche + ".png"))
                    {
                        finalModel.image = "files/phototheque/" + idFiche.ToString() + "/thumbs750/" + iImgForFiche + ".png";
                    }
                }
                else
                {
                    if (System.IO.File.Exists("files/images/" + sFileName + ".png"))
                    {
                        finalModel.image = "files/images/" + sFileName + ".png";
                    }
                }

                if (System.IO.File.Exists("files/OPQIBI/" + sFileName + ".pdf"))
                {
                    finalModel.OPQIBI = "files/OPQIBI/" + sFileName + ".pdf";
                }

                if (System.IO.File.Exists("files/awards/" + sFileName + ".pdf"))
                {
                    finalModel.award = "files/awards/" + sFileName + ".pdf";
                }

                //voir fichier: C:\xampp\htdocs\site\Boomerang\pages\fiche.view.inc.php
                //il faut utiliser la fonction LoadFromId du fichier fiche.view.class.back.php C:\xampp\htdocs\site\Boomerang\includes\boomerang.fiche.class.back.php

                //1. en premier lieu il faut determiner quelle est la langue de la session 
                var langSession = Session["lang"].ToString().ToLower();

                finalModel.services = _BoomerangService.GetAllLibelleTypeMission(idFiche, langSession);

                langSession = langSession != "fr" ? "_" + langSession : "";

                //2. on cherche d'abord dans la table fiche_(la_langue) si il existe une fiche correspondant a ce id_fiche 
                //si on est en francais, on cherche dans la table fiche . 
                //si on n a rien trouvé, on recherche dans la table fiche (qui est en francais) et on va charger un  minimum e données

                //donc en premier , recherche dans la table "preferée", celle qui correspond a la langue de la session: 
                sForcart = sForcart == "admin" ? "" : sForcart;

                var andWhere = "";
                if (iIdPanier != 0)
                {
                    andWhere = " AND f.id_panier = " + iIdPanier.ToString();
                    if (existForCart(idFiche, langSession, iIdPanier) == false)
                    {
                        sForcart = "";
                        andWhere = "";
                    }
                }

                if (idFiche <= 0)
                {
                    return null;
                }


                var fiche1 = _BoomerangService.getFicheLang(langSession, sForcart, idFiche.ToString(), andWhere);
                if (fiche1 != null)
                {
                    finalModel.ficheDetails = fiche1;
                    finalModel.ficheDetails.description = finalModel.ficheDetails.description.Replace("\r\n", "<br />").Replace("\n", "<br />");

                    finalModel.ficheDetails.equipe_interne = (finalModel.ficheDetails.equipe_interne == null || finalModel.ficheDetails.equipe_interne == "") ? "" : finalModel.ficheDetails.equipe_interne.Replace("\r\n", "\n");
                    List<string> noResult = new List<string> { "S//O" };

                    finalModel.equipeList = (finalModel.ficheDetails.equipe_interne == "") ? noResult : finalModel.ficheDetails.equipe_interne.Split('\n').ToList();
                    return PartialView("_ficheDetailsPartial", finalModel);
                }
                else
                {

                    var fiche2 = convertTo_Boomerang_tempFicheLang(_BoomerangService.getFicheInFrenchFromId(idFiche)); //c'est la sp qui va renvoyer un minimum de données en francais pour le idFiche
                    finalModel.ficheDetails = fiche2;
                    return PartialView("_ficheDetailsPartial", finalModel);

                }

            }

            catch (Exception e)

            {
                return null;
            }
        }


        public bool existForCart(int iId, string sLang, int iIdPanier = 0)
        {
            var andWhere = (iIdPanier != 0) ? " AND id_panier = " + iIdPanier : "";
            var sSQL = "SELECT  id_fiche";
            sSQL += " ,iif(f.id_fiche IN (SELECT id_fiche  FROM fiche_en WHERE etat = 'ONLINE'), 1, 0) AS existInEn  ";
            sSQL += " ,iif(f.id_fiche IN (SELECT id_fiche FROM fiche_esp WHERE etat = 'ONLINE'), 1, 0) AS existInEsp ";
            sSQL += " FROM fiche" + sLang + "_forcart";
            sSQL += " WHERE id_fiche = " + iId.ToString();
            sSQL += andWhere;
            var ListFiches = _BoomerangService.ExecuteStoredProcedure(sSQL, "researchResult");
            return ListFiches.Count > 0 ? true : false;
        }

        public Boomerang_tempFicheLang convertTo_Boomerang_tempFicheLang(usp_Boomerang_getFicheInFrenchFromId_Result toconvert)
        {
            Boomerang_tempFicheLang model = new Boomerang_tempFicheLang();
            model.code_affaire = toconvert.code_affaire;
            model.id_activite = Int32.Parse(toconvert.id_activite.ToString());
            model.localisation = toconvert.localisation;
            model.id_province = toconvert.id_province;
            model.autre_lieu = toconvert.autre_lieu;
            model.id_pays = toconvert.id_pays;
            model.id_type_mission = toconvert.id_type_mission;
            model.mois_debut = toconvert.mois_debut;
            model.annee_debut = toconvert.annee_debut;
            model.mois_fin = toconvert.mois_fin;
            model.annee_fin = toconvert.annee_fin;
            model.duree = toconvert.duree;
            model.id_type_unite_1 = Int32.Parse(toconvert.id_type_unite_1.ToString());
            model.unite_1 = decimal.Parse(toconvert.unite_1.ToString());
            model.id_type_unite_2 = Int32.Parse(toconvert.id_type_unite_2.ToString());
            model.unite_2 = decimal.Parse(toconvert.unite_2.ToString());
            model.montant_operation = decimal.Parse(toconvert.montant_operation.ToString());
            model.montant_equipe = decimal.Parse(toconvert.montant_equipe.ToString());
            model.montant_societe = decimal.Parse(toconvert.montant_societe.ToString());
            model.type_client = toconvert.type_client;
            model.client_nom = toconvert.client_nom;
            model.client_adresse = toconvert.client_adresse;
            model.client_telephone = toconvert.client_telephone;
            model.contact_nom = toconvert.contact_nom;
            model.num_ref = toconvert.num_ref;
            model.equipe_interne = toconvert.equipe_interne;
            model.architecte = toconvert.architecte;
            model.conducteur_operation = toconvert.conducteur_operation;
            model.id_agence = Int32.Parse(toconvert.id_agence.ToString());
            model.id_charge_affaire = toconvert.id_charge_affaire;
            model.proprio = toconvert.proprio;
            model.code_qualification_opqibi = toconvert.code_qualification_opqibi;
            model.etat = toconvert.etat;
            model.libelle_charge_affaire = toconvert.libelle_charge_affaire;

            return model;

        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string ExportData, string id_fiche)
        {
            var fiche = returnFicheDetails(Int32.Parse(id_fiche));


            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(ExportData);
            ExportData = doc.DocumentNode.OuterHtml;

            var cssText = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Content/HTML_To_PDF/htmlToPDF.css"));
            var html = ExportData;
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText));
                var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
                StringReader reader = new StringReader(ExportData);
                Document PdfFile = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();
                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\images\0000003622.png"), true);
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                if (pic.Height > pic.Width)
                {
                    //Maximum height is 800 pixels.
                    float percentage = 0.0f;
                    percentage = 350 / pic.Height;
                    pic.ScalePercent(percentage * 100);
                }
                else
                {
                    //Maximum width is 600 pixels.
                    float percentage = 0.0f;
                    percentage = 270 / pic.Width;
                    pic.ScalePercent(percentage * 100);
                }

                //pic.Border = iTextSharp.text.Rectangle.BOX;
                //pic.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //pic.BorderWidth = 3f;
                pic.SetAbsolutePosition(38, 570);
                PdfFile.Add(pic);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, htmlMemoryStream, cssMemoryStream);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportArg(string ExportDataArg, string id_fiche_arg)
        {

            ExportDataArg = ExportDataArg.Replace("&amp;#9642;", " - ");
            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(ExportDataArg);
            ExportDataArg = doc.DocumentNode.OuterHtml;

            //var cssText = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Content/HTML_To_PDF/htmlToPDF.css"));
            //var html = ExportDataArg;
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                //var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText));
                //var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
                //StringReader reader = new StringReader(ExportDataArg);
                Document PdfFile = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();
                var arrayTex = ExportDataArg.Split(new string[] { "&lt;br /&gt;" }, StringSplitOptions.None);
                foreach (string item in arrayTex)
                {

                    PdfFile.Add(new Paragraph(item));
                }
                //  XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, htmlMemoryStream, cssMemoryStream);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", "ExportDataArg.pdf");
            }
        }


        ////https://theartofdev.com/html-renderer/
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(String ExportData)
        //{
        //    Byte[] res = null;
        //    string cssStr = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Content/HTML_To_PDF/htmlToPDF.css"));//bootstrap.min.css
        //    CssData css = PdfGenerator.ParseStyleSheet(cssStr);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] array = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HttpContext.Server.MapPath(@"~\Content\images\TTProjetPlus_Boomerang_images\files\images\0000003623.png")));
        //        ExportData+= " img.logo { width:110px;height:110px;content: url('data:/Content/images/TTProjetPlus_Boomerang_images/files/images;base64," + Convert.ToBase64String(array) + "')} ";
        //        var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(ExportData, PdfSharp.PageSize.A4, 20, css, null, null);
        //        pdf.Save(ms);
        //        res = ms.ToArray();
        //    }
        //    return File(res.ToArray(), "application/pdf", "ExportData.pdf");
        //}


        public ActionResult returnFicheDetailsPDF(int idFiche, string sForcart = "", int iIdPanier = 0)
        {
            try
            {
                FicheModel finalModel = new FicheModel();
                //ici j ai mis la logique de l attribution de l'image (et OPQIBI et award) pour la fiche; cette logique est a validée avc Line:
                var sFileName = idFiche.ToString().PadLeft(10, '0');
                var iImgForFiche = _BoomerangService.GetImgForFiche(idFiche);

                if (iImgForFiche != "")
                {
                    if (System.IO.File.Exists("files/phototheque/" + idFiche.ToString() + "/thumbs750/" + iImgForFiche + ".png"))
                    {
                        finalModel.image = "files/phototheque/" + idFiche.ToString() + "/thumbs750/" + iImgForFiche + ".png";
                    }
                }
                else
                {
                    if (System.IO.File.Exists("files/images/" + sFileName + ".png"))
                    {
                        finalModel.image = "files/images/" + sFileName + ".png";
                    }
                }

                if (System.IO.File.Exists("files/OPQIBI/" + sFileName + ".pdf"))
                {
                    finalModel.OPQIBI = "files/OPQIBI/" + sFileName + ".pdf";
                }

                if (System.IO.File.Exists("files/awards/" + sFileName + ".pdf"))
                {
                    finalModel.award = "files/awards/" + sFileName + ".pdf";
                }

                //voir fichier: C:\xampp\htdocs\site\Boomerang\pages\fiche.view.inc.php
                //il faut utiliser la fonction LoadFromId du fichier fiche.view.class.back.php C:\xampp\htdocs\site\Boomerang\includes\boomerang.fiche.class.back.php

                //1. en premier lieu il faut determiner quelle est la langue de la session 
                var langSession = Session["lang"].ToString().ToLower();

                finalModel.services = _BoomerangService.GetAllLibelleTypeMission(idFiche, langSession);

                langSession = langSession != "fr" ? "_" + langSession : "";

                //2. on cherche d'abord dans la table fiche_(la_langue) si il existe une fiche correspondant a ce id_fiche 
                //si on est en francais, on cherche dans la table fiche . 
                //si on n a rien trouvé, on recherche dans la table fiche (qui est en francais) et on va charger un  minimum e données

                //donc en premier , recherche dans la table "preferée", celle qui correspond a la langue de la session: 
                sForcart = sForcart == "admin" ? "" : sForcart;

                var andWhere = "";
                if (iIdPanier != 0)
                {
                    andWhere = " AND f.id_panier = " + iIdPanier.ToString();
                    if (existForCart(idFiche, langSession, iIdPanier) == false)
                    {
                        sForcart = "";
                        andWhere = "";
                    }
                }

                if (idFiche <= 0)
                {
                    return null;
                }


                var fiche1 = _BoomerangService.getFicheLang(langSession, sForcart, idFiche.ToString(), andWhere);
                if (fiche1 != null)
                {
                    finalModel.ficheDetails = fiche1;
                    //finalModel.ficheDetails.description = finalModel.ficheDetails.description.Replace("\r\n","\n").Replace("\n", "\n");
                    var text = new iTextSharp.text.Chunk(finalModel.ficheDetails.description);
                    finalModel.texteDesc = text;
                    finalModel.ficheDetails.equipe_interne = (finalModel.ficheDetails.equipe_interne == null || finalModel.ficheDetails.equipe_interne == "") ? "" : finalModel.ficheDetails.equipe_interne.Replace("\r\n", "\n");
                    List<string> noResult = new List<string> { "S//O" };

                    finalModel.equipeList = (finalModel.ficheDetails.equipe_interne == "") ? noResult : finalModel.ficheDetails.equipe_interne.Split('\n').ToList();
                    return PartialView("_FichePDF_Partial", finalModel);
                }
                else
                {

                    var fiche2 = convertTo_Boomerang_tempFicheLang(_BoomerangService.getFicheInFrenchFromId(idFiche)); //c'est la sp qui va renvoyer un minimum de données en francais pour le idFiche
                    finalModel.ficheDetails = fiche2;
                    return PartialView("_FichePDF_Partial", finalModel);

                }

            }

            catch (Exception e)

            {
                return null;
            }
        }

        public ActionResult returnFicheDetailsPDF_Arg(int idFiche, string sForcart = "", int iIdPanier = 0)
        {
            try
            {
                FicheModel finalModel = new FicheModel();


                if (idFiche <= 0)
                {
                    return null;
                }


                var fiche1 = _BoomerangService.getFicheLang("", sForcart, idFiche.ToString(), "");
                if (fiche1 != null)
                {
                    finalModel.ficheDetails = fiche1;
                    //  finalModel.ficheDetails.argumentaire_com.Replace("<br />", System.Environment.NewLine);
                    return PartialView("_FichePDF_Partial_Arg", finalModel);
                }
                else
                {

                    var fiche2 = convertTo_Boomerang_tempFicheLang(_BoomerangService.getFicheInFrenchFromId(idFiche)); //c'est la sp qui va renvoyer un minimum de données en francais pour le idFiche
                    finalModel.ficheDetails = fiche2;
                    //      finalModel.ficheDetails.argumentaire_com.Replace("<br />", System.Environment.NewLine);
                    return PartialView("_FichePDF_Partial_Arg", finalModel);

                }

            }

            catch (Exception e)

            {
                return null;
            }
        }

        public decimal? convertToEnglishDecimal(string input)
        {
            // unify string (no spaces, only . )
            string output = input.Trim().Replace(" ", "").Replace(",", ".");

            // split it on points
            string[] split = output.Split('.');

            if (split.Count() > 1)
            {
                // take all parts except last
                output = string.Join("", split.Take(split.Count() - 1).ToArray());

                // combine token parts with last part
                output = string.Format("{0}.{1}", output, split.Last());
            }

            // parse double invariant
            double d = double.Parse(output, CultureInfo.InvariantCulture);
            return Convert.ToDecimal(d);
        }


        public ActionResult Carts(int id_panier)

        {
            var currentUser = UserService.CurrentUser;
            var nom = currentUser.FullName.Split(' ')[1];
            var prenom = currentUser.FullName.Split(' ')[0];
            BoomerangHomeModel model = new BoomerangHomeModel();
            model.listePanier = _BoomerangService.returnListePanier(nom, prenom);
            if (id_panier != -1)
            {
                string sResults = "";
                string langue = Session["lang"].ToString().ToLower();
                var langueQuery = langue != "fr" ? "_" + langue : "";
                sResults = "SELECT f.*, p.couleur, m.intitule_court AS intitule_court_modif";
                sResults += " FROM fiche" + langueQuery + "  AS f ";
                sResults += " LEFT JOIN activite AS a ON a.id_activite = f.id_activite ";
                sResults += " LEFT JOIN pole AS p ON p.id_pole = a.id_pole ";
                sResults += "  LEFT JOIN fiche" + langueQuery + "_forcart AS m ON m.id_fiche = f.id_fiche ";
                sResults += "  AND m.id_panier =" + id_panier;
                sResults += "  WHERE f.id_fiche IN (SELECT id_fiche";
                sResults += " FROM panier_fiche ";
                sResults += " WHERE id_panier = " + id_panier + ") AND f.etat = 'ONLINE'";

                model.listeFiches = _BoomerangService.ExecuteStoredProcedure(sResults, "researchResult");

            }
            else model.listeFiches = null;
            return View(model);
        }

        public ActionResult CropperTest()

        {
            return View();
        }

        public ActionResult HelpPage()

        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateImg(string img)
        {
            //currentCreateModel.fiche.image = img;
            //BoomerangSearchModel model = new BoomerangSearchModel();
            //model = (BoomerangSearchModel)Session["currentCreateModel"];
            //model.fiche.image = img;
            try
            {
                ////https://www.codeproject.com/Questions/1089974/How-to-save-image-into-folder-or-directory
                ///
                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/ImagesFolder/"));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }


            string folderPath = Server.MapPath("~/ImagesFolder/");  //Create a Folder in your Root directory on your solution.
            string fileName = "IMageName.jpeg";
            string imagePath = folderPath + fileName ;

            string base64StringData = img; // Your base 64 string data
            string cleandata = base64StringData.Replace("data:image/png;base64,", "");
            byte[] data = System.Convert.FromBase64String(cleandata);
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);
            img1.Save(imagePath , System.Drawing.Imaging.ImageFormat.Jpeg);
                Directory.GetFiles(Server.MapPath("~/ImagesFolder/"));
                return Json(new { newImg = imagePath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { newImg = "failed" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CreateNewFiche(string CodeAffaire, string Activite, string Localisation, string Province, string Pays, string AutreLieu,
                                     string Intitule, string IntituleCourt, string Description, string TLinxProjectNumber, string ProjectURLSharepoint,
                                     string ClientNom, string ClientAdresse, string ClientTelephone, string ContactNom, string ClientMail, string numRef)
        {
            try
            {

                var IdFiche = _BoomerangService.returnIdFiche(CodeAffaire);

                if (IdFiche == 0)
                {
                    _BoomerangService.InsertIntoFicheTable(CodeAffaire, Activite, Localisation, Province, Pays, AutreLieu,
                                                                      Intitule, IntituleCourt, Description, TLinxProjectNumber, ProjectURLSharepoint,
                                                                      ClientNom, ClientAdresse, ClientTelephone, ContactNom, ClientMail, numRef);

                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = _BoomerangService.InsertIntoFicheTable(CodeAffaire, Activite, Localisation, Province, Pays, AutreLieu,
                                                                      Intitule, IntituleCourt, Description, TLinxProjectNumber, ProjectURLSharepoint,
                                                                      ClientNom, ClientAdresse, ClientTelephone, ContactNom, ClientMail, numRef);

                    if (result == true)
                    {
                        try
                        {

                            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception e)
                        {
                            return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);
                }
               

            }
            catch (Exception e)
            {
                return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult CreateUpdateNewFiche(string code_affaire, string id_activite, string localisation, string id_province, string autre_lieu, string id_pays, string intitule, string intitule_court, string description,
                                                string mois_debut, string annee_debut, string mois_fin, string annee_fin, string duree, string id_type_unite_1, string id_type_unite_2, string montant_equipe,
                                                string montant_societe, string client_nom, string client_adresse, string client_telephone, string client_mail, string contact_nom, string num_ref, string architecte,
                                                string conducteur_operation, string equipe, string equipe_interne, string id_charge_affaire, string proprio, string plan_prevu_mois_debut, string plan_prevu_annee_debut,
                                                string plan_prevu_mois_fin, string plan_prevu_annee_fin, string plan_final_mois_debut, string plan_final_annee_debut, string plan_final_mois_fin, string plan_final_annee_fin,
                                                string construction_prevue_mois_debut, string construction_prevue_annee_debut, string construction_prevue_mois_fin, string construction_prevue_annee_fin,
                                                string construction_finale_mois_debut, string construction_finale_annee_debut, string construction_finale_mois_fin, string construction_finale_annee_fin,
                                                string ecart_periode_real, string montant_estimation, string montant_soumission, string montant_final, string ecart_cout, string changement_bpr, string montant_changement_bpr,
                                                string nature_changement_bpr, string changement_client, string montant_changement_client, string nature_changement_client, string structure_prevu, string structure_final,
                                                string ecart_structure, string amenagement_prevu, string amenagement_final, string ecart_amenagement, string mecanique_prevu, string mecanique_final,
                                                string ecart_mecanique, string electricite_prevu, string electricite_final, string ecart_electricite, string infrastructure_prevu, string infrastructure_final, string ecart_infrastructure,
                                                string gestion_prevu, string gestion_final, string ecart_gestion, string autre_nom, string autre_prevu, string autre_final, string ecart_autre, string structure_prevu_bpr,
                                                string structure_final_bpr, string ecart_structure_bpr, string amenagement_prevu_bpr, string amenagement_final_bpr, string ecart_amenagement_bpr, string mecanique_prevu_bpr,
                                                string mecanique_final_bpr, string ecart_mecanique_bpr, string electricite_prevu_bpr, string electricite_final_bpr, string ecart_electricite_bpr, string infrastructure_prevu_bpr,
                                                string infrastructure_final_bpr, string ecart_infrastructure_bpr, string gestion_prevu_bpr, string gestion_final_bpr, string ecart_gestion_bpr, string autre_nom_bpr,
                                                string autre_prevu_bpr, string autre_final_bpr, string ecart_autre_bpr, string eco_energie_prevu, string eco_energie_reel, string eco_monetaire_prevu, string eco_monetaire_reel,
                                                string pri_prevu, string pri_reel, string conso_prevu, string conso_reel, string mesurage_eco, string appui_financier_organisme, string appui_financier_organisme2,
                                                string appui_financier_organisme3, string appui_financier_prevu, string appui_financier_prevu2, string appui_financier_prevu3, string appui_financier_reel, string appui_financier_reel2,
                                                string appui_financier_reel3, string mois_debut_fin, string annee_debut_fin, string mois_fin_fin, string annee_fin_fin, string montant_equipe_fin, string ecart_montant_equipe,
                                                string montant_societe_fin, string ecart_montant_societe, string TLinxProjectNumber, string ProjectURLSharepoint, string imgForFiche)
        {
            var result = _BoomerangService.CreateUpdateNewFiche(code_affaire, id_activite, localisation, id_province, autre_lieu, id_pays, intitule, intitule_court, description,
                                                                mois_debut, annee_debut, mois_fin, annee_fin, duree, id_type_unite_1, id_type_unite_2, montant_equipe,
                                                                montant_societe, client_nom, client_adresse, client_telephone, client_mail, contact_nom, num_ref, architecte,
                                                                conducteur_operation, equipe, equipe_interne, id_charge_affaire, proprio, plan_prevu_mois_debut, plan_prevu_annee_debut,
                                                                plan_prevu_mois_fin, plan_prevu_annee_fin, plan_final_mois_debut, plan_final_annee_debut, plan_final_mois_fin, plan_final_annee_fin,
                                                                construction_prevue_mois_debut, construction_prevue_annee_debut, construction_prevue_mois_fin, construction_prevue_annee_fin,
                                                                construction_finale_mois_debut, construction_finale_annee_debut, construction_finale_mois_fin, construction_finale_annee_fin,
                                                                ecart_periode_real, montant_estimation, montant_soumission, montant_final, ecart_cout, changement_bpr, montant_changement_bpr,
                                                                nature_changement_bpr, changement_client, montant_changement_client, nature_changement_client, structure_prevu, structure_final,
                                                                ecart_structure, amenagement_prevu, amenagement_final, ecart_amenagement, mecanique_prevu, mecanique_final,
                                                                ecart_mecanique, electricite_prevu, electricite_final, ecart_electricite, infrastructure_prevu, infrastructure_final, ecart_infrastructure,
                                                                gestion_prevu, gestion_final, ecart_gestion, autre_nom, autre_prevu, autre_final, ecart_autre, structure_prevu_bpr,
                                                                structure_final_bpr, ecart_structure_bpr, amenagement_prevu_bpr, amenagement_final_bpr, ecart_amenagement_bpr, mecanique_prevu_bpr,
                                                                mecanique_final_bpr, ecart_mecanique_bpr, electricite_prevu_bpr, electricite_final_bpr, ecart_electricite_bpr, infrastructure_prevu_bpr,
                                                                infrastructure_final_bpr, ecart_infrastructure_bpr, gestion_prevu_bpr, gestion_final_bpr, ecart_gestion_bpr, autre_nom_bpr,
                                                                autre_prevu_bpr, autre_final_bpr, ecart_autre_bpr, eco_energie_prevu, eco_energie_reel, eco_monetaire_prevu, eco_monetaire_reel,
                                                                pri_prevu, pri_reel, conso_prevu, conso_reel, mesurage_eco, appui_financier_organisme, appui_financier_organisme2,
                                                                appui_financier_organisme3, appui_financier_prevu, appui_financier_prevu2, appui_financier_prevu3, appui_financier_reel, appui_financier_reel2,
                                                                appui_financier_reel3, mois_debut_fin, annee_debut_fin, mois_fin_fin, annee_fin_fin, montant_equipe_fin, ecart_montant_equipe,
                                                                montant_societe_fin, ecart_montant_societe, TLinxProjectNumber, ProjectURLSharepoint, imgForFiche);

            if (result == true)
            {
                try
                {

                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result = "failed", details1 = e.Message, details2 = e.InnerException }, JsonRequestBehavior.AllowGet);

                }
            }
            else return Json(new { result = "failed" }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult SaveImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Chemin du répertoire de destination
                string destinationDirectory = @"C:\Users\Khounata.Fahmi\Desktop\Boomerang\Images";

                // Chemin complet de destination de l'image
                string destinationPath = Path.Combine(destinationDirectory, fileName);

                // Récupérer l'objet HttpPostedFileBase correspondant à l'image envoyée

                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase imageFile = Request.Files[0];
                    // Le reste de votre code de sauvegarde de l'image ici
                    imageFile.SaveAs(destinationPath);

                    // L'image a été sauvegardée avec succès
                    return Json(new { success = true });
                }
                else
                {
                    // Gérer le cas où aucun fichier n'a été envoyé
                    return Json(new { success = false, error = "Aucun fichier n'a été envoyé." });
                }              
            }
            //// Aucun nom de fichier n'a été fourni ou une erreur s'est produite lors de la récupération de l'image
            return Json(new { success = false });
        }



    }
}
