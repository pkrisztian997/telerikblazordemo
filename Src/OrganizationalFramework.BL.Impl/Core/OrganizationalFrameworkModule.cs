using Microsoft.Extensions.DependencyInjection;
using SHD.OrganizationalFramework.BL.Feedback;

namespace SHD.OrganizationalFramework.BL.Core
{
    public static class OrganizationalFrameworkModule
    {
        public static IServiceCollection AddOrganizationalFrameworkModule(this IServiceCollection services)
        {
            services.AddScoped<IFeedbackCollector, FeedbackCollector>();

            return services;
        }
    }
}
