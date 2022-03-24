namespace WebHost.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class UseSwaggerUIConfigurationExtensions
    {
        public static void UseSwaggerUICustom(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Developer API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}