using Microsoft.AspNetCore.Builder;

namespace meetingsAPI.Middleware
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseSimplExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
