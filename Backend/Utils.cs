using System.Dynamic;
using System.Security.Claims;
using AspNet.Security.OAuth.MailRu;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AuthServer
{
    internal class Utils
    {
        private static readonly IDeserializer deserializer = new DeserializerBuilder().Build();
        private static readonly ISerializer serializer = new SerializerBuilder().Build();

        public static T Deserialize<T>(string input) => deserializer.Deserialize<T>(input);
        public static string Serialize<T>(T input) => serializer.Serialize(input);
    }


    public static class Extensions
    {
        public static T Cast<T>(this ExpandoObject obj)
        {
            var text = Utils.Serialize(obj);
            return Utils.Deserialize<T>(text);
        }

        public static T GetFrom<T>(this T target, T input) where T : OAuthOptions
        {
            target.ClientId = input.ClientId;
            target.ClientSecret = input.ClientSecret;
            return target;
        }

        // https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/samples/Mvc.Client/Extensions/HttpContextExtensions.cs
        public static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            return (from scheme in await schemes.GetAllSchemesAsync()
                where !string.IsNullOrEmpty(scheme.DisplayName)
                select scheme).ToArray();
        }

        public static UserInfo? GetInformation(this ClaimsPrincipal claimsPrincipal)
        {
            var idType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == idType);
            if (claim == null) return null;
            var provider = claim.Subject?.AuthenticationType;
            if (provider == null) return null;
            return new UserInfo { Provider = provider, Username = claim.Value };
        }
    }
}
