namespace AntCommerce.Module.GraphQL
{
    using AntCommerce.Module.Modular;
    using AntCommerce.Module.GraphQL.Contexts;
    using AntCommerce.Module.GraphQL.Services;
    using HotChocolate.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class ModuleInitializer : IModuleInitializer
    {
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            throw new System.NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration["ConnectionStrings:ProductDbContext"];
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

            services.AddGraphQLServer();
        }
    }
}