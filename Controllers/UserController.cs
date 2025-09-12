using Microsoft.AspNetCore.Mvc;
using ciber_backend.Models;
using ciber_backend.Services;

namespace ciber_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var success = _userService.Register(request.Username, request.Password);
        if (!success)
            return BadRequest("Username already exists.");

        return Ok("User registered successfully.");
    }

    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var success = _userService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
        if (!success)
            return BadRequest("Invalid username or password.");

        return Ok("Password changed successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.Login(request.Username, request.Password);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        return Ok(new { Message = "Login successful", UserId = user.Id, user.Username });
    }

}
