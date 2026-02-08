using Microsoft.Extensions.DependencyInjection;
using SHD.UserCatalog.BL.DataAccess;
using SHD.UserCatalog.BL.Workflow.Commands;
using SHD.UserCatalog.BL.Workflow.Queries;

namespace SHD.UserCatalog.BL.Core
{
    public static class UserCatalogModule
    {
        public static IServiceCollection AddUserCatalogModule(this IServiceCollection services, string dataFilePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dataFilePath);

            AddCommands(services);
            AddQueries(services);

            AddRepositories(services, dataFilePath);

            return services;
        }

        private static void AddCommands(IServiceCollection services)
        {
            services.AddTransient<IUpdateUserCommand, UpdateUserCommand>();
        }

        private static void AddQueries(IServiceCollection services)
        {
            services.AddTransient<IGetAllUsersQuery, GetAllUsersQuery>();
            services.AddTransient<IGetAuthenticatedUserQuery, GetAuthenticatedUserQuery>();
            services.AddTransient<IGetUserDetailsQuery, GetUserDetailsQuery>();
        }

        private static void AddRepositories(IServiceCollection services, string dataFilePath)
        {
            services.AddTransient<IUserRepository>(u => new UserRepository(dataFilePath));
        }
    }
}
