using System.Dynamic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
#pragma warning disable CS8618

namespace AuthServer
{
    public class Config
    {
        public Brand Brand { get; set; }
        public Dictionary<string, OAuthOptions> Providers { get; set; }
        public Dictionary<string, User> Users { get; set; }
        public Dictionary<string, HashSet<string>> Groups { get; set; }
        public Dictionary<string, Application> Applications { get; set; }
    }

    public class Application
    {
        public string Title { get; set; }
        public string BaseURL { get; set; }
        public HashSet<string> RedirectURLS { get; set; }
        public HashSet<string> Access { get; set; }
    }

    public class User
    {
        public string DisplayName { get; set; }
        public Dictionary<string, HashSet<string>> Providers { get; set; }
    }

    public class Brand
    {
        public string Name { get; set; }
        public string Logotype { get; set; }
        public Point Size { get; set; }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
