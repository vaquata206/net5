using Dapper.Contrib.Extensions;
using System;

namespace WebClient.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool DaXoa { get; set; }
    }
}