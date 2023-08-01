using Microsoft.AspNetCore.Identity;
namespace MGAware.Database.DTO;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get;  set; }
}