using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ThucHanh2MVC.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời bạn nhập Tên Tài Khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời bạn nhập Mật Khẩu")]
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}