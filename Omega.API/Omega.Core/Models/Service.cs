using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Core.Models
{
    public class Service
    {
        [JsonProperty("$id")]
        public string Id { get; set; }
        public int serviceId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int price { get; set; }
        public bool isDefault { get; set; }
        public int qty { get; set; }
        public int minQty { get; set; }
        public int maxQty { get; set; }
        public string description { get; set; }
        public int contractorSharePercent { get; set; }
        public int contractorId { get; set; }
        public string contractorName { get; set; }
        public int unitMeasureId { get; set; }
        public string unitMeasureName { get; set; }
    }
}
