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
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

builder.Services.AddAuthentication("Bearer").AddJwtBearer();

// var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<ProductDbContext>(options =>
{
    options.UseSqlServer(connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });

    // Changing default behavior when client evaluation occurs to throw.
    // Default in EF Core would be to log a warning when client evaluation is performed.
    //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
});
builder.Services.AddStackExchangeRedisCache(options =>
 {
     options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
     options.InstanceName = "ProductCacheInstance";
 });
builder.Services.AddTransient<IProductQueryService, ProductQueryService>();
builder.Services.AddTransient<IProductCommandService, ProductCommandService>();
builder.Services.AddMediatR(typeof(QueryProductHandler));
builder.Services.AddMediatR(typeof(CreateProductHandler));
builder.Services.AddMediatR(typeof(EditProductHandler));

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
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
});

builder.Services.AddHealthChecks();
builder.Services.AddCustomSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddApiVersioningService();
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
