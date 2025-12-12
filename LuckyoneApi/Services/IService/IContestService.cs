using LuckyoneApi.DTOs;
using static LuckyoneApi.DTOs.ContestDTOs;

namespace LuckyoneApi.Services.IService
{
    public interface IContestService
    {
        Task<ContestResponse> createContest(CreateContestRequest createContestRequest , int adminId);
        Task<bool> JoinContest(int contestId, int userId);

        Task<bool> DeclareWinner(DeclareWinnerRequest request, int adminId);

        Task<List<ContestResponse>> GetActiveContests ();
        Task<ContestResponse> GetContestById(int contestId);

        Task<List<ContestResponse>> GetUserContest(int userId);


    }
}
