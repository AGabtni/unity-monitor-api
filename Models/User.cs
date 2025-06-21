namespace Unity.Monitoring.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime? LastLoginUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
