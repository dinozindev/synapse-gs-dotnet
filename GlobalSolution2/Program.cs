using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using Asp.Versioning;
using DotNetEnv;
using GlobalSolution2;
using GlobalSolution2.Endpoints;
using GlobalSolution2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.Filters;

// TODO
// Documentação
// Exemplos???
// Vídeo

[assembly: InternalsVisibleTo("GlobalSolution2.Tests")]

// Carregando variáveis de ambiente do arquivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Visualizar saída do Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var serviceName = "Synapse_API";
var serviceVersion = "1.0.0";

// Configuração OpenTelemetry 
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation() 
            .AddHttpClientInstrumentation() 
            .AddSource(serviceName) 
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddConsoleExporter();  
    });

// CONFIGURAÇÃO DO JWT 
var jwtSecret = Environment.GetEnvironmentVariable("JwtSettings__Secret")
                ?? builder.Configuration["JwtSettings:Secret"];

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException(
        "JWT Secret não configurado. Configure a variável de ambiente 'JwtSettings__Secret' ou adicione em appsettings.json"
    );
}

var jwtIssuer = Environment.GetEnvironmentVariable("JwtSettings__Issuer")
                ?? builder.Configuration["JwtSettings:Issuer"]
                ?? "SynapseAPI"; 

var jwtAudience = Environment.GetEnvironmentVariable("JwtSettings__Audience")
                  ?? builder.Configuration["JwtSettings:Audience"]
                  ?? "SynapseUsers"; 

var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = !builder.Environment.IsEnvironment("Test"),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// VERSIONAMENTO DA API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


// ConnectionString usando variável de ambiente com .env
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__OracleConnection");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("ConnectionString Oracle não configurada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

// HEALTH CHECKS
builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: connectionString,
        name: "oracle-database",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "db", "oracle", "sql" },
        timeout: TimeSpan.FromSeconds(10)
    );

// SERVICES
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<CompetenciaService>();
builder.Services.AddScoped<RegistroBemEstarService>();
builder.Services.AddScoped<RecomendacaoProfissionalService>();
builder.Services.AddScoped<RecomendacaoSaudeService>();
builder.Services.AddScoped<ProcedureService>();

// RATE LIMITER
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opt =>
    {
        opt.AllowAnyOrigin();
        opt.AllowAnyMethod();
        opt.AllowAnyHeader();
    });
});

// JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// SWAGGER
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
     options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Synapse API", 
        Version = "v1" 
    });
    
    options.SwaggerDoc("v2", new OpenApiInfo 
    { 
        Title = "Synapse API", 
        Version = "v2" 
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Informe o token JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    options.ExampleFilters();
    options.IgnoreObsoleteActions();
    options.IgnoreObsoleteProperties();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

// MIDDLEWARES
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// ENDPOINTS VERSIONADOS
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .Build();

app.MapLoginEndpoints(apiVersionSet);
app.MapUsuarioEndpoints(apiVersionSet);
app.MapCompetenciaEndpoints(apiVersionSet);
app.MapRegistroBemEstarEndpoints(apiVersionSet);
app.MapRecomendacaoProfissionalEndpoints(apiVersionSet);
app.MapRecomendacaoSaudeEndpoints(apiVersionSet);
app.MapProcedureEndpoints(apiVersionSet);
app.MapHealthCheckEndpoints();

// SWAGGER UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Synapse API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Synapse API V2");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
    });
}

await app.RunAsync();

public partial class Program { }