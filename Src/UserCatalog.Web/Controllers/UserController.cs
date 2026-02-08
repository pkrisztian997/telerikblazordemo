using Microsoft.AspNetCore.Mvc;
using SHD.UserCatalog.BL;
using SHD.UserCatalog.BL.Workflow.Queries;

namespace UserCatalog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IGetAllUsersQuery _getAllUsersQuery;

        public UserController(IGetAllUsersQuery getAllUsersQuery)
        {
            _getAllUsersQuery = getAllUsersQuery ?? throw new ArgumentNullException(nameof(getAllUsersQuery));
        }

        [HttpGet]
        public async Task<IEnumerable<IUser>> Users()
        {
            return await _getAllUsersQuery.GetAllUsersAsync();
        }
    }
}
