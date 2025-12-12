using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LuckyoneApi.DTOs.ContestDTOs;

namespace LuckyoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContestController : ControllerBase
    {
        private readonly IContestService _contestService;

        public ContestController(IContestService contestService)
        {
            _contestService = contestService;
        }


        [HttpPost]
        [Route("CreateContest")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateContest([FromBody] CreateContestRequest createContestRequest)
        {

            var adminId =  int.Parse(User.FindFirst("UserId").Value);

            //var response = await _contestService.CreateContestAsync(createContestRequest , adminId);


            return Ok();
        }


    }
}
