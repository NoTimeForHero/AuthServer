using System.Dynamic;
using AuthServer.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
#pragma warning disable CS8618

namespace AuthServer
{
    public class Config
    {
        public Brand Brand { get; set; }
        public Dictionary<string, OAuthOptions> Providers { get; set; } = new();
        public Dictionary<string, User> Users { get; set; } = new();
        public Dictionary<string, HashSet<string>> Groups { get; set; } = new();
        public Dictionary<string, Application> Applications { get; set; } = new();
        public TokenSettings Token { get; set; } = new();
    }

    public class TokenSettings
    {
        public AlgType Algorithm { get; set; }
        public string? Secret { get; set; }
        public string? SecretFile { get; set; }
        public string? Issuer { get; set; }
        public TimeSpan TTL { get; set; }

        public enum AlgType
        {
            HS256, HS384, HS512,
            RS256, RS384, RS512,
        }
    }

    public class Application
    {
        public string Title { get; set; }
        public string BaseURL { get; set; }
        public HashSet<string> RedirectURLS { get; set; } = new();
        public HashSet<string> Access { get; set; } = new();
        public string? Audience { get; set; }

        public IEnumerable<string> AccessUsers => Access.Where(x => !x.StartsWith(Constants.GroupPrefix));
        public IEnumerable<string> AccessGroups => Access
            .Where(x => x.StartsWith(Constants.GroupPrefix))
            .Select(x => x.Replace(Constants.GroupPrefix, ""));
    }

    public class User
    {
        public string DisplayName { get; set; }
        public Dictionary<string, HashSet<string>> Providers { get; set; } = new();

        public static IEnumerable<(string provider, string id)> FlattenProviders(User user) =>
            user.Providers.SelectMany(pair =>
                pair.Value.Select(id => (pair.Key, id)).ToList()
            );
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
