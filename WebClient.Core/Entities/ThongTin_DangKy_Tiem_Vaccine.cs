using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("ThongTin_DangKy_Tiem_Vaccine")]
    public class ThongTin_DangKy_Tiem_Vaccine : BaseEntity
    {
        [ExplicitKey]
        public int Id_DangKy { get; set; }
        public int Id_ThongTin_NguoiDan { get; set; }
        public int Id_DotTiem_Vaccine { get; set; }
    }
}
