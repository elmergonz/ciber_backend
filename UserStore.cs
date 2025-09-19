using System.Text.Json;

public class UserStore
{
    private readonly string _filePath;
    private Dictionary<string, User> _users;

    public UserStore(string filePath = "data/users.json")
    {
        _filePath = filePath;
        _users = LoadUsers();
    }

    public bool UserExists(string username) => _users.ContainsKey(username);

    public User? GetUser(string username) =>
        _users.TryGetValue(username, out var user) ? user : null;

    public void AddUser(string username, string hash)
    {
        _users[username] = new User
        {
            Username = username,
            PasswordHash = hash,
            IsLoggedIn = false
        };
        SaveUsers();
    }

    public void UpdatePassword(string username, string newHash)
    {
        if (_users.TryGetValue(username, out var user))
        {
            user.PasswordHash = newHash;
            SaveUsers();
        }
    }

    public void SetLoginState(string username, bool isLoggedIn)
    {
        if (_users.TryGetValue(username, out var user))
        {
            user.IsLoggedIn = isLoggedIn;
            SaveUsers();
        }
    }

    private Dictionary<string, User> LoadUsers()
    {
        if (!File.Exists(_filePath))
            return new Dictionary<string, User>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Dictionary<string, User>>(json)
               ?? new Dictionary<string, User>();
    }

    private void SaveUsers()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}
