using System; 
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using TetraTech.TTProjetPlus.Data;

namespace TetraTech.TTProjetPlus.Security
{
    public static class UserService
    {
        private const string AuthenticatedUserKey = "AuthenticatedUser";
        private const string ImpersonatedUserKey = "ActiveUser";
        private const string ImpersonateCookieName = "impersonate";

        /// <summary>
        /// Gets the authenticated user.
        /// </summary>
        /// <remarks>
        /// The user object is cached on a per-request basis.
        /// </remarks>
        public static TetraTechUser AuthenticatedUser
        {
            get
            {
                var user = HttpContext.Current.Items[AuthenticatedUserKey] as TetraTechUser;

                if (user == null)
                {
                    var identity = HttpContext.Current.User.Identity as WindowsIdentity;

                    if (identity == null)
                        throw new Exception("The authention mode must be configured to 'Windows'.");

                    var directoryService = new DirectoryService();
                    var principal = directoryService.GetUser(identity);

                    user = new TetraTechUser(principal);
                    HttpContext.Current.Items[AuthenticatedUserKey] = user;
                }

                return user;
            }
        }

        /// <summary>
        /// Returns the current user.
        /// </summary>
        public static TetraTechUser CurrentUser
        {
            get
            {
                return ImpersonatedUser ?? AuthenticatedUser;
            }
        }

        /// <summary>
        /// Returns a boolean value indicating if the current user is impersonating.
        /// </summary>
        public static bool IsImpersonating
        {
            get
            {
                return ImpersonatedUser != null;
            }
        }

        /// <summary>
        /// Gets or sets the impersonated user.
        /// </summary>
        /// <remarks>
        /// The user object is cached on a per-request basis and its value persisted in a session cookie.
        /// </remarks>
        public static TetraTechUser ImpersonatedUser
        {
            get
            {
                var user = HttpContext.Current.Items[ImpersonatedUserKey] as TetraTechUser;

                if (user == null)
                {
                    var cookie = HttpContext.Current.Request.Cookies[ImpersonateCookieName];

                    if (cookie != null)
                    {
                        var directoryService = new DirectoryService();
                        var principal = directoryService.FindUserByEmployeeId(cookie.Value);

                        if (principal != null)
                        {
                            user = new TetraTechUser(principal);
                            HttpContext.Current.Items[ImpersonatedUserKey] = user;
                        }
                        else
                            HttpContext.Current.Response.Cookies.Remove(ImpersonateCookieName);
                    }
                }

                return user;
            }
            set
            {
                if (value != null)
                    HttpContext.Current.Response.SetCookie(new HttpCookie(ImpersonateCookieName, value.EmployeeId));
                else
                {
                    var cookie = HttpContext.Current.Request.Cookies[ImpersonateCookieName];

                    if (cookie != null)
                        cookie.Expires = DateTime.Now.AddDays(-1);

                    HttpContext.Current.Response.SetCookie(cookie);
                }

                HttpContext.Current.Items[ImpersonatedUserKey] = value;
            }
        }

        
        public static bool IsAdmin(TetraTechUser currentUser)
        {
            var value = ConfigurationManager.AppSettings["Admins"];
            return value != null && value.Split(',').Any(name => String.Compare(name.Trim(), currentUser.TTAccount, true) == 0);
        }


        public static int isPRODEnvironment()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusUrl"];
            int result = 1;

            if (value != null)
            {
                if (value.ToUpper() != "HTTP://TTPROJETPLUS.TETRATECH.COM")
                {
                    result = 0;
                }
            }

            return result;
        }

        public static int isTQEModelException()
        {
            var value = ConfigurationManager.AppSettings["TQEModelExceptions"];
            int result = 0;

            if (value != null && value.Split(',').Any(name => String.Compare(name.Trim(), CurrentUser.TTAccount, true) == 0))
            {
                result = 1;
            }
            return result;
        }


        public static string TTProjetPlusUserNasuni()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusUserNasuni"];
            return value;
        }

        public static string TTProjetPlusPwdNasuni()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusPwdNasuni"];
            return value;
        }

        public static string TTProjetPlusEnvrNasuni()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusEnvrNasuni"];
            return value;
        }

        public static string TTProjetPlusconnectionStringBPRProjetUser()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusconnectionStringBPRProjetUser"];
            return value;
        }

        public static string TTProjetPlusconnectionStringBPRProjetConf()
        {
            var value = ConfigurationManager.AppSettings["TTProjetPlusconnectionStringBPRProjetConf"];
            return value;
        }


        public static string LecteurReseau()
        {
            var value = ConfigurationManager.AppSettings["LecteurReseau"];
            return value;
        }

        public static string LecteurSpecialMontrealPrj_Usine_3D()
        {
            var value = ConfigurationManager.AppSettings["LecteurSpecialMontrealPrj_Usine_3D"];
            return value;
        }

        public static string LecteurSpecialQuebecTTIPrj_Usine_3D()
        {
            var value = ConfigurationManager.AppSettings["LecteurSpecialQuebecTTIPrj_Usine_3D"];
            return value;
        }
        
        public static string LecteurSpecialMontrealSuncor()
        {
            var value = ConfigurationManager.AppSettings["LecteurSpecialMontrealSuncor"];
            return value;
        }

        public static string LecteurSpecialJonquierePrj_Usine_3D()
        {
            var value = ConfigurationManager.AppSettings["LecteurSpecialJonquierePrj_Usine_3D"];
            return value;
        }

        public static string FichierQIBSatisfactionClient()
        {
            var value = ConfigurationManager.AppSettings["FichierQIBSatisfactionClient"];
            return value;
        }

        public static string FichierTQESatisfactionClient()
        {
            var value = ConfigurationManager.AppSettings["FichierTQESatisfactionClient"];
            return value;
        }

        public static string CourrielTestSatisfactionClient()
        {
            var value = ConfigurationManager.AppSettings["CourrielTestSatisfactionClient"];
            return value;
        }
        
        public static string CourrielTestSatisfactionClientTQE()
        {
            var value = ConfigurationManager.AppSettings["CourrielTestSatisfactionClientTQE"];
            return value;
        }

        public static string CourrielQIBSatisfactionClientFROM()
        {
            var value = ConfigurationManager.AppSettings["CourrielQIBSatisfactionClientFROM"];
            return value;
        }

        public static string CourrielQIBSatisfactionClientCC()
        {
            var value = ConfigurationManager.AppSettings["CourrielQIBSatisfactionClientCC"];
            return value;
        }

        public static string CourrielTQESatisfactionClientFROM()
        {
            var value = ConfigurationManager.AppSettings["CourrielTQESatisfactionClientFROM"];
            return value;
        }

        public static string CourrielTQESatisfactionClientCC()
        {
            var value = ConfigurationManager.AppSettings["CourrielTQESatisfactionClientCC"];
            return value;
        }

        public static string CourrielQIBConfirmationSatisfactionClientTo()
        {
            var value = ConfigurationManager.AppSettings["CourrielQIBConfirmationSatisfactionClientTo"];
            return value;
        }

        public static string CourrielQIBConfirmationSatisfactionClientFROM()
        {
            var value = ConfigurationManager.AppSettings["CourrielQIBConfirmationSatisfactionClientFROM"];
            return value;
        }

        public static string CourrielQIBConfirmationSatisfactionClientCC()
        {
            var value = ConfigurationManager.AppSettings["CourrielQIBConfirmationSatisfactionClientCC"];
            return value;
        }

        public static string CourrielTQEConfirmationSatisfactionClientTo()
        {
            var value = ConfigurationManager.AppSettings["CourrielTQEConfirmationSatisfactionClientTo"];
            return value;
        }

        public static string CourrielTQEConfirmationSatisfactionClientFROM()
        {
            var value = ConfigurationManager.AppSettings["CourrielTQEConfirmationSatisfactionClientFROM"];
            return value;
        }

        public static string CourrielTQEConfirmationSatisfactionClientCC()
        {
            var value = ConfigurationManager.AppSettings["CourrielTQEConfirmationSatisfactionClientCC"];
            return value;
        }



    }
}