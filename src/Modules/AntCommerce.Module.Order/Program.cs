using System.IO.Compression;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using AntCommerce.Module.Web.Middlewares;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Seq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddCustomSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});
builder.Services.AddProblemDetails();
// Configure logging
// https://www.meziantou.net/monitoring-a-dotnet-application-using-opentelemetry.htm

builder.Logging.AddOpenTelemetry(builder =>
{
     
    builder.IncludeFormattedMessage = true;
    builder.IncludeScopes = true;
    builder.ParseStateValues = true;
    // builder.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
});

builder.Host.UseSerilog((context, loggerConfig) 
    => loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// builder.Host.UseSerilog();
// Log.Logger = new LoggerConfiguration()
//                 .MinimumLevel.Information()
//                 .Enrich.FromLogContext()
//                 .Enrich.WithExceptionDetails()
//                 .Enrich.WithMachineName()
//                 .WriteTo.Console()
//                 .WriteTo.Seq("http://localhost:5341", Serilog.Events.LogEventLevel.Information)
//                 .CreateLogger();

app.UseRouting();
app.UseResponseCompression();
app.UseExceptionHandler();
app.UseRateLimiter();
app.MapControllers();
app.UseSwaggerUICustom();
app.MapHealthChecks("/health");
// app.MapGet("/", () => "Hello World!");

app.Run();
