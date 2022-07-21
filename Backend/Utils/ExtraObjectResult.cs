using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Utils
{
    class ForbidResult : ObjectResult
    {
        public ForbidResult(object? value) : base(value) {}
    }

    class ServerErrorResult : ObjectResult
    {
        public ServerErrorResult(object? value) : base(value) { }
    }

    internal static class ObjectResultExtensions
    {
        public static ForbidResult Forbid(this ControllerBase controller, object? value)
        {
            controller.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return new ForbidResult(value);
        }

        public static ServerErrorResult ServerError(this ControllerBase controller, object? value)
        {
            controller.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new ServerErrorResult(value);
        }
    }
}
