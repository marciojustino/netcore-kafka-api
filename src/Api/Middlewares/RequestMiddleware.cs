namespace Api.Middlewares
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestMiddleware
    {
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestMiddleware(ILogger<RequestMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                
                // Leave the body open so the next middleware can read it.
                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: (int)context.Request.ContentLength,
                    leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    // Do some processing with body…
                    
                    _logger.LogTrace("Request captured|Path={0}|RequestData={1}", context.Request.Path, body);

                    // Reset the request body stream position so the next middleware can read it
                    context.Request.Body.Position = 0;
                }
                
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleException(e, context.Response);
            }
        }

        private async Task HandleException(Exception exception, HttpResponse response)
        {
            _logger.LogError(exception, "Error on request.|Response={0}", response.Body);

            response.ContentType = "application/json";
            response.StatusCode = 500;
            await response.WriteAsync(JsonSerializer.Serialize(new {reason = exception.Message}));
        }
    }
}
