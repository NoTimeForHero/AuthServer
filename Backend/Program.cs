using System.Net;
using AuthServer;
using AuthServer.Data;
using AuthServer.Services;
using AuthServer.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var config = StaticSerializer.Deserialize<Config>(File.ReadAllText("settings/main.yml"));

// Add services to the container.
var services = builder.Services;

services.AddSingleton(config);
services.AddSingleton<AccessService>();
services.AddSingleton<TokenService>();
services.AddRouting();

services.Configure<ForwardedHeadersOptions>(options =>
{
    if (!config.Network.UseForwarding) return;
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.RequireHeaderSymmetry = false;
    options.ForwardLimit = config.Network.ForwardLimit;
    var networks = config.Network.KnownNetworks;
    var proxies = config.Network.KnownProxies;
    networks.ForEach(options.KnownNetworks.Add);
    proxies.ForEach(options.KnownProxies.Add);
});


var auth = services.AddAuthentication((options) =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
});

// auth.AddJwtBearer((options) =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters()
//     {
//         ValidateAudience = false,
//         ValidTypes = new[] { "at+jwt" },
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("<<key/secret>>")),
//     };
// });

auth.AddCookie(options =>
{
    options.LoginPath = "/signin";
    options.LogoutPath = "/signout";
});

if (config.Providers.TryGetValue("MailRu", out var mailSettings))
    auth.AddMailRu((opts) => opts.GetFrom(mailSettings));

if (config.Providers.TryGetValue("GitHub", out var gitHubSettings))
    auth.AddGitHub((opts) => opts.GetFrom(gitHubSettings));

if (config.Providers.TryGetValue("Google", out var googleSettings))
    auth.AddGoogle((opts) => opts.GetFrom(googleSettings));

if (config.Providers.TryGetValue("Yandex", out var yandexSettings))
    auth.AddYandex((opts) => opts.GetFrom(yandexSettings));

services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (config.Network.UseForwarding)
{
    app.Logger.LogInformation("ForwardingHeaders Enabled!");
    app.UseForwardedHeaders();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SpaMiddleware>(Constants.IndexFile);
app.MapControllers();

app.Run();
