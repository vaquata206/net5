using System.Collections.Generic;

namespace WebClient.Core.Models
{
    public class PolygonInfo
    {
        public string PolygonId { get; set; }
        public decimal StrokeWidth { get; set; }
        public string FillColor { get; set; }
        public string StrokeColor { get; set; }
        public List<ToaDo> Points { get; set; }
    }

    public class ToaDo
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
