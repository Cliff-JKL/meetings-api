using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using meetingsAPI.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Middleware
{
    public class ExceptionMiddleware
    {
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        private RequestDelegate Next { get; }
        private ILogger<ExceptionMiddleware> Logger { get;  }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                var detail = new ErrorDetails
                {
                    Message = e.Message,
                    InnerMessage = e?.InnerException?.Message,
                };
                Logger.LogError(e.Message + " " + e?.InnerException?.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(detail));
            }
        }
    }
}
