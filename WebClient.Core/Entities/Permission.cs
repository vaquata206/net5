using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("Quyen")]
    public class Permission : BaseEntity
    {
        public string TenQuyen { get; set; }
        public int ThuTu { get; set; }
    }
}
