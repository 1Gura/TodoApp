namespace TodoApp.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public string UserEmail { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
