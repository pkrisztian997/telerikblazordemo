using Microsoft.AspNetCore.Mvc;
using SHD.UserCatalog.BL;

namespace UserCatalog.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet("users")]
        public IEnumerable<IUser> Users()
        {


            //return Enumerable.Range(1, 15).Select(index => new WeatherDto
            //{
            //    Date = startDate.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = summaries[Random.Shared.Next(summaries.Length)]
            //});
            return Enumerable.Empty<IUser>();
        }
    }
}
