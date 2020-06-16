using SQLite;

namespace PLI.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool? specify_load { get; set; }
        public string load { get; set; }
        [Indexed]
        public int remission_id { get; set; }
    }
}
