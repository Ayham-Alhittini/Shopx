using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.Helper;

namespace Shopx.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
