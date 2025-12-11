using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LuckyoneApi.DTOs.AuthDTOs;

namespace LuckyoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[Route("register")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest )
        //{

        //    var response = await _authService.Register(registerRequest);
        //    if(response == null)
        //    {
        //        return BadRequest("User registration failed.");
        //    }

        //    return Ok(response);
        //}

        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {

            var response = "fdf";
           

            return Ok(response);
        }


    }
}
