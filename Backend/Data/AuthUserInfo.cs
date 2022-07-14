namespace AuthServer.Data
{
    public class AuthUserInfo
    {
        public string Provider { get; set; } = "";
        public string Id { get; set; } = "";
        public string DisplayName { get; set; } = "";

        public string FullId => Provider.ToLower() + "_" + Id.ToLower();
    }
}
