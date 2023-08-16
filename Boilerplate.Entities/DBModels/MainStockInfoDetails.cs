using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Entities.DBModels
{
    public class MainStockInfoDetails
    {
        public string? MSIDId { get; set; }
        public string? MSIId { get; set; }
        public string? ChallanNo { get; set; }
        public string? ItemId { get; set; }
        public decimal Qty_ML { get; set; }
        public decimal Qty_Pag { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
