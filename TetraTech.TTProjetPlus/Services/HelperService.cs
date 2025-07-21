using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq; 
using System.Web; 
using TetraTech.TTProjetPlus.Data;
using TetraTech.TTProjetPlus.Exceptions;
using TetraTech.TTProjetPlus.Models; 
using ControllerBase = TetraTech.TTProjetPlus.Controllers.ControllerBase;
 

namespace TetraTech.TTProjetPlus.Services
{
    public static class HelperService
    {
        private static readonly HomeService _HomeService = new HomeService();
        public static DateTime GetCurrentPeriodWeekDate()
        {
            using (var _entities = new SuiviMandatEntitiesNew2())
            {
                return _entities.XX_PROJECT_DATA.Max(pd => pd.Period_Week_Date);
            }
        } 

        public static BaseViewModel  GetBaseViewModel(ControllerBase controller)
        { 
            if (controller != null)
            { 
                string employeeid = controller.CurrentUser.EmployeeId;  

                var result = new BaseViewModel
                {
                    CurrentUserNo = employeeid,
                    CurrentUserFirstName = controller.CurrentUser.FirstName,
                    CurrentPeriodWeekDateTime = GetCurrentPeriodWeekDate(), 
                    Url = controller.HttpContext.Request.CurrentExecutionFilePath 
                }; 

                return result;
            }

            return new BaseViewModel();
        } 

        public static void CreateCommentFermeture_Projet(int projectId,  DateTime period, int version, string comment, string employeeNb, string employeeRole, string employeeName)
        {
            try
            {
            //un seul commentaire (instruction facturation) par projet; si existe déjà faire une exception; la version en paramètre devrait être 0
            if (string.IsNullOrEmpty(comment))
                return;

            SuiviMandatEntitiesNew2 _entitiesSuiviMandat = new SuiviMandatEntitiesNew2();

            string commentTypeString = "Fermeture_Projet";
            DateTime today = DateTime.Now.Date;
            var projectVersionComment = _entitiesSuiviMandat
                .PROJECT_COMMENT.Where(pr => pr.Project_id == projectId && pr.Version_ID == version && pr.CommentType == commentTypeString)
                .FirstOrDefault();

            var aComment = HttpUtility.HtmlDecode(comment.Trim());

            if (projectVersionComment == null)
            {
                var newId = _entitiesSuiviMandat.PROJECT_COMMENT.Max(x => x.Project_Comment_Id) + 1;
                var versionComment = new PROJECT_COMMENT
                {
                    Project_id = projectId,
                    Period_Week_Date = period,
                    Version_ID = version,
                    DateModification = DateTime.Now.Date,
                    LastUpdtBy = employeeNb,
                    Comment = aComment,
                    CommentType = commentTypeString,
                    LastUpdtByKMRole = employeeRole,
                    full_name = employeeName,
                    Active = true,
                    Project_Comment_Id = newId

                };
                _entitiesSuiviMandat.PROJECT_COMMENT.Add(versionComment);
            }
            else
            {
                throw new DuplicateItemException();
            }
            _entitiesSuiviMandat.SaveChanges();
            }
            catch(Exception e)
            {
                _HomeService.addErrorLog(e, "helperService", "CreateCommentFermeture_Projet" , projectId.ToString());
                throw e;
            }

        }

        public static int GetOSProjectIdFromTlxNumber(string tlxprojectnumber)
        {
            if (string.IsNullOrEmpty(tlxprojectnumber))
                return -1;  

            int i = tlxprojectnumber.IndexOf('-'); 
            string os_projectnumber = tlxprojectnumber.Substring(i + 1);
            string org = (i > 0)?  tlxprojectnumber.Substring(0, i) : "-";

            using (var _entities = new TTProjetPlusEntitiesNew())
            {
                return _entities.OS_Project.Where(p=>p.Company.StartsWith(org) && p.Project_Number==os_projectnumber).Select(p=>p.OS_Project_ID).FirstOrDefault();  
            }
        }
    }

    public static partial class HelperExtensions
    {  
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return defaultValue;
        }  
    } 
}