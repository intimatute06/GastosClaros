var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("SwaggerHub", policy =>
    {
        policy.WithOrigins("https://app.swaggerhub.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("SwaggerHub");

app.MapGet("/", () => "GastosClaros API Gateway - activo");

app.MapReverseProxy();

app.Run();
