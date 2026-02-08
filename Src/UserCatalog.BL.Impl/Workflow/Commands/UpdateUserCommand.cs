using SHD.OrganizationalFramework.BL.Workflow.Commands;
using SHD.UserCatalog.BL.DataAccess;

namespace SHD.UserCatalog.BL.Workflow.Commands
{
    internal class UpdateUserCommand : AAsyncCommand, IUpdateUserCommand
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommand(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void AddUserCommandItem(IUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            AddCommandItem(new UpdateUserCommandItem(user, _userRepository));
        }
    }
}
