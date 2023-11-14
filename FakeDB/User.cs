namespace FakeDB
{
    public enum UserRole
    {
        Admin,
        User
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public UserRole Role { get; set; }
    }
}
