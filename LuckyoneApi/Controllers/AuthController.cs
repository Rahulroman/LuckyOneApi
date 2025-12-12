using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static LuckyoneApi.DTOs.AuthDTOs;

namespace LuckyoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {

            
                var response = await _authService.Register(registerRequest);
                if (response == null)
                {
                    return BadRequest("User registration failed.");
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                throw ;
            }


        }



        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] LoginRequest loginRequest ) 
        {
            var response = await _authService.login(loginRequest);
            if (response == null)
            {
                return BadRequest("UserName Or Password Incorrect.");
            }
            return Ok(response);

        }






    }
}
