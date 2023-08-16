using System;

namespace Boilerplate.Entities.Common
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        public string MakeBy { get; set; }

        public DateTime? MakeDate { get; set; }

        public DateTime? InsertTime { get; set; } = DateTime.Now;

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? UpdateTime { get; set; } = DateTime.Now;
    }
}
