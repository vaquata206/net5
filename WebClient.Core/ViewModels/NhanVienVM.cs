using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class NhanVienVM
    {
        public int Id { get; set; }

        /// <summary>
        /// The "Ma Nhan Vien"
        /// </summary>
        public string MaNhanVien { get; set; }

        /// <summary>
        /// The FullName
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string HoTen { get; set; }

        /// <summary>
        /// The Address
        /// </summary>
        [MaxLength(250)]
        [Required]
        public string DiaChi { get; set; }

        /// <summary>
        /// The PhoneNumber
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string SoDienThoai { get; set; }

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
        public string NgaySinh { get; set; }

        /// <summary>
        /// the identity number card
        /// </summary>
        [MaxLength(20)]
        public string SoCMND { get; set; }

        /// <summary>
        /// The date created identity card
        /// </summary>
        [MaxLength(20)]
        public string NgayCapCMND { get; set; }

        /// <summary>
        /// The place created identity card
        /// </summary>
        [MaxLength(200)]
        public string NoiCapCMND { get; set; }

        [Required]
        public int IdDonVi { get; set; }

        public int ChucVu { get; set; }
        public string GhiChu { get; set; }
    }
}
