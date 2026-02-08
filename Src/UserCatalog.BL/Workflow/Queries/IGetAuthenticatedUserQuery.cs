using SHD.OrganizationalFramework.BL.Workflow.Queries;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    public interface IGetAuthenticatedUserQuery : IAsyncQuery
    {
        Task<IUser?> GetAuthenticatedUserAsync(string username, string password);
    }
}
