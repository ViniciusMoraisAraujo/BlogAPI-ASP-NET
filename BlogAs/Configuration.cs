namespace BlogAs;

public static class Configuration
{
    public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU="; //TOKEN
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "1234567890";
    public static SmtpConfiguration Smpt { get; set; } = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}