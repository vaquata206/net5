using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class EmailDepartmentVM
    {
        [Required]
        public int Id_DonVi { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string SMTP_Email { get; set; }

        public int? Port_Email { get; set; }

        [MaxLength(32)]
        public string Account_Email { get; set; }

        [MaxLength(32)]
        public string Pass_Email { get; set; }
    }
}
