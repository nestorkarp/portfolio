using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RRHH.WebApi.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        //En este punto se puede inyectar cualquier servicio que se requiera como: BD, Email, etc.)
        
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Registrar el error en el archivo .txt (vía Serilog)
            _logger.LogError(exception, "Ocurrió una excepción no controlada: {Message}", exception.Message);

            // 2. Crear una respuesta estándar usando ProblemDetails (Estándar RFC 7807)
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Ocurrió un error inesperado. Por favor, intente más tarde.",
                Instance = httpContext.Request.Path
            };

            // 3. Configurar la respuesta HTTP
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            // 4. Escribir la respuesta en el cliente
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // Retornar 'true' significa que manejamos el error y la ejecución termina acá
            return true;
        }
    }

}
