
using System.ComponentModel.DataAnnotations;

namespace Nhom8.ViewModels
{
    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "*")]
        //public string? Role { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Tk { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mật khẩu")]
        public string? Mk { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Họ và tên")]
        public string? TenKh { get; set; }

        [MaxLength(15, ErrorMessage = "*")]
        [RegularExpression(@"0\d{9}",ErrorMessage ="Chưa đúng định dạng")]
        public string? Sdt { get; set; }

        [EmailAddress(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int? Otp { get; set; }

        public string? Img { get; set; }
       
    }
}
