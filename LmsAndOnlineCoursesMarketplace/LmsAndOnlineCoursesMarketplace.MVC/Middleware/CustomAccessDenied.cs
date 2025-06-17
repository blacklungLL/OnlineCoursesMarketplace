using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LmsAndOnlineCoursesMarketplace.MVC.Middleware
{
    public class CustomAccessDeniedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomAccessDeniedMiddleware> _logger;

        public CustomAccessDeniedMiddleware(RequestDelegate next, ILogger<CustomAccessDeniedMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                _logger.LogWarning("403 Forbidden — пользователь перенаправлен на /errors/403.html");

                context.Response.Redirect("/errors/403.html");
            }
        }
    }
}