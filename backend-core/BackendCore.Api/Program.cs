using BackendCore.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<BackendCore.Api.Repositories.Interfaces.IGrupoRepository, BackendCore.Api.Repositories.GrupoRepository>();
builder.Services.AddScoped<BackendCore.Api.Services.Interfaces.IGrupoService, BackendCore.Api.Services.GrupoService>();
builder.Services.AddScoped<BackendCore.Api.Repositories.Interfaces.IMiembroRepository, BackendCore.Api.Repositories.MiembroRepository>();
builder.Services.AddScoped<BackendCore.Api.Services.Interfaces.IMiembroService, BackendCore.Api.Services.MiembroService>();
builder.Services.AddSingleton<BackendCore.Api.Services.Interfaces.IEventPublisher, BackendCore.Api.Services.RabbitMqEventPublisher>();
builder.Services.AddScoped<BackendCore.Api.Repositories.Interfaces.IGastoRepository, BackendCore.Api.Repositories.GastoRepository>();
builder.Services.AddScoped<BackendCore.Api.Services.Interfaces.IGastoService, BackendCore.Api.Services.GastoService>();
builder.Services.AddScoped<BackendCore.Api.Repositories.Interfaces.IDeudaRepository, BackendCore.Api.Repositories.DeudaRepository>();
builder.Services.AddScoped<BackendCore.Api.Services.Interfaces.IDeudaService, BackendCore.Api.Services.DeudaService>();
builder.Services.AddHttpClient<BackendCore.Api.Services.Interfaces.IPagoLambdaClient, BackendCore.Api.Services.PagoLambdaClient>(client =>
{
    var baseUrl = builder.Configuration["ModuloPago:BaseUrl"] ?? "http://localhost:7071";
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "GastosClaros - Backend Core API",
        Version = "v1",
        Description = "API principal para gestion de grupos, gastos, saldos y deudas."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GastosClaros API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
