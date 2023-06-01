using CustomerWebApi;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbpassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var stringConnection = $"Data Source={dbHost};Initial Catalog={dbName}; User ID=sa; Password={dbpassword}";

builder.Services.AddDbContext<CustomerWebDbContext>(options =>
{
    options.UseSqlServer(stringConnection);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
