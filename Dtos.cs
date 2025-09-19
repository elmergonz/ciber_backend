// DTOs
record UserDto(string Username, string Password);
record UserNameDto(string Username);
record ChangePasswordDto(string Username, string OldPassword, string NewPassword);