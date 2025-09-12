using ciber_backend.Models;
using ciber_backend.Data;

namespace ciber_backend.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public bool Register(string username, string passwordHash)
    {
        if (_context.Users.Any(u => u.Username == username))
            return false;

        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public bool ChangePassword(string username, string oldPasswordHash, string newPasswordHash)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null) return false;

        if (user.PasswordHash != oldPasswordHash) return false;

        user.PasswordHash = newPasswordHash;
        _context.SaveChanges();
        return true;
    }

    public User? Login(string username, string passwordHash)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null) return null;

        return user.PasswordHash == passwordHash ? user : null;
    }
}
