using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class BaoHongVM
    {
        public int Id { get; set; }
        [Required]
        public int IdDichVu { get; set; }
        [MaxLength(200)]
        public string MoTa { get; set; }
    }
}
