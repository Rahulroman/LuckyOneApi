using LuckyoneApi.Data;
using LuckyoneApi.DTOs;
using LuckyoneApi.Entity;
using LuckyoneApi.Services.IService;
using Microsoft.EntityFrameworkCore;
using static LuckyoneApi.DTOs.AuthDTOs;
using LuckyoneApi.Helper;

namespace LuckyoneApi.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenHelper JwtHelper;
        
        public AuthService(AppDbContext appDbContext , TokenHelper tokenHelper)
        {
            this._context = appDbContext;
            this.JwtHelper = tokenHelper;
        }

        public async Task<AuthResponse> Register(RegisterRequest registerRequest)
        {
            var existinguser = await (from U in _context.Users
                                      where U.Email == registerRequest.Email || U.Username == registerRequest.Username
                                      select U
                                      ).FirstOrDefaultAsync();

            if (existinguser != null) { return null; }

            var newUser = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = PasswordHelper.HashPassword( registerRequest.Password), // In real application, hash the password
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PhoneNumber = registerRequest.PhoneNumber
            };


          await _context.Users.AddAsync(newUser);
          await  _context.SaveChangesAsync();

            var token = JwtHelper.GenerateJwtToken(newUser.UserId, newUser.Role);

            return new AuthResponse { Username = newUser.Username, Token = token };


        }
    }
}
