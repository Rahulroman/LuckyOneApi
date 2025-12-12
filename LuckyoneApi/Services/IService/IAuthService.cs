using static LuckyoneApi.DTOs.AuthDTOs;

namespace LuckyoneApi.Services.IService
{
    public interface IAuthService
    {
       Task<AuthResponse> Register(RegisterRequest registerRequest);
        Task<AuthResponse> login(LoginRequest loginRequest);
    }
}
