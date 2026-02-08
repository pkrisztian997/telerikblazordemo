using SHD.UserCatalog.BL.DataAccess;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    internal class GetUserDetailsQuery : IGetUserDetailsQuery
    {
        private readonly IUserRepository _userRepository;

        public GetUserDetailsQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IUser> GetUserDetailsAsync(Guid userId)
        {
            return await _userRepository.GetUserDetailsAsync(userId);
        }
    }
}
