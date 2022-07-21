// File: Extensions.cs
// Created by NoTimeForHero, 2022
// Distributed under the Apache License 2.0

using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using AuthServer.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;

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
}