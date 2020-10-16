using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Infrastructure.Dtos
{
    public class ServicesForReportDto
    {
        public int Number { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string UnitMeasureName { get; set; }
        public string ContractorName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
