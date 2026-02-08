using SHD.UserCatalog.BL.Core;

namespace UserCatalog.Web.Core
{
    internal static class UserCatalogWebModule
    {
        public static IServiceCollection AddUserCatalogWebModule(this IServiceCollection services, string dataFilePath)
        {
            ArgumentNullException.ThrowIfNull(dataFilePath);

            services.AddUserCatalogModule(dataFilePath);

            return services;
        }
    }
}
