using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace _7lab
{
    public class MiddlewareClass
    {


        private readonly RequestDelegate _next;
        TimeService _timeService;

        public MiddlewareClass(RequestDelegate next, TimeService timeService)
        {
            _next = next;
            _timeService = timeService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower() == "/time")
            {
                await context.Response.WriteAsync($"Текущее время: {_timeService?.GetTime()}");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class TimerExtensions
    {
        public static IApplicationBuilder UseTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareClass>();
        }
    }
}
public class TimeService
{
    public string GetTime() => System.DateTime.Now.ToString("hh:mm:ss");
}