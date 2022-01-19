using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class EmployeeVM
    {
        public int Id_NhanVien { get; set; }

        /// <summary>
        /// The "Ma Nhan Vien"
        /// </summary>
        public string MaNhanVien { get; set; }

        /// <summary>
        /// The FullName
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }

        /// <summary>
        /// The Address
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string DiaChi { get; set; }

        /// <summary>
        /// The PhoneNumber
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string DienThoai { get; set; }

        /// <summary>
        /// The Email
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// The birthday
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string NamSinh { get; set; }

        /// <summary>
        /// the identity number card
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string SoCMND { get; set; }

        /// <summary>
        /// The date created identity card
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string NgayCapCMND { get; set; }

        /// <summary>
        /// The place created identity card
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string NoiCapCMND { get; set; }

        [Required]
        public int IdDonVi { get; set; }

        public int Chuc_Vu { get; set; }
        public string GhiChu { get; set; }
    }
}
