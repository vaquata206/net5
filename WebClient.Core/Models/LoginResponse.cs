namespace WebClient.Core.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public AccountInfo Account { get; set; }
    }
}
