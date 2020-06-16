using System.Collections.Generic;

namespace PLI.Models.Post
{
    public class ChangeStatusUser
    {
        public int shipment_id { get; set; }
        public List<int> remissions_id { get; set; }
        public int status { get; set; }
    }
}
