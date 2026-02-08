using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHD.UserCatalog.BL;
using SHD.UserCatalog.BL.Workflow.Queries;
using System.Security.Claims;

namespace UserCatalog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IGetAllUsersQuery _getAllUsersQuery;
        private readonly IGetAuthenticatedUserQuery _getAuthenticatedUserQuery;
        private readonly IGetUserDetailsQuery _getUserDetailsQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(
            IGetAllUsersQuery getAllUsersQuery,
            IGetAuthenticatedUserQuery getAuthenticatedUserQuery,
            IGetUserDetailsQuery getUserDetailsQuery,
            IHttpContextAccessor httpContextAccessor)
        {
            _getAllUsersQuery = getAllUsersQuery ?? throw new ArgumentNullException(nameof(getAllUsersQuery));
            _getAuthenticatedUserQuery = getAuthenticatedUserQuery;
            _getUserDetailsQuery = getUserDetailsQuery ?? throw new ArgumentNullException(nameof(getUserDetailsQuery));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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

        public sealed class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
