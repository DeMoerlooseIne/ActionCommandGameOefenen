using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api
{
	[Authorize]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
    }
}
