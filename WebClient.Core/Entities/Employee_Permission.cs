using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("NguoiDung_Quyen")]
    public class Employee_Permission : BaseEntity
    {
        public int Id_NhanVien { get; set; }
        public int Id_Quyen { get; set; }
        public int Id_ChucNang { get; set; }
    }
}