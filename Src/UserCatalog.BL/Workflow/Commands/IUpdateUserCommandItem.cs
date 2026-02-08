using SHD.OrganizationalFramework.BL.Workflow.Commands;

namespace SHD.UserCatalog.BL.Workflow.Commands
{
    public interface IUpdateUserCommandItem : IAsyncCommandItem
    {
        IUser? UpdatedUser { get; }
    }
}
