
using mait.stats.Services;
using Mait.Stats.Dto;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = false;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
});

builder.Services.AddHttpClient<SerieSeriveClient>(client =>
{
    var options = builder.Configuration.GetSection(nameof(ClientsOptions)).Get<ClientsOptions>();
    client.BaseAddress = new Uri(options!.Series);
});

builder.Services.AddHttpClient<DocumentServiceClient>(client =>
{
    var options = builder.Configuration.GetSection(nameof(ClientsOptions)).Get<ClientsOptions>();
    client.BaseAddress = new Uri(options!.Documents);
});

builder.Services.AddHttpClient<FuncionarioServiceClient>(client =>
{
    var options = builder.Configuration.GetSection(nameof(ClientsOptions)).Get<ClientsOptions>();
    client.BaseAddress = new Uri(options!.Funcionario);
});


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Stat Service API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Funcionario Series API v1");
        options.RoutePrefix = "swagger"; // disponible en /swagger
    });
}

var serive = app.Services.GetService<SerieSeriveClient>() ??
throw new Exception("🟥 SerieSeriveClient no se ha registrado correctamente.");

var logger = app.Services.GetService<ILogger<StatsController>>() ??
throw new Exception("🟥 Logger no se ha registrado correctamente.");

logger.LogInformation("✅️ SerieSeriveClient ha sido registrado correctamente.");
logger.LogInformation("✅️ APLICACION INICIADA");
app.Run();

public partial class Program { } // <--- Esto es clave para que WebApplicationFactory lo encuentre
