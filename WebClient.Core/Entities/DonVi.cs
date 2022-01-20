using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("DONVI")]
    public class DonVi : BaseEntity
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int DonViCha { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }

        [Computed]
        public IEnumerable<DonVi> Children { get; set; }
    }
}
