using SHD.UserCatalog.BL.DataAccess;

namespace SHD.UserCatalog.BL.Workflow.Queries
{
    internal class GetAuthenticatedUserQuery : IGetAuthenticatedUserQuery
    {
        private readonly IUserRepository _userRepository;

        public GetAuthenticatedUserQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IUser?> GetAuthenticatedUserAsync(string username, string password)
        {
            return await _userRepository.GetAuthenticatedUserAsync(username, password);
        }
    }
}
