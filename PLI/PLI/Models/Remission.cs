using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PLI.Models
{
    public class Remission
    {
        public int id { get; set; }
        public string number { get; set; }
        public string status { get; set; }
        public int status_id { get; set; }
        [Indexed]
        public int shipment_id { get; set; }
        [Ignore]
        public ObservableCollection<Product> products { get; set; }
        [Indexed]
        public bool IsChecked { get; set; }
        [Indexed]
        public bool IsEnabled { get; set; }
    }
}
