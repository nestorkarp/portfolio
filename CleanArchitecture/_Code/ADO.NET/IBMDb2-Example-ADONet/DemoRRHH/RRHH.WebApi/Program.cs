using Microsoft.AspNetCore.Mvc;
using RRHH.Infrastructure.Persistence.Configuration;
using RRHH.WebApi;
using RRHH.WebApi.Exceptions;
using Serilog;


// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error() // Guarda solo Errores y Fatals
    .WriteTo.File("Logs/rrhh-api-errors-.txt",
        rollingInterval: RollingInterval.Day, // Crea un archivo nuevo por día
        outputTemplate: "{Timestamp:yyyy-MM-day HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);


// Indicar a la app que use Serilog
builder.Host.UseSerilog();


// Registrar el Handler en el contenedor de dependencias
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Agregar los servicios necesarios para ProblemDetails
builder.Services.AddProblemDetails();


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cfg Infrastructure y App
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

//Permite visualizar msj de validación definido a través de  DataAnnotations
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
//..

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Middleware global para capturar excepciones no controladas
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        // Esto previene que se muestren detalles sensibles al usuario
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"Ocurrió un error interno en el servidor.\"}");
    });
});

app.UseAuthorization();

app.MapControllers();


try
{
    Log.Information("Start app...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start app...");
}
finally
{
    Log.CloseAndFlush();
}
