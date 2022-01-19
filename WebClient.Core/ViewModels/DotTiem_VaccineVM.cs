using System.Collections.Generic;
using WebClient.Core.Entities;

namespace WebClient.Core.ViewModels
{
    public class DotTiem_VaccineVM : DotTiemVaccine
    {
        public string Ten_DonVi;
        public IEnumerable<ThongTin_NguoiDanVM> DsDoiTuongDangKy;
    }
}
