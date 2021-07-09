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


                var response = context.Response;
                response.ContentType = "application/json";

                // Creds to https://jasonwatmore.com/post/2020/10/02/aspnet-core-31-global-error-handler-tutorialW

                HttpStatusCode statusCode;
                switch (ex)
                {
                    case UnauthorizedException:
                        Logger.LogWarning(ex.Message, nameof(UnauthorizedException));
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    // Peticiones Malformadas
                    case BadRequestException:
                        Logger.LogWarning(ex.Message, nameof(BadRequestException));
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    // Errores no controlados
                    default:
                        Logger.LogError(exception: ex, "Uncaught Exception");
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                response.StatusCode = (int)statusCode;

                var result = JsonSerializer.Serialize(new { error = ex.ToString() });
                await response.WriteAsync(result).ConfigureAwait(false);
            }
        }
    }
}
