using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("TrangThaiPhieu")]
    public class TrangThaiPhieu : BaseEntity
    {
        public string Ten { get; set; }
    }
}
