﻿using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class InformationController : ControllerBase
    {
        private Config config;

        public InformationController(Config config)
        {
            this.config = config;
        }

        [HttpGet("api/settings")]
        public async Task<object> ListProviders()
        {
            var providers =
                (await HttpContext.GetExternalProvidersAsync())
                .Select(x => x.DisplayName);
            var user = User.GetInformation();
            var brand = config.Brand;
            return new { brand, user, providers };
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