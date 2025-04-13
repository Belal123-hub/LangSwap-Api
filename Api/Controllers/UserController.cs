using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using Swashbuckle.AspNetCore.Annotations;
using DTO;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _usersService;

        public UserController(IUserService usersService, ILogger<UserController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.", typeof(UserRegisterDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError.", typeof(Response))]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _usersService.Register(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("User with same email already exists");
            }
            catch (Exception ex)
            {
                return Problem("Something happened during users registration");
            }

            return Ok();
        }
    }
}
