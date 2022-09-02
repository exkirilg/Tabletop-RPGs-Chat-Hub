using DataAccess;
using Domain.DataAccessInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Hubs;
using Services;
using Services.Interfaces;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSignalR();

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

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Auth:Audience"],
        ValidateLifetime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Auth:JWTSecret"])),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Identity"));
});

builder.Services.AddDbContext<ChatHubContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ChatHub"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<INotificationsServices, NotificationsServices>();

builder.Services.AddScoped<IStatisticsServices, StatisticsServices>();
builder.Services.AddScoped<IIdentityServices, IdentityServices>();
builder.Services.AddScoped<IChatServices, ChatServices>();

builder.Services.AddSingleton<ChatHubBroadcast>();

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1.0.0/swagger.json", "TtRPGs Chat-Hub Server API v1.0.0");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();
