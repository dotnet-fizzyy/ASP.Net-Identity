using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApi.PL.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterUser()
        {
            return CreatedAtAction(nameof(RegisterUser), null);
        }
    }
}