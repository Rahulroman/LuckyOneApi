using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

            //var adminId =  int.Parse(User.FindFirst("UserId").Value);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _contestService.createContest(createContestRequest, adminId);

            if (response == null)
            {
                return BadRequest("Contest Not Created.");
            }

            return Ok(new { isSuccess = true, messsage = "Contest created Success", response = response });
        }


        [HttpPost]
        [Route("JoinContest")]
        public async Task<IActionResult> JoinContest([FromBody] JoinContestRequest joinContestRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isJoined = await _contestService.JoinContest(joinContestRequest.ContestId, userId);
            if (!isJoined)
            {
                return BadRequest("Failed to join contest.");
            }
            return Ok(new { isSuccess = true, message = "Successfully joined the contest." });
        }


        [HttpPost]
        [Route("DeclareWinner")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeclareWinner([FromBody] DeclareWinnerRequest declareWinnerRequest)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isDeclared = await _contestService.DeclareWinner(declareWinnerRequest, adminId);
            if (!isDeclared)
            {
                return BadRequest("Failed to declare winner.");
            }
            return Ok(new { isSuccess = true, message = "Winner declared successfully." });
        }


        [HttpGet]
        [Route("GetActiveContests")]
        public async Task<IActionResult> GetActiveContests()
        {
            var contests = await _contestService.GetActiveContests();
            return Ok(new { isSuccess = true, message = "Contest Fetch Successfully", response = contests });
        }

        [HttpGet]
        [Route("GetContestById/{contestId}")]
        public async Task<IActionResult> GetContestById(int contestId)
        {
            var contests = await _contestService.GetContestById(contestId);
            if (contests == null)
            {
                return NotFound(new { isSuccess = false, message = "Contest not found." });
            }
            return Ok(new { isSuccess = true, message = "Contest Fetch Successfully", response = contests });
        }


        [HttpGet]
        [Route("GetUserContests")]
        public async Task<IActionResult> GetUserContests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var contests = await _contestService.GetUserContest(userId);
            return Ok(new { isSuccess = true, message = "User Contests Fetch Successfully", response = contests });
        }






    }
}