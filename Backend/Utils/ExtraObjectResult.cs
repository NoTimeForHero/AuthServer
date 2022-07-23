using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Utils
{
    class ForbidResult : ObjectResult
    {
        public ForbidResult(object? value) : base(value)
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }

    class ServerErrorResult : ObjectResult
    {
        public ServerErrorResult(object? value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

    class SuccessResult : ObjectResult
    {
        public SuccessResult(object? value) : base(value)
        {
            StatusCode = StatusCodes.Status200OK;
        }
    }

    internal static class ObjectResultExtensions
    {
        public static ForbidResult Forbid(this ControllerBase _, object? value)
        {
            return new ForbidResult(value);
        }

        public static SuccessResult Success(this ControllerBase _, object? value)
        {
            return new SuccessResult(value);
        }

        public static ServerErrorResult ServerError(this ControllerBase _, object? value)
        {
            return new ServerErrorResult(value);
        }
    }
}
