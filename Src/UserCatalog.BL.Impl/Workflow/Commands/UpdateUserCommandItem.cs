using SHD.OrganizationalFramework.BL.Workflow.Commands;
using SHD.UserCatalog.BL.DataAccess;

namespace SHD.UserCatalog.BL.Workflow.Commands
{
    internal class UpdateUserCommandItem : AAsyncCommandItem, IUpdateUserCommandItem
    {
        public IUser? UpdatedUser { get; private set; }

        private readonly IUser _userToUpdate;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandItem(IUser userToUpdate, IUserRepository userRepository)
        {
            _userToUpdate = userToUpdate ?? throw new ArgumentNullException(nameof(userToUpdate));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public override async ValueTask ExecuteAsync()
        {
            UpdatedUser = await _userRepository.UpdateUserAsync(_userToUpdate);
        }
    }
}
