using System.Security.Claims;

namespace AuthServer.Data
{
    public class AuthUserInfo
    {
        public string Provider { get; set; } = "";
        public string Id { get; set; } = "";
        public string DisplayName { get; set; } = "";

        public string FullId => Provider.ToLower() + "_" + Id.ToLower();

        public static AuthUserInfo? Get(ClaimsPrincipal claimsPrincipal)
        {
            var idType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var nameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == idType);
            if (claim == null) return null;
            var provider = claim.Subject?.AuthenticationType;
            if (provider == null) return null;
            var name = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameType)?.Value;
            return new AuthUserInfo { Provider = provider, Id = claim.Value, DisplayName = name };
        }
    }
}
