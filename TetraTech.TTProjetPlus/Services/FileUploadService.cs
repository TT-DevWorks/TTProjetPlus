using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Models;

namespace TetraTech.TTProjetPlus.Services
{
    public class FileUploadService
    {
        private readonly BPRProjetEntitiesNew _entitiesBPR = new BPRProjetEntitiesNew();
        private static readonly TTProjetPlusEntitiesNew _entities = new TTProjetPlusEntitiesNew();


        public List<string> returnFolderList ()
        {
            var list = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Select(p=>p.REP_Code).Distinct().ToList();
            return list;
        }

        public List<string> returnDocNameList()
        {
            List<string> finalList = new List<string>();
            //var folder = @"\\tts349test04\D$\TTProjetPlus\TQEDocs\";

            var folder = @"\\tts349iis\D$\TTProjetPlus\TQEDocs\";

            DirectoryInfo d = new DirectoryInfo(folder); //IMPORTANT: tts349iis pour prod

            FileInfo[] Files = d.GetFiles(); //Getting Text files
            foreach (FileInfo file in Files)
            {
                //  int idx = file.Name.LastIndexOf('\\');
                //  finalList.Add(item + "|" + item.Substring(idx + 1));
                finalList.Add(folder+ file.Name + "|" + file.Name);
            }
            
            return finalList.Distinct().ToList();
        }




        public List<RepModel> GetSubFolders(string repCode)
        {
            List<RepModel> finalList = new List<RepModel>();
            var list = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p=>p.REP_Code== repCode).Distinct().ToList();
            foreach (TQEDocsToInsertInSubForlders_Sly item in list)
            {
                if (item.SousRep_code != null && item.SousRep_id != null)
                {
                RepModel model = new RepModel();
                model.code = item.SousRep_code;
                model.id = item.SousRep_id;
                finalList.Add(model);
                }
               
            }
            return finalList.GroupBy(elem => elem.id).Select(group => group.First()).ToList();
        }


        public List<string> GetDocList(int? repId, int? sousRepId)
        {
            List<string> finalList = new List<string>();
            var list = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.REP_Id == repId && p.SousRep_id == sousRepId).Select(p=>p.NumeroDoc).ToList();
            foreach (string item in list)
            {
                if (item != null && item !="")
                {
                    var array = item.Split('\\');
                    var arrayCount = array.Count() - 1;
                    finalList.Add(array[arrayCount]);
                }

            }
            return finalList.Distinct().ToList();
        }


        public int? returnRepId(string repCode)
        {
            var repID = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.REP_Code == repCode).Select(p => p.REP_Id).FirstOrDefault();
            return repID;
        }


        public int? returnSousRepId(string repSubCode)
        {
            var sousRepID = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.SousRep_code == repSubCode).Select(p => p.SousRep_id).FirstOrDefault();
            return sousRepID;
        }

        public string returnSubRepCode(string subRepId)
        {
            var sub_RepId = Int32.Parse(subRepId);
            var subRepCode = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.SousRep_id == sub_RepId).Select(p => p.SousRep_code).FirstOrDefault();
            return subRepCode;
        }

        public string insertIntoTable(string repCode, int? rep_id, string subRepCode, int ? subRepId, string FileName, string CustomerName, string CustomerNumber)
        {   try
            {
                //existe il un element dans la table qui a les parametres recus?
            var item = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(
                p => p.REP_Code == repCode && p.REP_Id == rep_id && p.SousRep_code == subRepCode && p.SousRep_id == subRepId && p.NumeroDoc == FileName && p.Customer_Name == CustomerName && p.Customer_Number == CustomerNumber).FirstOrDefault();
              
                if (item != null)
                  {//il existe un tel element.on ne fait rien
                    return "successInsert";
                 }
            //il n y a aucun element qui correspond au parametre; on le crée integralement
                else
                {
                    //y a t il au moins un element qui a le meme rep_id et le meme subrep_id
                    var item2 = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(
                     p => p.REP_Code == repCode && p.REP_Id == rep_id  && p.SousRep_id == subRepId && p.NumeroDoc == null && p.Customer_Name == CustomerName && p.Customer_Number == CustomerNumber).FirstOrDefault();

                    if (item2 != null)// il y a bien un element mais avec un autre nom de numdoc (ou null); il faut alors ajouter un ligne dans la bd
                    {

                            item2.NumeroDoc = FileName;
                            item2.Customer_Name = CustomerName;
                            item2.Customer_Number = CustomerNumber;
                            _entitiesBPR.SaveChanges();
                            return "successInsert";

                    }
                    else //il faut creer une nouvelle ligne
                    {
                        TQEDocsToInsertInSubForlders_Sly model = new TQEDocsToInsertInSubForlders_Sly();
                        model.REP_Code = repCode;
                        model.REP_Id = rep_id;
                        model.SousRep_code = subRepCode;
                        model.SousRep_id = subRepId;
                        model.NumeroDoc = FileName;
                        model.Customer_Name = CustomerName;
                        model.Customer_Number = CustomerNumber;

                        _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Add(model);
                        _entitiesBPR.SaveChanges();
                        return "successInsert";

                    }

                }
            }

            catch (Exception e)
            {
                return "failedInsert";
            }
        }


        public string  removeDocFromDB (int ? repId, int ? sousRepId, string nomDoc, string customerNumber)
        {
            try
            {
                if (customerNumber == null || customerNumber == "")
                {
                    var list = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.REP_Id == repId && p.SousRep_id == sousRepId).ToList();
                    var item = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.REP_Id == repId && p.SousRep_id == sousRepId && p.NumeroDoc.Contains(nomDoc) && p.Customer_Number == "NULL").FirstOrDefault();

                    var countList = list.Count();
                    if (countList == 1)
                    {
                        item.NumeroDoc = null;
                        _entitiesBPR.SaveChanges();
                    }
                    else
                    {
                        _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Remove(item);
                        _entitiesBPR.SaveChanges();
                    }

                    return "successDelete";
                }
                else
                {
                    string[] itemsNumber = customerNumber.Split(',');

                    for (int i = 0; i < itemsNumber.Length; i++)  
                    {
                        var customer = itemsNumber[i];

                        var itemCustomer = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.REP_Id == repId && p.SousRep_id == sousRepId && p.NumeroDoc.Contains(nomDoc) && p.Customer_Number == customer).FirstOrDefault();
                      
                        _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Remove(itemCustomer);
                        _entitiesBPR.SaveChanges();
                    }

                    return "successDelete";
                }

              
            }
            catch (Exception e)
            {
                return "failedDelete";
            }
        }

        public string removeDocFromDB_Final(string nomDoc)
        {
            try
                
            {
                var filename = nomDoc.Split('\\')[6];
                var list = _entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Where(p => p.NumeroDoc.Contains(nomDoc)).ToList();

                foreach (TQEDocsToInsertInSubForlders_Sly item in list)
                {
                    //_entitiesBPR.TQEDocsToInsertInSubForlders_Sly.Remove(item);
                    item.NumeroDoc = null;
                    _entitiesBPR.SaveChanges();
                    if (File.Exists(nomDoc))
                    {
                        File.Delete(nomDoc);
                    }

                }
                if (list.Count == 0)
                {
                    if (File.Exists(nomDoc))
                    {
                        File.Delete(nomDoc);
                    }
                }
                _entitiesBPR.SaveChanges();
                return "successDelete";
            }
            catch (Exception e)
            {
                return "failedDelete";
            }
        }

        public static int? IsEmplopyeeAdmin(string currentUserId)
        {
            int? status = 0;
            try
            {
            status = _entities.TT_EmployeesPermissions.Where(p => p.employeeNumber.ToString() == currentUserId).Select(p => p.Role_Id).FirstOrDefault();
                return status;
            }
           catch(Exception e)
            {
                return 0;
            }

        }


        public List<structureItem> GetListCustomer()
        {
            var ListCustomers = from p in _entities.Customers.OrderBy(p => p.Customer_Name)
                                select new structureItem { Name = p.Customer_Name, Value = p.Customer_Number };
            return ListCustomers.ToList();
        }


    }
}