using System.Collections.Generic;

namespace PLI.Models.Post
{
    public class DeliveryPhoto
    {
        public int shipment_id { get; set; }
        public List<int> remissions_id { get; set; }
        public string photo { get; set; }
    }
}
