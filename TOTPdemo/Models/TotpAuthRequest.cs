namespace TOTPdemo.Models;

public record TotpAuthRequest(string Email, string TotpPassword);