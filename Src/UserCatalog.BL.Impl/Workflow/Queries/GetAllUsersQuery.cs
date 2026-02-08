using SHD.UserCatalog.BL.DataAccess;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    internal class GetAllUsersQuery : IGetAllUsersQuery
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<IUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
