using SHD.UserCatalog.BL.Core;
using UserCatalog.Web.Components.Pages.ViewModels.Converters;

namespace UserCatalog.Web.Core
{
    internal static class UserCatalogWebModule
    {
        public static IServiceCollection AddUserCatalogWebModule(this IServiceCollection services, string dataFilePath)
        {
            ArgumentNullException.ThrowIfNull(dataFilePath);

            services.AddUserCatalogModule(dataFilePath);

            services.AddTransient<UserProxyConverter>();

            return services;
        }
    }
}
