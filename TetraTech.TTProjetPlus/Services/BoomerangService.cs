using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus.Services
{
    public class BoomerangService
    {
        private readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();

        public List<panier> returnListePanier(string nom, string prenom)
        {
            //a decommenter
            // var id = _entities.utilisateurs.Where(p=>p.nom == nom && p.prenom==prenom).Select(p=>p.id_utilisateur).FirstOrDefault();
            return _entities.paniers.Where(p => p.id_utilisateur == 2022).ToList();
        }
        public List<usp_Boomerang_getExpertises_Result> returnSearchCriterias()
        {
            var liste = _entities.usp_Boomerang_getExpertises().ToList();
            return liste;
        }

        public List<string> returnListProjects()
        {
            var liste = _entities.fiches.OrderBy(p => p.code_affaire).Where(p => p.code_affaire != null).Select(p => p.code_affaire).Distinct().ToList();
            return liste;
        }
        public List<string> returnListBusinessPlaces()
        {
            List<string> liste = new List<string>();
            liste = _entities.agences.OrderBy(p => p.libelle).Select(p => p.libelle + "#" + p.id_agence).Distinct().ToList();
            return liste;
        }


        public List<string> returnTypeUnities()
        {
            List<string> liste = new List<string>();
            liste = _entities.type_unite.OrderBy(p => p.unite).Select(p => p.unite + "#" + p.id_type_unite).Distinct().ToList();
            return liste;
        }

        public List<string> returnCustomers()
        {
            List<string> liste = new List<string>();
            liste = _entities.usp_Boomerang_getCustomers().OrderBy(p => p.client_nom).Select(p => p.client_nom).Distinct().ToList();
            return liste;
        }

        public List<string> returnDisciplines()
        {
            List<string> liste = new List<string>();
            liste = _entities.usp_Boomerang_getDiscplines().OrderBy(p => p.mission).Select(p => p.mission).Distinct().ToList();
            return liste;
        }


        public List<string> returnFicheTableFields()
        {
            List<string> liste = new List<string>();
            liste = _entities.usp_Boomerang_getFicheTableColumnsName().Select(p => p.COLUMN_NAME).ToList();
            return liste;
        }

        public decimal getUtilisateurId(string word)
        {
            return _entities.utilisateurs.Where(p => p.nom.Contains(word) || p.prenom.Contains(word)).Select(p => p.id_utilisateur).FirstOrDefault();
        }

        public string queryResult(string query)
        {
            var result = _entities.usp_Boomerang_executeQueryFromCode(query).Select(p => p.numrow).FirstOrDefault().ToString();
            return result;
        }

        public List<researchResult> ExecuteStoredProcedure(string query, string objectType)
        {
            try
            {
            DataSet ds = new DataSet();
            List<researchResult> results = new List<researchResult>();
            List<fiche> listeFiches = new List<fiche>();
            var con = new SqlConnection("data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework");

            var com = new SqlCommand();
            com.Connection = con;
            //com.CommandTimeout = 360;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "usp_Boomerang_getSearchResults";
            com.Parameters.Add("@query", SqlDbType.NVarChar).Value = query;
            var adapt = new SqlDataAdapter();
            adapt.SelectCommand = com;
            adapt.Fill(ds);


            con.Close();
            com.Dispose();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (objectType == "researchResult")
                    {
                        researchResult record = new researchResult();
                        record.id_fiche = row["id_fiche"].ToString().TrimEnd().TrimStart();
                        record.localisation = row["localisation"].ToString().TrimEnd().TrimStart();
                        record.endYear = row["annee_fin"].ToString().TrimEnd().TrimStart();
                        record.intitutle_court = row["intitule_court"].ToString().TrimEnd().TrimStart();
                        record.status = row["etat"].ToString().TrimEnd().TrimStart();
                        record.nom_award = row["nom_award"].ToString().TrimEnd().TrimStart();
                        record.lastmod_opqibi = row["lastmod_opqibi"].ToString().TrimEnd().TrimStart();
                        try
                        {
                            record.existInEn = row["existInEn"].ToString().TrimEnd().TrimStart();
                        }
                        catch
                        {
                            record.existInEn = "";
                        }

                        try
                        {
                            record.existInEsp = row["existInEsp"].ToString().TrimEnd().TrimStart();
                        }
                        catch
                        {
                            record.existInEsp = "";
                        }

                        try
                        {

                            record.hasArgumentaire = row["argComExist"].ToString().TrimEnd().TrimStart();
                        }
                        catch
                        {
                            record.hasArgumentaire = "";
                        }

                        string path = @"C:\xampp\htdocs\site\Boomerang\files\phototheque\" + row["id_fiche"].ToString().TrimEnd().TrimStart();
                        //string path = @"D:\Sites\Boomerang\files\phototheque\" + row["id_fiche"].ToString().TrimEnd().TrimStart();

                        if (Directory.Exists(path))
                        {
                            try
                            {
                                record.phototheque = "ok";
                            }
                            catch
                            {
                                record.phototheque = "";
                            }

                        }


                        results.Add(record);

                    }
                    else if (objectType == "ficheLang")
                    {

                    }
                    else if (objectType == "fiche")
                    {

                    }

                }
            }



            return results;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        //test
        public DataTable GetDataTableFast(string query)
        {


            var con = new SqlConnection("data source=tts349test02;initial catalog=TTProjetPlus;persist security info=True;user id=BPRProjetUser;password=BPRProjetUser;MultipleActiveResultSets=True;App=EntityFramework");
            var com = new SqlCommand();
            con.Open();
            com.Connection = con;
            //com.CommandTimeout = 360;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "usp_Boomerang_getSearchResults";
            com.Parameters.Add("@query", SqlDbType.NVarChar).Value = query;
            IDataReader rdr = com.ExecuteReader();
            DataTable resultTable = GetDataTableFromDataReader(rdr);
            con.Close();
            com.Dispose();
            return resultTable;
        }

        public DataTable GetDataTableFromDataReader(IDataReader dataReader)
        {
            DataTable schemaTable = dataReader.GetSchemaTable();
            DataTable resultTable = new DataTable();

            foreach (DataRow dataRow in schemaTable.Rows)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = dataRow["ColumnName"].ToString();
                dataColumn.DataType = Type.GetType(dataRow["DataType"].ToString());
                dataColumn.ReadOnly = (bool)dataRow["IsReadOnly"];
                dataColumn.AutoIncrement = (bool)dataRow["IsAutoIncrement"];
                dataColumn.Unique = (bool)dataRow["IsUnique"];

                resultTable.Columns.Add(dataColumn);
            }

            while (dataReader.Read())
            {
                DataRow dataRow = resultTable.NewRow();
                for (int i = 0; i < resultTable.Columns.Count; i++)
                {
                    dataRow[i] = dataReader[i];
                }
                resultTable.Rows.Add(dataRow);
            }

            return resultTable;
        }

        public usp_Boomerang_getFicheInFrenchFromId_Result getFicheInFrenchFromId(int idFiche)
        {
            var result = _entities.usp_Boomerang_getFicheInFrenchFromId(idFiche).FirstOrDefault();
            return result;
        }

        public Boomerang_tempFicheLang getFicheLang(string langSession, string sForcart, string idFiche, string andWhere)
        {
            _entities.usp_Boomerang_getFicheLang(langSession, sForcart, idFiche, andWhere);
            var result = _entities.Boomerang_tempFicheLang.Where(p => p.id_fiche.ToString() == idFiche).FirstOrDefault();
            return result;
        }

        public string GetImgForFiche(int id)
        {
            var result = _entities.fiches.Where(x => x.id_fiche == id).Select(x => x.imgForFiche).FirstOrDefault();
            return (result == null || result == "") ? "" : result;
        }

        public string GetAllLibelleTypeMission(int idFiche, string lang)
        {
            try
            {
                var id_type_mission = _entities.fiches.Where(x => x.id_fiche == idFiche).Select(x => x.id_type_mission).FirstOrDefault();
                List<int> list_id_type_mission = new List<int>();
                List<int?> list_order_type_mission = new List<int?>();
                var arrayIdTypeMission = id_type_mission.Split(',');
                string missionLibelle = "";
                foreach (string item in arrayIdTypeMission)
                {
                    if (item != "")
                    {
                        list_id_type_mission.Add(Int32.Parse(item));
                    }
                }
                foreach (int item in list_id_type_mission)
                {

                    list_order_type_mission.Add(_entities.type_mission.Where(x => x.id_type_mission == item).Select(x => x.order).FirstOrDefault());
                }
                foreach (int? item in list_order_type_mission.OrderBy(x => x))
                {

                    if (lang == "fr")
                    {
                        missionLibelle += " - " + _entities.type_mission.Where(x => x.order == item).Select(x => x.libelle).FirstOrDefault();
                    }
                    else if (lang == "en")
                    {
                        missionLibelle += " - " + _entities.type_mission.Where(x => x.order == item).Select(x => x.libelle_en).FirstOrDefault();
                    }

                    else if (lang == "esp")
                    {
                        missionLibelle += " - " + _entities.type_mission.Where(x => x.order == item).Select(x => x.libelle_esp).FirstOrDefault();
                    }

                }

                int index = missionLibelle.IndexOf("-");
                string cleanMissionLibelle = (index < 0) ? missionLibelle : missionLibelle.Remove(index, "-".Length);
                return cleanMissionLibelle;

            }
            catch (Exception e)
            {
                return "";
            }
        }

        public List<province> returnProvinces()
        {
            var liste = _entities.provinces.OrderBy(x => x.type).ToList();
            return liste;
        }

        public List<pay> returnPays(string lang)
        {
            var liste = new List<pay>();
            if (lang == "fr" || lang == "")
            {
                liste = _entities.pays.OrderBy(x => x.libelle).ToList();
            }
            else if (lang == "en")
            {
                liste = _entities.pays.OrderBy(x => x.libelle_en).ToList();
            }
            else if (lang == "esp")
            {
                liste = _entities.pays.OrderBy(x => x.libelle_esp).ToList();
            }
            return liste;
        }
        public List<type_mission> returnTypeMission(string lang)
        {
            var liste = new List<type_mission>();
            if (lang == "fr" || lang == "")
            {
                liste = _entities.type_mission.OrderBy(x => x.libelle).ToList();
            }
            else if (lang == "en")
            {
                liste = _entities.type_mission.OrderBy(x => x.libelle_en).ToList();
            }
            else if (lang == "esp")
            {
                liste = _entities.type_mission.OrderBy(x => x.libelle_esp).ToList();
            }
            return liste;
        }

        public List<type_unite> returnTypeUnits()
        {
            var liste = _entities.type_unite.OrderBy(x => x.categorie).ToList();
            return liste;
        }

        public List<agence> returnAgences()
        {
            var liste = _entities.agences.OrderBy(x => x.libelle).ToList();
            return liste;
        }

        public string returnImageName(int id_fiche)
        {
            var image = _entities.fiches.Where(x => x.id_fiche == id_fiche).Select(x => x.imgForFiche).FirstOrDefault();
            return image;
        }

        public decimal returnIdUtilisateur(string utilisateur)
        {
            var id = _entities.utilisateurs.Where(p => p.login == utilisateur).Select(p => p.id_utilisateur).FirstOrDefault();
            return id;
        }


        public List<fiche> returnListeFiche(int id_panier)
        {
            var listeIdFiches = _entities.panier_fiche.Where(p => p.id_panier == id_panier).Select(p => p.id_fiche).ToList();
            var listeFiche = _entities.fiches.Where(p => listeIdFiches.Contains(p.id_fiche)).ToList();
            return listeFiche;
        }

        public List<usp_Boomerang_getDraftsForUser_Result> returnListeDrafts(int utilisateur)
        {
            var liste = _entities.usp_Boomerang_getDraftsForUser(utilisateur).OrderBy(p=>p.localisation).ToList();
            return liste;
        }

        public List<usp_Boomerang_getDroitByUA_Result> returnRespList()
        {
            var liste = _entities.usp_Boomerang_getDroitByUA().OrderBy(p=>p.nomprenom).ToList();
            return liste;
        }

        public List<utilisateur> returnUtilisateurListe()
        {
            return _entities.utilisateurs.OrderBy(p => p.nom).ToList();
        }


        public bool InsertIntoFicheTable(string CodeAffaire, string Activite, string Localisation, string Province, string Pays, string AutreLieu,
                                          string Intitule, string IntituleCourt, string Description, string TLinxProjectNumber, string ProjectURLSharepoint,
                                          string ClientNom, string ClientAdresse, string ClientTelephone, string ContactNom, string ClientMail, string numRef)
        {
            try
            {
                fiche modelfiche = new fiche();

                var idFiche = _entities.fiches.Select(p => p.id_fiche).Max();

                modelfiche.id_fiche = _entities.fiches.Select(p => p.id_fiche).Max() + 1;
                modelfiche.code_affaire = CodeAffaire;
                modelfiche.id_activite  = int.Parse(Activite);
                modelfiche.localisation = Localisation;
                modelfiche.id_province = Province;
                modelfiche.id_pays = Pays;
                modelfiche.autre_lieu = AutreLieu;
                modelfiche.intitule = Intitule;
                modelfiche.intitule_court  = IntituleCourt;
                modelfiche.description  = Description;
                modelfiche.TLinxProjectNumber  = TLinxProjectNumber;
                modelfiche.ProjectURLSharepoint  = ProjectURLSharepoint;
                modelfiche.client_nom  = ClientNom;
                modelfiche.client_adresse  = ClientAdresse;
                modelfiche.client_telephone  = ClientTelephone;
                modelfiche.contact_nom  = ContactNom;
                modelfiche.client_mail  = ClientMail;
                modelfiche.num_ref  = numRef;

                _entities.fiches.Add(modelfiche);
                _entities.SaveChanges();
                                

                return true;
            }

            catch (Exception e)
            {

                return false;
            }
        }

     
        public int returnIdFiche(string code_affaire)
        {
            var list = _entities.fiches.Where(p => p.code_affaire == code_affaire).Select(p => p.id_fiche).FirstOrDefault();
            return list;
        }


        public bool CreateUpdateNewFiche(string code_affaire, string id_activite, string localisation, string id_province, string autre_lieu, string id_pays, string intitule, string intitule_court, string description,
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
            var db = _entities;            

            try
            {                
                var con = new SqlConnection(db.Database.Connection.ConnectionString);
                DataSet ds = new System.Data.DataSet();

                SqlParameter parameter = new SqlParameter();
                var cmd = new SqlCommand("[dbo].[usp_Boomerang_getCreateFiche]", con);
                cmd.CommandType = CommandType.StoredProcedure;

              
                parameter = new SqlParameter();
                parameter.ParameterName = "@code_affaire";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(code_affaire) || string.IsNullOrWhiteSpace(code_affaire) ? (object)DBNull.Value : code_affaire;
                cmd.Parameters.Add(parameter);
                               

                parameter = new SqlParameter();
                parameter.ParameterName = "@id_activite";
                parameter.SqlDbType = SqlDbType.NVarChar;                
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = id_activite;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@localisation";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(localisation) || string.IsNullOrWhiteSpace(localisation) ? (object)DBNull.Value : localisation;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@id_province";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(id_province) || string.IsNullOrWhiteSpace(id_province) ? (object)DBNull.Value : id_province;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_lieu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(autre_lieu) || string.IsNullOrWhiteSpace(autre_lieu) ? (object)DBNull.Value : autre_lieu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@id_pays";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(id_pays) || string.IsNullOrWhiteSpace(id_pays) ? (object)DBNull.Value : id_pays;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@intitule";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(intitule) || string.IsNullOrWhiteSpace(intitule) ? (object)DBNull.Value : intitule;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@intitule_court";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(intitule_court) || string.IsNullOrWhiteSpace(intitule_court) ? (object)DBNull.Value : intitule_court;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@description";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mois_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mois_debut == "00-0" || string.IsNullOrEmpty(mois_debut) || string.IsNullOrWhiteSpace(mois_debut) ? "0" : mois_debut.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@annee_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = annee_debut == "00-0" || string.IsNullOrEmpty(annee_debut) || string.IsNullOrWhiteSpace(annee_debut) ? "0" : annee_debut.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mois_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mois_fin == "00-0" || string.IsNullOrEmpty(mois_fin) || string.IsNullOrWhiteSpace(mois_fin) ? "0" : mois_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@annee_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mois_fin == "00-0" || string.IsNullOrEmpty(annee_fin) || string.IsNullOrWhiteSpace(annee_fin) ? "0" : annee_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@duree";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(duree) || string.IsNullOrWhiteSpace(duree) ? "0" : duree;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@id_type_unite_1";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(id_type_unite_1) || string.IsNullOrWhiteSpace(id_type_unite_1) ? "0" : id_type_unite_1;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@id_type_unite_2";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(id_type_unite_2) || string.IsNullOrWhiteSpace(id_type_unite_2) ? "0" : id_type_unite_2;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_equipe";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_equipe == "0,00" || string.IsNullOrEmpty(montant_equipe) || string.IsNullOrWhiteSpace(montant_equipe) ? "0.00" : montant_equipe;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_societe";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_societe == "0,00" || string.IsNullOrEmpty(montant_societe) || string.IsNullOrWhiteSpace(montant_societe) ? "0.00" : montant_societe;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@client_nom";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(client_nom) || string.IsNullOrWhiteSpace(client_nom) ? (object)DBNull.Value : client_nom;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@client_adresse";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(client_adresse) || string.IsNullOrWhiteSpace(client_adresse) ? (object)DBNull.Value : client_adresse;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@client_telephone";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(client_telephone) || string.IsNullOrWhiteSpace(client_telephone) ? (object)DBNull.Value : client_telephone;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@client_mail";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(client_mail) || string.IsNullOrWhiteSpace(client_mail) ? (object)DBNull.Value : client_mail;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@contact_nom";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(contact_nom) || string.IsNullOrWhiteSpace(contact_nom) ? (object)DBNull.Value : contact_nom;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@num_ref";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(num_ref) || string.IsNullOrWhiteSpace(num_ref) ? (object)DBNull.Value : num_ref;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@architecte";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(architecte) || string.IsNullOrWhiteSpace(architecte) ? (object)DBNull.Value : architecte;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@conducteur_operation";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(conducteur_operation) || string.IsNullOrWhiteSpace(conducteur_operation) ? (object)DBNull.Value : conducteur_operation;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@equipe";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(equipe) || string.IsNullOrWhiteSpace(equipe) ? (object)DBNull.Value : equipe;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@equipe_interne";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(equipe_interne) || string.IsNullOrWhiteSpace(equipe_interne) ? (object)DBNull.Value : equipe_interne;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@id_charge_affaire";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(id_charge_affaire) || string.IsNullOrWhiteSpace(id_charge_affaire) ? (object)DBNull.Value : id_charge_affaire;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@proprio";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(proprio) || string.IsNullOrWhiteSpace(proprio) ? (object)DBNull.Value : proprio;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_prevu_mois_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_prevu_mois_debut == "00-0" || string.IsNullOrEmpty(plan_prevu_mois_debut) || string.IsNullOrWhiteSpace(plan_prevu_mois_debut) ? "0" : plan_prevu_mois_debut.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_prevu_annee_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_prevu_annee_debut == "00-0" || string.IsNullOrEmpty(plan_prevu_annee_debut) || string.IsNullOrWhiteSpace(plan_prevu_annee_debut) ? "0" : plan_prevu_annee_debut.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_prevu_mois_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_prevu_mois_fin == "00-0" || string.IsNullOrEmpty(plan_prevu_mois_fin) || string.IsNullOrWhiteSpace(plan_prevu_mois_fin) ? "0" : plan_prevu_mois_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_prevu_annee_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_prevu_annee_fin == "00-0" || string.IsNullOrEmpty(plan_prevu_annee_fin) || string.IsNullOrWhiteSpace(plan_prevu_annee_fin) ? "0" : plan_prevu_annee_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_final_mois_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_prevu_annee_fin == "00-0" || string.IsNullOrEmpty(plan_final_mois_debut) || string.IsNullOrWhiteSpace(plan_final_mois_debut) ? "0" : plan_final_mois_debut.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_final_annee_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_final_annee_debut == "00-0" || string.IsNullOrEmpty(plan_final_annee_debut) || string.IsNullOrWhiteSpace(plan_final_annee_debut) ? "0" : plan_final_annee_debut.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_final_mois_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_final_mois_fin == "00-0" || string.IsNullOrEmpty(plan_final_mois_fin) || string.IsNullOrWhiteSpace(plan_final_mois_fin) ? "0" : plan_final_mois_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@plan_final_annee_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = plan_final_annee_fin == "00-0" || string.IsNullOrEmpty(plan_final_annee_fin) || string.IsNullOrWhiteSpace(plan_final_annee_fin) ? "0" : plan_final_annee_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_prevue_mois_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_prevue_mois_debut == "00-0" || string.IsNullOrEmpty(construction_prevue_mois_debut) || string.IsNullOrWhiteSpace(construction_prevue_mois_debut) ? "0" : construction_prevue_mois_debut.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_prevue_annee_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_prevue_annee_debut == "00-0" || string.IsNullOrEmpty(construction_prevue_annee_debut) || string.IsNullOrWhiteSpace(construction_prevue_annee_debut) ? "0" : construction_prevue_annee_debut.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_prevue_mois_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_prevue_mois_fin == "00-0" || string.IsNullOrEmpty(construction_prevue_mois_fin) || string.IsNullOrWhiteSpace(construction_prevue_mois_fin) ? "0" : construction_prevue_mois_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_prevue_annee_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_prevue_annee_fin == "00-0" || string.IsNullOrEmpty(construction_prevue_annee_fin) || string.IsNullOrWhiteSpace(construction_prevue_annee_fin) ? "0" : construction_prevue_annee_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_finale_mois_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_finale_mois_debut == "00-0" || string.IsNullOrEmpty(construction_finale_mois_debut) || string.IsNullOrWhiteSpace(construction_finale_mois_debut) ? "0" : construction_finale_mois_debut.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_finale_annee_debut";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_finale_annee_debut == "00-0" || string.IsNullOrEmpty(construction_finale_annee_debut) || string.IsNullOrWhiteSpace(construction_finale_annee_debut) ? "0" : construction_finale_annee_debut.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_finale_mois_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_finale_mois_fin == "00-0" || string.IsNullOrEmpty(construction_finale_mois_fin) || string.IsNullOrWhiteSpace(construction_finale_mois_fin) ? "0" : construction_finale_mois_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@construction_finale_annee_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = construction_finale_annee_fin == "00-0" || string.IsNullOrEmpty(construction_finale_annee_fin) || string.IsNullOrWhiteSpace(construction_finale_annee_fin) ? "0" : construction_finale_annee_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_periode_real";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_periode_real) || string.IsNullOrWhiteSpace(ecart_periode_real) ? (object)DBNull.Value : ecart_periode_real;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_estimation";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_estimation == "0,00" || string.IsNullOrEmpty(montant_estimation) || string.IsNullOrWhiteSpace(montant_estimation) ? "0.00" : montant_estimation;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_soumission";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_soumission == "0,00" || string.IsNullOrEmpty(montant_soumission) || string.IsNullOrWhiteSpace(montant_soumission) ? "0.00" : montant_soumission;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_final == "0,00" || string.IsNullOrEmpty(montant_final) || string.IsNullOrWhiteSpace(montant_final) ? "0.00" : montant_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_cout";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Value = string.IsNullOrEmpty(ecart_cout) || string.IsNullOrWhiteSpace(ecart_cout) ? (object)DBNull.Value : ecart_cout;
                parameter.Value = ecart_cout;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@changement_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = changement_bpr == "0,00" || string.IsNullOrEmpty(changement_bpr) || string.IsNullOrWhiteSpace(changement_bpr) ? "0.00" : changement_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_changement_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_changement_bpr == "0,00" || string.IsNullOrEmpty(montant_changement_bpr) || string.IsNullOrWhiteSpace(montant_changement_bpr) ? "0.00" : montant_changement_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@nature_changement_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(nature_changement_bpr) || string.IsNullOrWhiteSpace(nature_changement_bpr) ? (object)DBNull.Value : nature_changement_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@changement_client";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = changement_client == "00-0" || string.IsNullOrEmpty(changement_client) || string.IsNullOrWhiteSpace(changement_client) ? "0" : changement_client;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_changement_client";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_changement_client == "0,00" || string.IsNullOrEmpty(montant_changement_client) || string.IsNullOrWhiteSpace(montant_changement_client) ? "0.00" : montant_changement_client;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@nature_changement_client";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(nature_changement_client) || string.IsNullOrWhiteSpace(nature_changement_client) ? (object)DBNull.Value : nature_changement_client;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@structure_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = structure_prevu == "0,00" || string.IsNullOrEmpty(structure_prevu) || string.IsNullOrWhiteSpace(structure_prevu) ? "0.00" : structure_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@structure_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = structure_final == "0,00" || string.IsNullOrEmpty(structure_final) || string.IsNullOrWhiteSpace(structure_final) ? "0.00" : structure_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_structure";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_structure) || string.IsNullOrWhiteSpace(ecart_structure) ? (object)DBNull.Value : ecart_structure;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@amenagement_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = amenagement_prevu == "0,00" || string.IsNullOrEmpty(amenagement_prevu) || string.IsNullOrWhiteSpace(amenagement_prevu) ? "0.00" : amenagement_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@amenagement_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = amenagement_final == "0,00" || string.IsNullOrEmpty(amenagement_final) || string.IsNullOrWhiteSpace(amenagement_final) ? "0.00" : amenagement_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_amenagement";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_amenagement) || string.IsNullOrWhiteSpace(ecart_amenagement) ? (object)DBNull.Value : ecart_amenagement;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mecanique_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mecanique_prevu == "0,00" || string.IsNullOrEmpty(mecanique_prevu) || string.IsNullOrWhiteSpace(mecanique_prevu) ? "0.00" : mecanique_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mecanique_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mecanique_final == "0,00" || string.IsNullOrEmpty(mecanique_final) || string.IsNullOrWhiteSpace(mecanique_final) ? "0.00" : mecanique_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_mecanique";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_mecanique) || string.IsNullOrWhiteSpace(ecart_mecanique) ? (object)DBNull.Value : ecart_mecanique;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@electricite_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = electricite_prevu == "0,00" || string.IsNullOrEmpty(electricite_prevu) || string.IsNullOrWhiteSpace(electricite_prevu) ? "0.00" : electricite_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@electricite_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = electricite_final == "0,00" || string.IsNullOrEmpty(electricite_final) || string.IsNullOrWhiteSpace(electricite_final) ? "0.00" : electricite_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_electricite";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_electricite) || string.IsNullOrWhiteSpace(ecart_electricite) ? (object)DBNull.Value : ecart_electricite;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@infrastructure_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = infrastructure_prevu == "0,00" || string.IsNullOrEmpty(infrastructure_prevu) || string.IsNullOrWhiteSpace(infrastructure_prevu) ? "0.00" : infrastructure_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@infrastructure_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = infrastructure_final == "0,00" || string.IsNullOrEmpty(infrastructure_final) || string.IsNullOrWhiteSpace(infrastructure_final) ? "0.00" : infrastructure_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_infrastructure";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_infrastructure) || string.IsNullOrWhiteSpace(ecart_infrastructure) ? (object)DBNull.Value : ecart_infrastructure;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@gestion_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = gestion_prevu == "0,00" || string.IsNullOrEmpty(gestion_prevu) || string.IsNullOrWhiteSpace(gestion_prevu) ? "0.00" : gestion_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@gestion_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = gestion_final == "0,00" || string.IsNullOrEmpty(gestion_final) || string.IsNullOrWhiteSpace(gestion_final) ? "0.00" : gestion_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_gestion";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_gestion) || string.IsNullOrWhiteSpace(ecart_gestion) ? (object)DBNull.Value : ecart_gestion;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_nom";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(autre_nom) || string.IsNullOrWhiteSpace(autre_nom) ? (object)DBNull.Value : autre_nom;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = autre_prevu == "0,00" || string.IsNullOrEmpty(autre_prevu) || string.IsNullOrWhiteSpace(autre_prevu) ? "0.00" : autre_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_final";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = autre_final == "0,00" || string.IsNullOrEmpty(autre_final) || string.IsNullOrWhiteSpace(autre_final) ? "0.00" : autre_final;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_autre";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;                
                parameter.Value = string.IsNullOrEmpty(ecart_autre) || string.IsNullOrWhiteSpace(ecart_autre) ? (object)DBNull.Value : ecart_autre;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@structure_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = structure_prevu_bpr == "0,00" || string.IsNullOrEmpty(structure_prevu_bpr) || string.IsNullOrWhiteSpace(structure_prevu_bpr) ? "0.00" : structure_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@structure_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = structure_final_bpr == "0,00" || string.IsNullOrEmpty(structure_final_bpr) || string.IsNullOrWhiteSpace(structure_final_bpr) ? "0.00" : structure_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_structure_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_structure_bpr) || string.IsNullOrWhiteSpace(ecart_structure_bpr) ? (object)DBNull.Value : ecart_structure_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@amenagement_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = amenagement_prevu_bpr == "0,00" || string.IsNullOrEmpty(amenagement_prevu_bpr) || string.IsNullOrWhiteSpace(amenagement_prevu_bpr) ? "0.00" : amenagement_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@amenagement_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = amenagement_final_bpr == "0,00" || string.IsNullOrEmpty(amenagement_final_bpr) || string.IsNullOrWhiteSpace(amenagement_final_bpr) ? "0.00" : amenagement_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_amenagement_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;                
                parameter.Value = string.IsNullOrEmpty(ecart_amenagement_bpr) || string.IsNullOrWhiteSpace(ecart_amenagement_bpr) ? (object)DBNull.Value : ecart_amenagement_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mecanique_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mecanique_prevu_bpr == "0,00" || string.IsNullOrEmpty(mecanique_prevu_bpr) || string.IsNullOrWhiteSpace(mecanique_prevu_bpr) ? "0.00" : mecanique_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mecanique_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mecanique_final_bpr == "0,00" || string.IsNullOrEmpty(mecanique_final_bpr) || string.IsNullOrWhiteSpace(mecanique_final_bpr) ? "0.00" : mecanique_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_mecanique_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_mecanique_bpr) || string.IsNullOrWhiteSpace(ecart_mecanique_bpr) ? (object)DBNull.Value : ecart_mecanique_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@electricite_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = electricite_prevu_bpr == "0,00" || string.IsNullOrEmpty(electricite_prevu_bpr) || string.IsNullOrWhiteSpace(electricite_prevu_bpr) ? "0.00" : electricite_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@electricite_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = electricite_final_bpr == "0,00" || string.IsNullOrEmpty(electricite_final_bpr) || string.IsNullOrWhiteSpace(electricite_final_bpr) ? "0.00" : electricite_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_electricite_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_electricite_bpr) || string.IsNullOrWhiteSpace(ecart_electricite_bpr) ? (object)DBNull.Value : ecart_electricite_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@infrastructure_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = infrastructure_prevu_bpr == "0,00" || string.IsNullOrEmpty(infrastructure_prevu_bpr) || string.IsNullOrWhiteSpace(infrastructure_prevu_bpr) ? "0.00" : infrastructure_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@infrastructure_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = infrastructure_final_bpr == "0,00" || string.IsNullOrEmpty(infrastructure_final_bpr) || string.IsNullOrWhiteSpace(infrastructure_final_bpr) ? "0.00" : infrastructure_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_infrastructure_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_infrastructure_bpr) || string.IsNullOrWhiteSpace(ecart_infrastructure_bpr) ? (object)DBNull.Value : ecart_infrastructure_bpr;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@gestion_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = gestion_prevu_bpr == "0,00" || string.IsNullOrEmpty(gestion_prevu_bpr) || string.IsNullOrWhiteSpace(gestion_prevu_bpr) ? "0.00" : gestion_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@gestion_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = gestion_final_bpr == "0,00" || string.IsNullOrEmpty(gestion_final_bpr) || string.IsNullOrWhiteSpace(gestion_final_bpr) ? "0.00" : gestion_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_gestion_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_gestion_bpr) || string.IsNullOrWhiteSpace(ecart_gestion_bpr) ? (object)DBNull.Value : ecart_gestion_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_nom_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(autre_nom_bpr) || string.IsNullOrWhiteSpace(autre_nom_bpr) ? (object)DBNull.Value : autre_nom_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_prevu_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = autre_prevu_bpr == "0,00" || string.IsNullOrEmpty(autre_prevu_bpr) || string.IsNullOrWhiteSpace(autre_prevu_bpr) ? (object)DBNull.Value : autre_prevu_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@autre_final_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = autre_final_bpr == "0,00" || string.IsNullOrEmpty(autre_final_bpr) || string.IsNullOrWhiteSpace(autre_final_bpr) ? (object)DBNull.Value : autre_final_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_autre_bpr";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_autre_bpr) || string.IsNullOrWhiteSpace(ecart_autre_bpr) ? (object)DBNull.Value : ecart_autre_bpr;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@eco_energie_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = eco_energie_prevu == "0,00" || string.IsNullOrEmpty(eco_energie_prevu) || string.IsNullOrWhiteSpace(eco_energie_prevu) ? "0.00" : eco_energie_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@eco_energie_reel";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = eco_energie_reel == "0,00" || string.IsNullOrEmpty(eco_energie_reel) || string.IsNullOrWhiteSpace(eco_energie_reel) ? "0.00" : eco_energie_reel;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@eco_monetaire_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = eco_monetaire_prevu == "0,00" || string.IsNullOrEmpty(eco_monetaire_prevu) || string.IsNullOrWhiteSpace(eco_monetaire_prevu) ? "0.00" : eco_monetaire_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@eco_monetaire_reel";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = eco_monetaire_reel == "0,00" || string.IsNullOrEmpty(eco_monetaire_reel) || string.IsNullOrWhiteSpace(eco_monetaire_reel) ? "0.00" : eco_monetaire_reel;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@pri_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = pri_prevu == "0,00" || string.IsNullOrEmpty(pri_prevu) || string.IsNullOrWhiteSpace(pri_prevu) ? "0.00" : pri_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@pri_reel";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = pri_reel == "0,00" || string.IsNullOrEmpty(pri_reel) || string.IsNullOrWhiteSpace(pri_reel) ? "0.00" : pri_reel;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@conso_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = conso_prevu == "0,00" || string.IsNullOrEmpty(conso_prevu) || string.IsNullOrWhiteSpace(conso_prevu) ? "0.00" : conso_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@conso_reel";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = conso_reel == "0,00" || string.IsNullOrEmpty(conso_reel) || string.IsNullOrWhiteSpace(conso_reel) ? "0.00" : conso_reel;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mesurage_eco";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(mesurage_eco) || string.IsNullOrWhiteSpace(mesurage_eco) ? (object)DBNull.Value : mesurage_eco;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_organisme";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(appui_financier_organisme) || string.IsNullOrWhiteSpace(appui_financier_organisme) ? (object)DBNull.Value : appui_financier_organisme;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_organisme2";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(appui_financier_organisme2) || string.IsNullOrWhiteSpace(appui_financier_organisme2) ? (object)DBNull.Value : appui_financier_organisme2;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_organisme3";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(appui_financier_organisme3) || string.IsNullOrWhiteSpace(appui_financier_organisme3) ? (object)DBNull.Value : appui_financier_organisme3;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_prevu";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_prevu == "0,00" || string.IsNullOrEmpty(appui_financier_prevu) || string.IsNullOrWhiteSpace(appui_financier_prevu) ? "0.00" : appui_financier_prevu;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_prevu2";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_prevu2 == "0,00" || string.IsNullOrEmpty(appui_financier_prevu2) || string.IsNullOrWhiteSpace(appui_financier_prevu2) ? "0.00" : appui_financier_prevu2;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_prevu3";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_prevu3 == "0,00" || string.IsNullOrEmpty(appui_financier_prevu3) || string.IsNullOrWhiteSpace(appui_financier_prevu3) ? "0.00" : appui_financier_prevu3;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_reel";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_reel == "0,00" || string.IsNullOrEmpty(appui_financier_reel) || string.IsNullOrWhiteSpace(appui_financier_reel) ? "0.00" : appui_financier_reel;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_reel2";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_reel2 == "0,00" || string.IsNullOrEmpty(appui_financier_reel2) || string.IsNullOrWhiteSpace(appui_financier_reel2) ? "0.00" : appui_financier_reel2;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@appui_financier_reel3";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appui_financier_reel3 == "0,00" || string.IsNullOrEmpty(appui_financier_reel3) || string.IsNullOrWhiteSpace(appui_financier_reel3) ? "0.00" : appui_financier_reel3;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mois_debut_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mois_debut_fin == "0,00" || mois_debut_fin == "0" || string.IsNullOrEmpty(mois_debut_fin)  || string.IsNullOrWhiteSpace(mois_debut_fin) ? "0" : mois_debut_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@annee_debut_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = annee_debut_fin == "0,00" || annee_debut_fin == "0" || string.IsNullOrEmpty(annee_debut_fin) || string.IsNullOrWhiteSpace(annee_debut_fin) ? "0" : annee_debut_fin.Split('-')[1];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@mois_fin_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = mois_fin_fin == "00-0" || string.IsNullOrEmpty(mois_fin_fin) || string.IsNullOrWhiteSpace(mois_fin_fin) ? "0" : mois_fin_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@annee_fin_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = annee_fin_fin == "00-0" || string.IsNullOrEmpty(annee_fin_fin) || string.IsNullOrWhiteSpace(annee_fin_fin) ? "0" : annee_fin_fin.Split('-')[0];
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_equipe_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_equipe_fin == "0,00" || string.IsNullOrEmpty(montant_equipe_fin) || string.IsNullOrWhiteSpace(montant_equipe_fin) ? "0.00" : montant_equipe_fin;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_montant_equipe";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_montant_equipe) || string.IsNullOrWhiteSpace(ecart_montant_equipe) ? (object)DBNull.Value : ecart_montant_equipe;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@montant_societe_fin";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = montant_societe_fin == "0,00" || string.IsNullOrEmpty(montant_societe_fin) || string.IsNullOrWhiteSpace(montant_societe_fin) ? "0.00" : montant_societe_fin;
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.ParameterName = "@ecart_montant_societe";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ecart_montant_societe) || string.IsNullOrWhiteSpace(ecart_montant_societe) ? (object)DBNull.Value : ecart_montant_societe;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@TLinxProjectNumber";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(TLinxProjectNumber) || string.IsNullOrWhiteSpace(TLinxProjectNumber) ? (object)DBNull.Value : TLinxProjectNumber;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@ProjectURLSharepoint";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(ProjectURLSharepoint) || string.IsNullOrWhiteSpace(ProjectURLSharepoint) ? (object)DBNull.Value : ProjectURLSharepoint;
                cmd.Parameters.Add(parameter);
                
                parameter = new SqlParameter();
                parameter.ParameterName = "@imgForFiche";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = string.IsNullOrEmpty(imgForFiche) || string.IsNullOrWhiteSpace(imgForFiche) ? (object)DBNull.Value : imgForFiche.Split('.')[0];
                cmd.Parameters.Add(parameter);
                                               
                // en secondes donc 5 minutes
                cmd.CommandTimeout = 300;

                con.Open();
                cmd.ExecuteNonQuery();                
                con.Close();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
                      
        }


    }
}