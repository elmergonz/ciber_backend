var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register persistence service
builder.Services.AddSingleton<UserStore>();
builder.Services.AddSingleton<HashSet<string>>(); // sessions

// Allow any origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Always enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
    c.RoutePrefix = "swagger"; // http://host:port/swagger
});

// Enable CORS
app.UseCors();

// Register
app.MapPost("/register", (UserDto dto, UserStore userStore) =>
{
    if (userStore.UserExists(dto.Username))
        return Results.BadRequest("User already exists.");

    var hash = PasswordHelper.HashPassword(dto.Password);
    userStore.AddUser(dto.Username, hash);

    return Results.Ok("User registered successfully.");
});

// Login
app.MapPost("/login", (UserDto dto, UserStore userStore) =>
{
    var user = userStore.GetUser(dto.Username);
    if (user is null)
        return Results.BadRequest("Invalid credentials.");

    if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
        return Results.BadRequest("Invalid credentials.");

    userStore.SetLoginState(dto.Username, true);
    return Results.Ok($"User {dto.Username} logged in.");
});

// Change password
app.MapPost("/change-password", (ChangePasswordDto dto, UserStore userStore) =>
{
    var user = userStore.GetUser(dto.Username);
    if (user is null)
        return Results.BadRequest("User not found.");

    if (!user.IsLoggedIn)
        return Results.BadRequest("User not logged in.");

    if (!PasswordHelper.VerifyPassword(dto.OldPassword, user.PasswordHash))
        return Results.BadRequest("Old password is incorrect.");

    userStore.UpdatePassword(dto.Username, PasswordHelper.HashPassword(dto.NewPassword));
    return Results.Ok("Password changed successfully.");
});

// Logout
app.MapPost("/logout", (UserNameDto dto, UserStore userStore) =>
{
    var user = userStore.GetUser(dto.Username);
        
    if (user is null || !user.IsLoggedIn)
        return Results.BadRequest("User not logged in.");

    userStore.SetLoginState(dto.Username, false);
    return Results.Ok("Logged out successfully.");
});

// Endpoint to check state
app.MapGet("/users/{username}", (string username, UserStore userStore) =>
{
    var user = userStore.GetUser(username);
    return user is null
        ? Results.NotFound("User not found.")
        : Results.Ok(new { user.Username, user.IsLoggedIn });
});

app.Run();
