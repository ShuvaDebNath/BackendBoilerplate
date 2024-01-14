using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Entities.DBModels
{
    public class MainStockInfoSingle
    {
        public string? MSIId { get; set; }
        public string? ComId { get; set; }
        public string? SupplierId { get; set; }
        public string? MRRNo { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string? MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public DateTime InsertTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<MainStockInfoDetails> Details { get; set; }
        public MainStockInfoSingle()
        {
            Details = new List<MainStockInfoDetails>();
        }
    }
}
