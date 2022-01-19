using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class ChangePasswordVM
    {
        /// <summary>
        /// The current pasword
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Mật khẩu phải 5 ký tự trở lên!", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string MatKhauCu { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Mật khẩu phải 5 ký tự trở lên!", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }

        /// <summary>
        /// Confirm new passwork
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Xác nhận mật khẩu không chính xác!")]
        public string XacNhanMatKhau { get; set; }
    }
}
