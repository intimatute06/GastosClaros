using ModuloCategoria.Worker;
using ModuloCategoria.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IClasificadorService, ClasificadorService>();

builder.Services.AddHttpClient<IBackendCoreClient, BackendCoreClient>(client =>
{
    var baseUrl = builder.Configuration["BackendCore:BaseUrl"] ?? "http://localhost:5035";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
