using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("TaiKhoan")]
    public class DichVu : BaseEntity
    {
        public string Ten { get; set; }
    }
}
