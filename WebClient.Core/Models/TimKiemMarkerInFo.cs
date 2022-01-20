using System.Collections.Generic;

namespace WebClient.Core.Models
{
    public class TimKiemMarkerInFo
    {
        public int Id { get; set; }
        public string Ma { get; set; }
        public string DiaChi { get; set; }
        public string Ten { get; set; }
        public decimal KinhDo { get; set; }
        public decimal ViDo { get; set; }
        public int Loai { get; set; }
        public int IdKhuCongNghiep { get; set; }
        public string AnhDaiDien { get; set; }
        public int? IdBanDo { get; set; }
    }

    public class TimKiemMarkerMobile : TimKiemMarkerInFo
    {
        public IEnumerable<PolygonInfo> ListPolygon { get; set; }
    }
}