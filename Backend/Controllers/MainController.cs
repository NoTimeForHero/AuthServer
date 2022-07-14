﻿using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class MainController : ControllerBase
    {
        private Config config;

        public MainController(Config config)
        {
            this.config = config;
        }


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

        // TODO: Написать юнит-тесты?
        [HttpGet("authorize")]
        public async Task<object> TryAuthorize(string? app, string? redirect = null)
        {

            if (app == null) return NotFound(new { Message = "Missing application ID!" });
            if (!config.Applications.TryGetValue(app, out var application))
            {
                return NotFound(new { Message = $"Application \"{app}\" not found!" });
            }

            redirect ??= application.RedirectURLS.First();

            if (!application.RedirectURLS.Contains(redirect))
                return BadRequest(new { Message = $"Invalid redirect URL for this app!" });

            return new
            {
                Application = application.Title,
                Redirect = redirect
            };
        }

    }
}
