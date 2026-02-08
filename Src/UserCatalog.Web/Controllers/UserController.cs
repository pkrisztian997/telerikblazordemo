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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IGetAllUsersQuery getAllUsersQuery, IHttpContextAccessor httpContextAccessor)
        {
            _getAllUsersQuery = getAllUsersQuery ?? throw new ArgumentNullException(nameof(getAllUsersQuery));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpGet]
        public async Task<IEnumerable<IUser>> Users()
        {
            return await _getAllUsersQuery.GetAllUsersAsync();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<string> Login([FromForm] LoginRequest request)
        {
            var claims = new List<Claim>()
            {
                new Claim("usr", request?.Username ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, "cookie");
            var user = new ClaimsPrincipal(identity);

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            await httpContext.SignInAsync("cookie", user);

            return "ok";
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
