using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanh2MVC.Common
{
    public static class CommonStatic
    {
        public const string USER_SESSION = "USER_SESSION";
        public const string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public const string CartSession = "CartSession";
        public static string CurrentCulture { get; set; }
    }
}