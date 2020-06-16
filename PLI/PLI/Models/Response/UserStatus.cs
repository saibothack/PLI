using System.Collections.Generic;

namespace PLI.Models.Response
{
    public class UserStatus : Default
    {
        public List<Status> status { get; set; }
    }

    public class Status
    {
        public int status_user_id { get; set; }
    }
}
