namespace AntCommerce.Module.Modular
{
    using System.Collections.Generic;

    public static class GlobalConfiguration
    {
        public static IList<ModuleInfo> Modules { get; set; } = new List<ModuleInfo>();
    }
}