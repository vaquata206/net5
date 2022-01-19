using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("DM_DOITUONG_UUTIEN")]
    public class DmDoiTuongUuTien : BaseEntity
    {
        public int Id_DoiTuong { get; set; }
        public string Ma_DoiTuong { get; set; }
        public string Nghe_Nghiep { get; set; }
        public string Mo_Ta { get; set; }
    }
}
