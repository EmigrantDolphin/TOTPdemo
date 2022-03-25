namespace TOTPdemo.Domain;

public record User(string Email, string Password, string FirstName, string LastName, string TotpSecretCode);