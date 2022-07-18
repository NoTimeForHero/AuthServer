using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace AuthServer.Utils
{
    public class SpaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string indexPath;

        public SpaMiddleware(RequestDelegate next, string indexPath)
        {
            _next = next;
            this.indexPath = indexPath;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (context.Response.StatusCode != 404) return;
            if (context.Response.HasStarted) return;

            var file = new IndexFile();
            await file.Write(context.Response);
        }
    }
}
