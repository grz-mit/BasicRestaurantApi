using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Middleware
{
    public class RequestTimerMiddleware : IMiddleware
    {
        private readonly Stopwatch _timer = new Stopwatch();
        private readonly ILogger _logger;

        public RequestTimerMiddleware(ILogger<RequestTimerMiddleware> logger)
        {
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _timer.Start();
            await next.Invoke(context);
            _timer.Stop();

            decimal timeElapsed = _timer.ElapsedMilliseconds/1000M;

            if(timeElapsed > 4)
            {
                var message = $"Request {context.Request.Method} on rout {context.Request.Path} completed in {timeElapsed}";

                _logger.LogInformation(message);
            }
        }
    }
}
