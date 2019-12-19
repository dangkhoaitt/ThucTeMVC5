using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanh2MVC.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }//vì UserID có kiểu bigint là kiểu long trong c#
        public string UserName { get; set; }
    }
}