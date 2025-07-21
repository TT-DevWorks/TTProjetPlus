using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus.Models
{ 
    public enum TypesEnum { ProjectToClose, Other, AccessDenied, ReadOnly, NotFound, NotFoundTlx, NA }
    
    public class BaseViewModel
    {
        public string CurrentUserNo { get; set; }

        public string CurrentUserFirstName { get; set; }

        public DateTime CurrentPeriodWeekDateTime { get; set; }

        public string Url { get; set; } 

        public string CurrentPeriodWeekLongDate
        {
                get   { CultureInfo culture = new CultureInfo("fr-CA", true); return CurrentPeriodWeekDateTime.ToString("dd MMM yyyy", culture); }
        }


        #region "constructors"
        public BaseViewModel() { }

        public BaseViewModel(BaseViewModel _base)
        {
            if (_base == null) return;

            CurrentUserNo = _base.CurrentUserNo;
            CurrentUserFirstName = _base.CurrentUserFirstName;
            CurrentPeriodWeekDateTime = _base.CurrentPeriodWeekDateTime;
            Url = _base.Url;
        }
        #endregion
    }
}