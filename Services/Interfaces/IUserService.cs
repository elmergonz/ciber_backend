using ciber_backend.Models;

namespace ciber_backend.Services;

public interface IUserService
{
    bool Register(string username, string password);
    bool ChangePassword(string username, string oldPassword, string newPassword);
    User? Login(string username, string passwordHash);
}
