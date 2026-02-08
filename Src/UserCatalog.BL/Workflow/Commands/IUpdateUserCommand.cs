using SHD.OrganizationalFramework.BL.Workflow.Commands;

namespace SHD.UserCatalog.BL.Workflow.Commands
{
    public interface IUpdateUserCommand : IAsyncCommand
    {
        void AddUserCommandItem(IUser user);
    }
}
