using SQLite;
using System;
using System.Collections.Generic;

namespace PLI.Models
{
    public class Shipment
    {
        public int id { get; set; }
        public string number { get; set; }
        public string origin { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string delivery { get; set; }
        public double delivery_latitude { get; set; }
        public double delivery_longitude { get; set; }
        public DateTime? start_time { get; set; }
        public string status { get; set; }
        public int status_id { get; set; }
        [Ignore]
        public List<Remission> remissions { get; set; }
    }
}
