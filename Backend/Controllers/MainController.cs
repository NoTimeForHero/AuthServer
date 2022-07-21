using System.Net;
using System.Web;
using AuthServer.Data;
using AuthServer.Services;
using AuthServer.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class MainController : ControllerBase
    {
        private Config config;
        private AccessService access;
        private TokenService tokenService;

        public MainController(Config config, AccessService access, TokenService tokenService)
        {
            this.config = config;
            this.access = access;
            this.tokenService = tokenService;
        }

        [HttpGet("api/login")]
        public async Task<object> List(string provider, string redirect)
        {
            if (string.IsNullOrEmpty(provider)) return BadRequest(new { Message = "Missing provider!" });
            if (string.IsNullOrEmpty(redirect)) return BadRequest(new { Message = "Missing redirect!" });
            var providers = await HttpContext.GetExternalProvidersAsync();
            var founded = providers.FirstOrDefault(x =>
                string.Equals(x.Name, provider, StringComparison.CurrentCultureIgnoreCase));
            if (founded == null)
                return BadRequest(new { Message = $"Unknown provider: {provider}"});
            return Challenge(new AuthenticationProperties { RedirectUri = redirect }, founded.Name);
        }

        // TODO: Написать юнит-тесты?
        [HttpGet("api/authorize")]
        public object TryAuthorize(string? app, string? redirect = null)
        {
            var model = new AuthModel(config, app, redirect);
            if (model.Error != null) return model.Error;
            var application = model.Application;
            return new
            {
                Application = new {
                    Id = app,
                    application.Title,
                    application.BaseURL
                },
                model.Redirect
            };
        }

        [HttpGet("api/access")]
        public object GetAccess(string? app, string? redirect = null)
        {
            if (!(User.Identity?.IsAuthenticated ?? false)) return BadRequest(new { Message = "No login info!" });
            var model = new AuthModel(config, app, redirect);
            if (model.Error != null) return model.Error;

            var user = AuthUserInfo.Get(User);
            if (user == null) return this.ServerError("Cannot get login info!");

            if (!access.HasAccess(model.Application, user))
                return this.Forbid(new { Message = "Access denied for this user ID!", Details = new {user.Id}});

            var token = tokenService.Generate(model.Application, user);
            redirect = string.Format(model.Redirect, token);
            return new { redirect, token };
        }


        private class AuthModel
        {
            public Application Application { get; private set; }
            public string Redirect { get; private set; }
            public ObjectResult? Error { get; }

            public AuthModel(Config config, string? app, string? redirect = null)
            {
                Error = DoValidate(config, app, redirect);
            }

            private ObjectResult? DoValidate(Config config, string? app, string? redirect = null)
            {
                if (app == null) return new NotFoundObjectResult(new { Message = "Missing application ID!" });
                if (!config.Applications.TryGetValue(app, out var application))
                {
                    return new NotFoundObjectResult(new { Message = $"Application \"{app}\" not found!" });
                }
                redirect ??= application.RedirectURLS.First();

                if (!application.RedirectURLS.Contains(redirect))
                    return new BadRequestObjectResult(new { Message = $"Invalid redirect URL for this app!" });

                Application = application;
                Redirect = redirect;
                return null;
            }
        }
    }
}
