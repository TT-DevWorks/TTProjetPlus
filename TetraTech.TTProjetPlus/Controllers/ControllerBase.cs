using System;
using System.Web;
using System.Web.Mvc;
using TetraTech.TTProjetPlus.Models;
using TetraTech.TTProjetPlus.Security;
using TetraTech.TTProjetPlus.Services;

namespace TetraTech.TTProjetPlus.Controllers
{
    public abstract class ControllerBase : Controller
    {
        //getters
        public TetraTechUser CurrentUser => UserService.CurrentUser;

        public BaseViewModel CurrentBaseViewModel => HelperService.GetBaseViewModel(this);
    }
}