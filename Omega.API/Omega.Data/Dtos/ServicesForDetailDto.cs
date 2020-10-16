using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Infrastructure.Dtos
{
    public class ServicesForDetailDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public string UnitMeasureName { get; set; }
        public string Description { get; set; }
    }
}
