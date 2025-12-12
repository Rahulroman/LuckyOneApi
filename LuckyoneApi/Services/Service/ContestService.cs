using LuckyoneApi.Data;
using LuckyoneApi.DTOs;
using LuckyoneApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using static LuckyoneApi.DTOs.ContestDTOs;

namespace LuckyoneApi.Services.Service
{
    public class ContestService : IContestService
    {

        private readonly AppDbContext _context;

        public ContestService(AppDbContext context)
        {
            _context = context;
        }

      
        //public Task<CreateContestRequest> createContestRequest(CreateContestRequest createContestRequest , int adminID)
        //{
           
        //}
    }
}
