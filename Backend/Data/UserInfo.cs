namespace AuthServer.Data
{
    public class UserInfo
    {
        public string Provider { get; set; } = "";
        public string Username { get; set; } = "";

        public string Combined => Provider.ToLower() + "_" + Username.ToLower();
    }
}
