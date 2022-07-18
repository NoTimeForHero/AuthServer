// File: Extensions.cs
// Created by NoTimeForHero, 2022
// Distributed under the Apache License 2.0

using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using AuthServer.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AuthServer.Utils;

public static class Extensions
{
    public static T Cast<T>(this ExpandoObject obj)
    {
        var text = StaticSerializer.Serialize(obj);
        return StaticSerializer.Deserialize<T>(text);
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

    public static AuthUserInfo? GetInformation(this ClaimsPrincipal claimsPrincipal)
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