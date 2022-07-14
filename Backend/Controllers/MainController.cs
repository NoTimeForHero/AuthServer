using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class MainController : ControllerBase
    {
        public object Get()
        {
            return new { Message = "Hello world!" };
        }

        [HttpGet("api/login")]
        public async Task<object> List(string provider)
        {
            if (string.IsNullOrEmpty(provider)) return BadRequest(new { Message = "Missing provider!" });

            var providers = await HttpContext.GetExternalProvidersAsync();


            var founded = providers.FirstOrDefault(x =>
                string.Equals(x.Name, provider, StringComparison.CurrentCultureIgnoreCase));

            if (founded == null)
                return BadRequest(new { Message = $"Unknown provider: {provider}"});

            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, founded.Name);
        }

        [HttpGet("api/settings")]
        public async Task<object> ListProviders()
        {
            var providers = 
                (await HttpContext.GetExternalProvidersAsync())
                .Select(x => x.DisplayName);
            var user = User.GetInformation();
            return new { providers, user };
        }

        [HttpGet("api/info")]
        public object Info()
        {
            if (!(User.Identity?.IsAuthenticated ?? false)) return new { Message = "No login info!" };
            var claims = User.Claims.Select(x => new
                {
                    x.Type,
                    x.Value,
                    x.ValueType,
                    Subject = x.Subject?.AuthenticationType ?? "unknown"
                })
                .GroupBy(x => x.Subject)
                .ToDictionary(x => x.Key, x => x.ToList());
            var user = User.GetInformation();
            return new { claims, user };
        }

    }
}
