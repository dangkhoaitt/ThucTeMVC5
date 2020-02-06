using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThucHanh2MVC.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "bạn chưa có tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "bạn chưa nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [StringLength(20,MinimumLength = 6,ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "bạn chưa xác thực mật khẩu")]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "xác nhận chưa đúng")]      
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "bạn vui lòng nhập tên")]
        [Display(Name = "Họ tên khách hàng")]
        public string Name { get; set; }

        [Required(ErrorMessage = "bạn cần nhập địa chỉ để có thể đặt hàng nhé")]
        [Display(Name = "Địa chỉ")]     
        public string Address { get; set; }

        [Required(ErrorMessage = "bạn cần nhập Email để có thể đặt hàng nhé")]
        [EmailAddress(ErrorMessage = "địa chỉ Email chưa chính xác")]       
        public string Email { get; set; }

        [Required(ErrorMessage = "bạn vui lòng nhập số điện thoại")]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name= "Tỉnh/Thành")]
        public string ProvinceID { get; set; }

        [Display(Name= "Quận/Huyện")]
        public string DistrictID { get; set; }

        [Required(ErrorMessage = "bạn chưa nhập xác thực")]
        [Display(Name = "mã xác thực")]
        public string CaptchaCode { get; set; }
    }
}