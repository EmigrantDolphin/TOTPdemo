using System.Text;
using Google.Authenticator;
using Microsoft.EntityFrameworkCore;
using TOTPdemo.Models;
using TOTPdemo.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer("Server=localhost,1433;Database=auth;User Id=sa;Password=yourStrong(@)Ppassword;Trusted_Connection=True;Integrated security=false"));
var app = builder.Build();

app.MapPost("/api/authenticate/basic", async (BasicAuthRequest request, UserDbContext context) =>
{
    var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);
    if (user is null)
    {
        return Results.Unauthorized();
    }

    if (string.IsNullOrWhiteSpace(user.TotpSecretCode))
    {
        // return url
        var twoFactorAuthenticator = new TwoFactorAuthenticator();
        var secretKey = $"randomSecretKey-{user.Email}";
        var setupCode =
            twoFactorAuthenticator.GenerateSetupCode("TOTP Demo", user.Email, Encoding.ASCII.GetBytes(secretKey));
        var updatedUser = user with {TotpSecretCode = secretKey};
        context.Update(updatedUser);
        await context.SaveChangesAsync();

        return Results.Ok(setupCode);
    }
    
    return Results.BadRequest("User has setup TOTP. Please use authenticate/TOTP endpoint to get the token");
});

app.MapPost("/api/authenticate/totp", async (TotpAuthRequest request, UserDbContext context) =>
{
    var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email);
    if (user is null)
    {
        return Results.Unauthorized();
    }

    if (string.IsNullOrWhiteSpace(user.TotpSecretCode))
    {
        return Results.BadRequest("TOTP is not setup");
    }

    var twoFactorAuth = new TwoFactorAuthenticator();
    var isAuthenticated = twoFactorAuth.ValidateTwoFactorPIN(user.TotpSecretCode, request.TotpPassword);

    if (!isAuthenticated)
    {
        return Results.BadRequest("Bad input code");
    }

    return Results.Ok("Authentication successfull");
});

app.Run();