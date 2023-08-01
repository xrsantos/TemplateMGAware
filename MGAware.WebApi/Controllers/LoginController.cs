using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MGAware.Security.JWT;
using MGAware.Database.DTO;

namespace MGAware.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoginController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public ActionResult<TokenDto> Post(
        [FromBody] UserDto usuario,
        [FromServices] ILogger<LoginController> logger,
        [FromServices] AccessManager accessManager)
    {
        logger.LogInformation($"Recebida solicitação para o usuário: {usuario?.UserID}");

        if (usuario is not null && accessManager.ValidateCredentials(usuario))
        {
            logger.LogInformation($"Sucesso na autenticação do usuário: {usuario.UserID}");
            return accessManager.GenerateToken(usuario);
        }
        else
        {
            logger.LogError($"Falha na autenticação do usuário: {usuario?.UserID}");
            return new UnauthorizedResult();
        }
    }

    [Route("Register")]
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<bool> Register(
        [FromBody] RegisterUserDto usuario,
        [FromServices] ILogger<LoginController> logger,
        [FromServices] AccessManager accessManager)
    {
        return await accessManager.RegisterUser(usuario, usuario.GetUser());
    }

}