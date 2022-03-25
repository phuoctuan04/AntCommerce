using System.IO.Compression;
using System.Net;
using System.Reflection;
using AntCommerce.Module.Modular.Extensions;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Exceptions;
using WebHost.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel().ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Any, 5000, listenOptions =>
    {
        // Use HTTP/3
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        // listenOptions.UseHttps();
    });
    options.AddServerHeader = false;
});

builder.Host.UseSerilog();
var configuration = builder.Configuration;

var services = builder.Services;

services.AddHttpContextAccessor();
services.AddResponseCompression();

services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
});

services.AddModuleConfigure(configuration);
services.AddHealthChecks();
services.AddCustomSwaggerGen();
services.AddLogging();
var app = builder.Build();
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .CreateLogger();
}
if (app.Environment.IsProduction())
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .CreateLogger();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseSwaggerUICustom();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
