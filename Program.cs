using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Unity.Monitoring.Data;
using Unity.Monitoring.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

    // JWT Auth support in Swagger
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

// Config db context to use PostgreSQL cnx
var connectionString = builder
    .Configuration.GetConnectionString("DefaultConnection")
    .Replace("${PGDBO}", Environment.GetEnvironmentVariable("PGDBO"))
    .Replace("${PGPASSWORD}", Environment.GetEnvironmentVariable("PGPASSWORD"));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Dependency injection
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IMetricDataService, MetricDataService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Auth service + Jwt:
var jwtService = builder.Services.BuildServiceProvider().GetRequiredService<IJwtService>();
jwtService.ConfigureJwtAuthentication(builder.Services);

builder.Services.AddAuthorization(options =>
    options.AddPolicy("AdminRights", policy => policy.RequireRole("Admin"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
