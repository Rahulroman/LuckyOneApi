namespace LuckyoneApi.DTOs
{
    public class AuthDTOs
    {
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class AuthResponse
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public int UserId { get; set; }
        }


    }
}
