namespace Jericho.Models.v1.DTOs.User
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}