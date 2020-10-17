using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Core.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }
        public bool IsDefault { get; set; }
        public int Qty { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }
        public string Description { get; set; }
        public int ContractorSharePercent { get; set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public int UnitMeasureId { get; set; }
        public string UnitMeasureName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
