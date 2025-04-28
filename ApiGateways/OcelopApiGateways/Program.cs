using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureLogging(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());

builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
