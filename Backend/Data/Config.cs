using System.Dynamic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AuthServer
{
    public class Config
    {
        public Brand Brand { get; set; }
        public Dictionary<string, OAuthOptions> Providers { get; set; }
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
