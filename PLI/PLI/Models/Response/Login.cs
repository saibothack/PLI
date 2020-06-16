namespace PLI.Models.Response
{
    public class Login : Default
    {
        public string id { get; set; }
        public string token { get; set; }
        public bool changePassword { get; set; }
    }
}
