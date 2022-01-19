using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("DM_DANTOC")]
    public class DmDanToc : BaseEntity
    {
        public int Id_DanToc { get; set; }
        public string Ma_DanToc { get; set; }
        public string Ten_DanToc { get; set; }
    }
}