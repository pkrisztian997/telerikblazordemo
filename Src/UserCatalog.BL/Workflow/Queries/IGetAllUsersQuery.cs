using SHD.OrganizationalFramework.BL.Workflow.Queries;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    public interface IGetAllUsersQuery : IAsyncQuery
    {
        Task<IEnumerable<IUser>> GetAllUsersAsync();
    }
}
