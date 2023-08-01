namespace MGAware.Database.DTO;

public class RegisterUserDto
{
    public string? UserName { get; set; }
    public required string Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    public ApplicationUser GetUser()
    {
        var user = new ApplicationUser()
        {
            UserName = this.UserName,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
        };
        return user;
    }
}