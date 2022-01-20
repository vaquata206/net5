using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("PhanQuyen")]
    public class PhanQuyen : BaseEntity
    {
        public int IdTaiKhoan { get; set; }
        public int? IdQuyen { get; set; }
        public int? IdChucNang { get; set; }
    }
}