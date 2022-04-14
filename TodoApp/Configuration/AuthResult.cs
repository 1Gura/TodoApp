namespace TodoApp.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public bool Success { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
