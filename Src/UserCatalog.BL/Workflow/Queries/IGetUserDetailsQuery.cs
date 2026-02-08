using SHD.OrganizationalFramework.BL.Workflow.Queries;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    public interface IGetUserDetailsQuery : IAsyncQuery
    {
        Task<IUser> GetUserDetailsAsync(Guid userId);
    }
}
