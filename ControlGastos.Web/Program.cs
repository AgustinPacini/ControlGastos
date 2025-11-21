using System;
using System.Text;
using ControlGastos.Application;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Infrastructure.Data;
using ControlGastos.Infrastructure.Repositories;
using ControlGastos.Infrastructure.Services;
using ControlGastos.Web.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Configuración de servicios (Dependency Injection)
// ==========================================

// Controladores (Web API)
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ControlGastos API",
        Version = "v1"
    });

    // Definición del esquema de seguridad Bearer
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese solo el token JWT (sin 'Bearer '), Swagger se encargará del header.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// DbContext con SQL Server
builder.Services.AddDbContext<ControlGastosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios genéricos y específicos
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IGastoRepository, GastoRepository>();
builder.Services.AddScoped<IIngresoRepository, IngresoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>();

// MediatR (CQRS)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

// FluentValidation (validación de DTOs / comandos)
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

// Servicio para generación de JWT
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// CORS para permitir el front en React (puertos típicos de Vite/React)
const string AllowFrontendPolicy = "FrontendCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontendPolicy,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:8081",
                             "http://localhost:8080") // tu front
                .AllowAnyHeader()
                .AllowAnyMethod();
            // Si más adelante usás cookies/autenticación con credenciales:
            // .AllowCredentials();
        });
});

// ===============================
// Configuración de Autenticación JWT
// ===============================
var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no está configurado");
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
        ValidateAudience = !string.IsNullOrWhiteSpace(audience),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.FromMinutes(2) // pequeña tolerancia en expiración
    };
});

// Autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// ==========================================
// Configuración del pipeline HTTP
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware global para manejo prolijo de errores
app.UseMiddleware<ExceptionHandlingMiddleware>();

// CORS para permitir llamadas desde el front
app.UseCors(AllowFrontendPolicy);

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Endpoints de la API
app.MapControllers();

// Punto de entrada de la aplicación
app.Run();
