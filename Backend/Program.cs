using System.Dynamic;
using System.Text;
using AspNet.Security.OAuth.MailRu;
using AuthServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = Utils.Deserialize<Config>(File.ReadAllText("settings/main.yml"));

// Add services to the container.
var services = builder.Services;

services.AddSingleton(config);
services.AddRouting();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
