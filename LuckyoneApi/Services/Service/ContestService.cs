using LuckyoneApi.Data;
using LuckyoneApi.DTOs;
using LuckyoneApi.Entity;
using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LuckyoneApi.DTOs.ContestDTOs;

namespace LuckyoneApi.Services.Service
{
    public class ContestService : IContestService
    {

        private readonly AppDbContext _context;
        private readonly PointService _pointService;

        public ContestService(AppDbContext context,PointService pointService)
        {
            _context = context;
            _pointService = pointService;
        }


        public async Task<ContestResponse> createContest(CreateContestRequest createContestRequest, int adminID)
        {
            var ContestCode = "CON" + DateTime.Now.Ticks.ToString();

            var contest = new Contest
            {
                ContestName = createContestRequest.ContestName,
                ContestCode = ContestCode,
                //Description = createContestRequest.Description,
                EntryPoints = (int)createContestRequest.EntryFee,
                TotalSlots = createContestRequest.MaxParticipants,
                FilledSlots = 0,
                StartTime = createContestRequest.StartTime,
                EndTime = createContestRequest.EndTime,
                Status = "Scheduled",
                WinningPoints = (int)createContestRequest.PrizePool,
                //IsDrawCompleted = false,
                CreatedBy = adminID,
                CreatedAt = DateTime.UtcNow
            };


            await _context.Contests.AddAsync(contest);
            await _context.SaveChangesAsync();

            return new ContestResponse 
            {
                ContestId = contest.ContestId,
                ContestName = contest.ContestName,
                ContestCode = contest.ContestCode,
                EntryFee = createContestRequest.EntryFee,
                MaxParticipants = createContestRequest.MaxParticipants,
                CurrentParticipants = contest.FilledSlots,
                StartTime = contest.StartTime,
                EndTime = contest.EndTime,
                Status = contest.Status,
                PrizePool = createContestRequest.PrizePool,
                IsDrawCompleted = false
            };

        }


        public async Task<bool> JoinContest(int contestId, int userId)
        {
           
               using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                var contest = await (from C in _context.Contests
                                     where C.ContestId == contestId
                                     select C).FirstOrDefaultAsync();
                if (contest == null) {     return false; }

                var existingSlot = (from C in _context.ContestSlots
                                    where C.ContestId == contestId && C.UserId == userId
                                    select C).FirstOrDefaultAsync();
                if (existingSlot == null) { return false; }

                var deductionResult = await _pointService.DeductPoints(userId, contest.EntryPoints, "ContestJoin", contestId);

                if (!deductionResult)
                    return false;

                // Generate slot number
                var slotNumber = $"S{contest.FilledSlots + 1:000}";

                var contestSlot = new ContestSlot
                {
                    ContestId = contestId,
                    UserId = userId,
                    SlotNumber = slotNumber,
                    JoinedAt = DateTime.UtcNow,
                    IsWinner = false
                };
                await _context.ContestSlots.AddAsync(contestSlot);

                // Update contest filled slots
                contest.FilledSlots++;
                if (contest.FilledSlots == contest.TotalSlots)
                {
                    contest.Status = "Ready";
                }
                _context.Contests.Update(contest);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;

            }
                catch (Exception)
                {
                await transaction.RollbackAsync();
                return false;
            }
             
           
        }


        public async  Task<bool> DeclareWinner(DeclareWinnerRequest request, int adminId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var contest = await (from c in _context.Contests
                                     where c.ContestId == request.ContestId
                                     select c).FirstOrDefaultAsync();
                if (contest == null || contest.Status != "Ready")
                    return false;

                // Find winning slot
                var winningSlot = await (from cs in _context.ContestSlots
                                         where cs.ContestId == request.ContestId
                                               && cs.SlotNumber == request.WinningSlotNumber
                                         select cs).FirstOrDefaultAsync();

                if (winningSlot == null)
                    return false;

                // Create draw record
                var draw = new ContestDraw
                {
                    ContestId = request.ContestId,
                    DrawTime = DateTime.UtcNow,
                    WinningSlotNumber = request.WinningSlotNumber,
                    WinnerUserId = winningSlot.UserId,
                    Status = "Completed",
                    DeclaredBy = adminId,
                    DeclaredAt = DateTime.UtcNow
                };

                await _context.ContestDraws.AddAsync(draw);
                await _context.SaveChangesAsync();

                // Create winner record
                var winner = new ContestWinner
                {
                    ContestId = request.ContestId,
                    UserId = winningSlot.UserId,
                    ContestDrawId = draw.DrawId,
                    WonPoints = contest.WinningPoints,
                    WonAt = DateTime.UtcNow
                };

                await _context.ContestWinners.AddAsync(winner);


                // Update slot as winner
                winningSlot.IsWinner = true;
                _context.ContestSlots.Update(winningSlot);

                // Update contest status
                contest.Status = "Completed";
                contest.DrawTime = DateTime.UtcNow;
                _context.Contests.Update(contest);

                // Credit points to winner
                await _pointService.CreditPoints(winningSlot.UserId, contest.WinningPoints,"ContestWin",request.ContestId);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log exception
                return false;
            }
        }

        public async Task<List<ContestResponse>> GetActiveContests()
        {
            try
            {
                var contests = await (from c in _context.Contests
                                      where c.IsActive
                                            && c.Status != "Completed"
                                            && c.Status != "Cancelled"
                                            && c.StartTime <= DateTime.UtcNow
                                            && c.EndTime >= DateTime.UtcNow
                                      orderby c.StartTime
                                      select new ContestResponse
                                      {
                                          ContestId = c.ContestId,
                                          ContestName = c.ContestName,
                                          ContestCode = c.ContestCode,
                                          EntryFee = c.EntryPoints,
                                          MaxParticipants = c.TotalSlots,
                                          CurrentParticipants = c.FilledSlots,
                                          StartTime = c.StartTime,
                                          EndTime = c.EndTime,
                                          Status = c.Status,
                                          PrizePool = c.WinningPoints,
                                         // IsDrawCompleted = c.IsDrawCompleted
                                      }).ToListAsync();

                return contests;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ContestResponse> GetContestById(int contestId)
        {
            try
            {
                var contest = await (from c in _context.Contests
                                    where c.ContestId == contestId
                                    select new ContestResponse
                                    {
                                        ContestId = c.ContestId,
                                        ContestName = c.ContestName,
                                        ContestCode = c.ContestCode,
                                        EntryFee = c.EntryPoints,
                                        MaxParticipants = c.TotalSlots,
                                        CurrentParticipants = c.FilledSlots,
                                        StartTime = c.StartTime,
                                        EndTime = c.EndTime,
                                        Status = c.Status,
                                        PrizePool = c.WinningPoints,
                                        //IsDrawCompleted = c.IsDrawCompleted
                                    } ).FirstOrDefaultAsync();

                return contest != null ? contest : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ContestResponse>> GetUserContest(int userId)
        {
            try
            {
                var contests = await (from cs in _context.ContestSlots
                                     join c in _context.Contests on cs.ContestId equals c.ContestId
                                     where cs.UserId == userId
                                     orderby c.StartTime descending
                                     select c).Distinct().ToListAsync();


                var contestList = contests.Select(c => new ContestResponse
                {

                    ContestId = c.ContestId,
                    ContestName = c.ContestName,
                    ContestCode = c.ContestCode,
                    EntryFee = c.EntryPoints,
                    MaxParticipants = c.TotalSlots,
                    CurrentParticipants = c.FilledSlots,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    Status = c.Status,
                    PrizePool = c.WinningPoints,
                    // IsDrawCompleted = c.IsDrawCompleted
                }).ToList();

                return contestList;



            }
            catch (Exception)
            {

                throw;
            }
        }

       
    }
}
