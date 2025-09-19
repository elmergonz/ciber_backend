public class User
{
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsLoggedIn { get; set; } = false;
}
