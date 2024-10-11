using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.AuthorizationModels;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await authServices.RegisterAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPost("token")]
        public async Task<IActionResult> GrtTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await authServices.GetTokenAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(new{Your_Token = result.Token, User_Roles = result.Roles});
        }
        [HttpPost("addRoleToUser")]
        public async Task<IActionResult> AddRoleAsync([FromBody]AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await authServices.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(result);
        }
    }
}
