using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHD.UserCatalog.BL;
using SHD.UserCatalog.BL.Workflow.Commands;
using SHD.UserCatalog.BL.Workflow.Queries;
using System.Security.Claims;
using UserCatalog.Web.Components.Pages.ViewModels;
using UserCatalog.Web.Components.Pages.ViewModels.Converters;

namespace UserCatalog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetAllUsersQuery _getAllUsersQuery;
        private readonly IGetAuthenticatedUserQuery _getAuthenticatedUserQuery;
        private readonly IGetUserDetailsQuery _getUserDetailsQuery;
        private readonly IUpdateUserCommand _updateUserCommand;
        private readonly IUserProxyConverter _userProxyConverter;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            IGetAllUsersQuery getAllUsersQuery,
            IGetAuthenticatedUserQuery getAuthenticatedUserQuery,
            IGetUserDetailsQuery getUserDetailsQuery,
            IUpdateUserCommand updateUserCommand,
            IUserProxyConverter userProxyConverter)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _getAllUsersQuery = getAllUsersQuery ?? throw new ArgumentNullException(nameof(getAllUsersQuery));
            _getAuthenticatedUserQuery = getAuthenticatedUserQuery;
            _getUserDetailsQuery = getUserDetailsQuery ?? throw new ArgumentNullException(nameof(getUserDetailsQuery));
            _updateUserCommand = updateUserCommand ?? throw new ArgumentNullException(nameof(updateUserCommand));
            _userProxyConverter = userProxyConverter ?? throw new ArgumentNullException(nameof(userProxyConverter));
        }

        [HttpGet]
        public async Task<IEnumerable<IUser>> Users()
        {
            return await _getAllUsersQuery.GetAllUsersAsync();
        }

        [HttpGet("{userId}")]
        public async Task<IUser> GetUsersById(Guid userId)
        {
            return await _getUserDetailsQuery.GetUserDetailsAsync(userId);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDetailFormModel userDetailFormModel)
        {
            var authenticatedUser = _getAuthenticatedUserQuery.GetAuthenticatedUserAsync(userDetailFormModel.Username, userDetailFormModel.CurrentPassword);
            if (authenticatedUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var user = _userProxyConverter.ConvertUserDetailFormModelToDomainModel(userDetailFormModel);
            _updateUserCommand.AddUserCommandItem(user);
            await _updateUserCommand.ExecuteAsync();

            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Password is required.");
            }

            var authenticatedUser = await _getAuthenticatedUserQuery.GetAuthenticatedUserAsync(request.Username, request.Password);
            if (authenticatedUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim("usr", request.Username)
            };
            var identity = new ClaimsIdentity(claims, "cookie");
            var user = new ClaimsPrincipal(identity);

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            await httpContext.SignInAsync("cookie", user);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookie");
            return Ok();
        }
    }
}
