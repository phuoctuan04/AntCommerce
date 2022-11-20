using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.RateLimiting;
using AntCommerce.Module.Product.Contexts;
using AntCommerce.Module.Product.Events;
using AntCommerce.Module.Product.Services;
using AntCommerce.Module.Web.Middlewares;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel().ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        // Use HTTP/3
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        // listenOptions.UseHttps();
    });
    options.AddServerHeader = false;
});


builder.Services.AddProblemDetails();

var services = builder.Services;

var connection = builder.Configuration["ConnectionStrings:ProductDbContext"];
services.AddDbContextPool<ProductDbContext>(options =>
{
    options.UseSqlServer(connection,
        sqlServerOptionsAction: sqlOptions =>
        {

            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });

    // Changing default behavior when client evaluation occurs to throw.
    // Default in EF Core would be to log a warning when client evaluation is performed.
    //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
});

services.AddTransient<IProductQueryService, ProductQueryService>();
services.AddTransient<IProductCommandService, ProductCommandService>();
services.AddMediatR(typeof(QueryProductHandler));
services.AddMediatR(typeof(CreateProductHandler));
services.AddMediatR(typeof(EditProductHandler));

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

services.AddHealthChecks();
services.AddCustomSwaggerGen();
services.AddLogging();
services.AddApiVersioningService();
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

builder.Host.UseSerilog((context, loggerConfig)
    => loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
app.UseResponseCompression();
app.UseRouting();
app.UseRateLimiter();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseSwaggerUICustom();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
