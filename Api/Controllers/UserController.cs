using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using Swashbuckle.AspNetCore.Annotations;
using DTO;
using Microsoft.IdentityModel.Tokens;


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

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Log in to the system.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.", typeof(UserRegisterDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError.", typeof(Response))]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto model)
        {
            try
            {
                return Ok(await _usersService.Login(model));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Login or maybe password invalid!");
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh token.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.", typeof(RefreshCredentialsDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError.", typeof(Response))]
        public async Task<IActionResult> Refresh([FromBody] RefreshCredentialsDto model)
        {
            try
            {
                return Ok(await _usersService.Refresh(model.RefreshToken));
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized("Token is expired!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("User is not found!");
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }
    }
}
