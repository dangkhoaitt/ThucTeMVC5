using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;


namespace ThucHanh2MVC
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
   
            var session = (UserLogin)HttpContext.Current.Session[Common.CommonStatic.USER_SESSION];
            if(session == null)
            {
                return false;
            }


            List<string> privilegeLevels = this.GetCredentiaByLoggedInUser(session.UserName); //Call another method to get rights of the

            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
            };
        }

        private List<string> GetCredentiaByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonStatic.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}