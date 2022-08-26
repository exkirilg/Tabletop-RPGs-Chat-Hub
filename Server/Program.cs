using Microsoft.OpenApi.Models;
using Server.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1.0.0", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "Tabletop RPG's Chat-Hub Server",
        Contact = new OpenApiContact
        {
            Email = builder.Configuration["Contacts:Email"]
        },
        License = new OpenApiLicense
        {
            Name = "MIT Licence"
        }
    });

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .WithOrigins(builder.Configuration["Clients:WebClientURL"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1.0.0/swagger.json", "TtRPGs Chat-Hub Server API v1.0.0");
});
app.UseHttpsRedirection();
app.UseCors();
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();
