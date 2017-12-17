using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Security;
using System.Security.Principal;
using System;

namespace Commons.Helper
{
    /// <summary>
    /// Class to provide a unified area of authentication/authorization checking.
    /// </summary>
    public partial class Security : IHttpModule
    {
        static Security()
        {

        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += ContextAuthenticateRequest;
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the context control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private static void ContextAuthenticateRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;

            // FormsAuthCookieName is a custom cookie name based on the current instance.
            HttpCookie authCookie = context.Request.Cookies[FormsAuthCookieName];
            if (authCookie != null)
            {


                FormsAuthenticationTicket authTicket = null;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch (Exception ex)
                {
                    //Utils.Log("Failed to decrypt the FormsAuthentication cookie.", ex);
                }

                if (authTicket != null && !string.IsNullOrWhiteSpace(authTicket.UserData))
                {
                    int delimiter = authTicket.UserData.IndexOf(AUTH_TKT_USERDATA_DELIMITER);
                    if (delimiter != -1)
                    {
                        // for extra security, make sure the data in UserData contains the SecurityValidationKey
                        // and current blog instance.  the current blog instance check would prevent a cookie name
                        // change for a forms auth cookie encrypted in the same application (different blog) as
                        // being valid for this blog instance.

                        string securityValidationKey = authTicket.UserData.Substring(0, delimiter).Trim();
                        string blogId = authTicket.UserData.Substring(delimiter + AUTH_TKT_USERDATA_DELIMITER.Length).Trim();

                    }
                }
            }

            //context.User = unauthPrincipal;
        }

        /// <summary>
        /// Name of the Forms authentication cookie for the current blog instance.
        /// </summary>
        public static string FormsAuthCookieName
        {
            get
            {
                return FormsAuthentication.FormsCookieName + "-" + "ABVKHGJ";
            }
        }

        /// <summary>
        /// Signs out user out of the current blog instance.
        /// </summary>
        public static void SignOut()
        {
            // using a custom cookie name based on the current blog instance.
            HttpCookie cookie = new HttpCookie(FormsAuthCookieName, string.Empty);
            cookie.Expires = DateTime.Now.AddYears(-3);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SaveCookie(string uname, string _userDate)
        {
            bool rememberMe = true;
            string userDate = _userDate;
            HttpContext context = HttpContext.Current;
            DateTime expirationDate = DateTime.Now.Add(FormsAuthentication.Timeout);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                uname,
                DateTime.Now,
                expirationDate,
                rememberMe,
                userDate,
                FormsAuthentication.FormsCookiePath
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // setting a custom cookie name based on the current blog instance.
            // if !rememberMe, set expires to DateTime.MinValue which makes the
            // cookie a browser-session cookie expiring when the browser is closed.
            HttpCookie cookie = new HttpCookie(FormsAuthCookieName, encryptedTicket);
            cookie.Expires = rememberMe ? expirationDate : DateTime.MinValue;
            // cookie.Expires = DateTime.Now.AddMinutes(30);
            cookie.HttpOnly = true;
            context.Response.Cookies.Set(cookie);
        }

        public static string GetCookie()
        {
            HttpContext context = HttpContext.Current;
            HttpCookie authCookie = context.Request.Cookies[FormsAuthCookieName];
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex) { return null; }
            return authTicket.UserData;
        }



        public static void SaveTCCookie(string _userDate)
        {
            bool rememberMe = true;
            string userDate = _userDate;
            HttpContext context = HttpContext.Current;
            DateTime expirationDate = DateTime.Now.Add(FormsAuthentication.Timeout);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                "LoginInfommlx",
                DateTime.Now,
                expirationDate,
                rememberMe,
                userDate,
                FormsAuthentication.FormsCookiePath
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // setting a custom cookie name based on the current blog instance.
            // if !rememberMe, set expires to DateTime.MinValue which makes the
            // cookie a browser-session cookie expiring when the browser is closed.
            HttpCookie cookie = new HttpCookie("LoginInfommlx", encryptedTicket);
            cookie.Expires = rememberMe ? expirationDate : DateTime.MinValue;
            cookie.HttpOnly = true;
            context.Response.Cookies.Set(cookie);
        }
         

        public static string GetTCCookie()
        {
            HttpContext context = HttpContext.Current;
            HttpCookie authCookie = context.Request.Cookies["LoginInfommlx"];
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex) { return null; }
            return authTicket.UserData;
        }


        public static void Logon()
        {
            // using a custom cookie name based on the current blog instance.
            HttpCookie cookie = new HttpCookie("LoginInfommlx", string.Empty);
            cookie.Expires = DateTime.Now.AddYears(-3);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }




        private const string AUTH_TKT_USERDATA_DELIMITER = "-|-";


        #region "Properties"


        #endregion


    }

}
