using JwtAuthenticationManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ocelot configuration >>>
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("ocelot.json", false, reloadOnChange: true)
       .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);

// add Authentication to ocelot
builder.Services.AddCustomJwtAuthentication();
// <<<

// <<<

var app = builder.Build();

// Ocelot configuration >>>
await app.UseOcelot();

// add Authentication to ocelot
app.UseAuthentication();
app.UseAuthorization();
// <<<

// <<<
app.Run();
