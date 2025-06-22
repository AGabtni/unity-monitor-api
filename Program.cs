using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Unity.Monitoring.Data;
using Unity.Monitoring.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddAuthorization();

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
