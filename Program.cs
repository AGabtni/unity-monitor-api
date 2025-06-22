using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
