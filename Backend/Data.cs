using System.Dynamic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AuthServer
{
    public class Config
    {
        public Dictionary<string, OAuthOptions> Providers { get; set; }
    }

    public class UserInfo
    {
        public string Provider { get; set; } = "";
        public string Username { get; set; } = "";

        public string Combined => Provider.ToLower() + "_" + Username.ToLower();
    }
}
