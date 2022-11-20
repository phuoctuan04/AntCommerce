using System.IO.Compression;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using AntCommerce.Module.Web.Middlewares;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Seq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MassTransit;

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

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder.Services.AddDbContextPool<ProductDbContext>(options =>
// {
//     options.UseSqlServer(connectionString,
//         sqlServerOptionsAction: sqlOptions =>
//         {
//             //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
//             sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
//         });

//     // Changing default behavior when client evaluation occurs to throw.
//     // Default in EF Core would be to log a warning when client evaluation is performed.
//     //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
// });
builder.Services.AddStackExchangeRedisCache(options =>
 {
     options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
     options.InstanceName = "ProductCacheInstance";
 });

builder.Services.AddProblemDetails();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddApiVersioningService();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
    // x.AddConsumer<SubmitOrderConsumer>(typeof(SubmitOrderConsumerDefinition));
});

builder.Services.AddOptions<MassTransitHostOptions>()
                .Configure(options =>
                {
                    // if specified, waits until the bus is started before
                    // returning from IHostedService.StartAsync
                    // default is false
                    options.WaitUntilStarted = true;

                    // if specified, limits the wait time when starting the bus
                    options.StartTimeout = TimeSpan.FromSeconds(10);

                    // if specified, limits the wait time when stopping the bus
                    options.StopTimeout = TimeSpan.FromSeconds(30);
                });

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUICustom();
app.MapHealthChecks("/health");

app.Run();
