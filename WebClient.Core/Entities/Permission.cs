using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("DM_Quyen")]
    public class Permission
    {
        [ExplicitKey]
        public int Id_Quyen { get; set; }
        public string Ma_Quyen { get; set; }
        public string Ten_Quyen { get; set; }
        public int Tinh_Trang { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
