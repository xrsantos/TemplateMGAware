using Microsoft.AspNetCore.Identity;
using MGAware.Database.DTO;
using MGAware.Database.Context;

namespace MGAware.Security.JWT;

public class IdentityInitializer
{
    private readonly MGADBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityInitializer(
        MGADBContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
            if (!_roleManager.RoleExistsAsync(Constant.RolesCnt.ROLE_ACCESS_APIS!).Result)
            {
                var resultado = _roleManager.CreateAsync(
                    new IdentityRole(Constant.RolesCnt.ROLE_ACCESS_APIS!)).Result;
                if (!resultado.Succeeded)
                {
                    throw new Exception(
                        $"Erro durante a criação da role {Constant.RolesCnt.ROLE_ACCESS_APIS}.");
                }
            }
            CreateUser(
                new ApplicationUser()
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Ricardo",
                    LastName = "Alves Santos"
                }, "P@s55_12", Constant.RolesCnt.ROLE_ACCESS_APIS);

    }

    private void CreateUser(
        ApplicationUser user,
        string password,
        string? initialRole = null)
    {
        if (_userManager.FindByNameAsync(user.UserName!).Result == null)
        {
            var resultado = _userManager
                .CreateAsync(user, password).Result;

            if (resultado.Succeeded &&
                !String.IsNullOrWhiteSpace(initialRole))
            {
                _userManager.AddToRoleAsync(user, initialRole).Wait();
            }
        }
    }
}