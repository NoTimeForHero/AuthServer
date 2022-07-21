using AuthServer;
using AuthServer.Data;
using AuthServer.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var config = StaticSerializer.Deserialize<Config>(File.ReadAllText("settings/main.yml"));

// Add services to the container.
var services = builder.Services;

services.AddSingleton(config);
services.AddSingleton<AccessService>();
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
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SpaMiddleware>(Constants.IndexFile);
app.MapControllers();

app.Run();
