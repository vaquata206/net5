using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("Chuc_Nang")]
    public class Feature
    {
        [ExplicitKey]
        public int Id_ChucNang { get; set; }
        public string Ma_ChucNang { get; set; }
        public string Ten_ChucNang { get; set; }
        public string MoTa_ChucNang { get; set; }
        public string Tooltip { get; set; }
        public string Icon { get; set; }
        public int Id_ChucNang_Cha { get; set; }
        public string Controller_Name { get; set; }
        public string Action_Name { get; set; }
        public int Thu_Tu { get; set; }
        public int HienThi_Menu { get; set; }
        public int Tinh_Trang { get; set; }
        public int Id_ChuongTrinh { get; set; }

        [Computed]
        public IEnumerable<Feature> Children { get; set; }
    }
}
