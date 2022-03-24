namespace AntCommerce.Module.Modular.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigureServicesExtension
    {
        public static void AddModuleConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var dll in Directory.GetFiles(path, "AntCommerce.Module.*"))
            {
                if (dll.EndsWith(".dll"))
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dll);
                    var moduleInfo = new ModuleInfo
                    {
                        Assembly = Assembly.Load(assemblyName)
                    };
                    GlobalConfiguration.Modules.Add(moduleInfo);
                }
            }

            foreach (var module in GlobalConfiguration.Modules)
            {
                var moduleInitializerType = module.Assembly.GetTypes()
                   .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModuleInitializer)))
                {
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.ConfigureServices(services, configuration);
                }
            }
        }
    }
}