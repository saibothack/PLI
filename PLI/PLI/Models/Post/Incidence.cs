using System;
using System.Collections.Generic;
using System.Text;

namespace PLI.Models.Post
{
    public class Incidence
    {
        public int shipment_id { get; set; }
        public int incidences_id { get; set; }
        public string comments { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
