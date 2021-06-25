using FileSharing.Middleware.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileSharing.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> Logger;
        private readonly RequestDelegate Next;
        public ExceptionHandlerMiddleware(RequestDelegate Next, ILogger<ExceptionHandlerMiddleware> Logger)
        {
            this.Logger = Logger;
            this.Next = Next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex, "Uncaught Exception");

                var response = context.Response;
                response.ContentType = "application/json";

                // Creds to https://jasonwatmore.com/post/2020/10/02/aspnet-core-31-global-error-handler-tutorialW

                var statusCode = ex switch
                {
                    UnauthorizedException _ => HttpStatusCode.Unauthorized,
                    // Peticiones Malformadas
                    BadRequestException _ => HttpStatusCode.BadRequest,
                    // Errores no controlados
                    _ => HttpStatusCode.InternalServerError,
                };

                response.StatusCode = (int)statusCode;

                var result = JsonSerializer.Serialize(new { error = ex.ToString() });
                await response.WriteAsync(result).ConfigureAwait(false);
            }
        }
    }
}
