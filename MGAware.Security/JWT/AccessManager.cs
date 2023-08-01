using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using MGAware.Database.DTO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace MGAware.Security.JWT;

public class AccessManager
{
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private SigningConfigurations _signingConfigurations;
    private TokenConfigurationsDto _tokenConfigurations;

    public AccessManager(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        SigningConfigurations signingConfigurations,
        TokenConfigurationsDto tokenConfigurations)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _signingConfigurations = signingConfigurations;
        _tokenConfigurations = tokenConfigurations;
    }

    public bool ValidateCredentials(UserDto user)
    {

        if (user is not null && !String.IsNullOrWhiteSpace(user.UserID))
        {
            var userIdentity = _userManager
                .FindByNameAsync(user.UserID).Result;
            if (userIdentity is not null)
            {
                var resultadoLogin = _signInManager
                    .CheckPasswordSignInAsync(userIdentity, user.Password!, false)
                    .Result;
                if (resultadoLogin.Succeeded)
                {
                    return _userManager.IsInRoleAsync(
                        userIdentity, Constant.RolesCnt.ROLE_ACCESS_APIS!).Result;
                }
            }
        }

        return false;
    }

    public TokenDto GenerateToken(UserDto user)
    {
        ClaimsIdentity identity = new(
            new GenericIdentity(user.UserID!, "Login"),
            new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID!)
            }
        );

        DateTime dateCreate = DateTime.Now;
        DateTime dateExp = dateCreate +
            TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _tokenConfigurations.Issuer,
            Audience = _tokenConfigurations.Audience,
            SigningCredentials = _signingConfigurations.SigningCredentials,
            Subject = identity,
            NotBefore = dateCreate,
            Expires = dateExp
        });
        var token = handler.WriteToken(securityToken);

        return new()
        {
            Authenticated = true,
            Created = dateCreate.ToString("o"),
            Expiration = dateExp.ToString("o"),
            AccessToken = token,
            Message = "OK"
        };
    }

    public async Task<bool> RegisterUser(RegisterUserDto usuario, ApplicationUser user)
    {
        var result = await _userManager.CreateAsync(user, usuario.Password);

        if (result.Succeeded)
        {
            _userManager.AddToRoleAsync(user, Constant.RolesCnt.ROLE_ACCESS_APIS!).Wait();
            return true;
        }
        return false;
    }

}
