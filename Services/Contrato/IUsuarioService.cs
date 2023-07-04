using System.Security.Claims;

namespace Servirform.Services.Contrato;

public interface IUsuarioService
{
    Task<bool> ValidarUsuario(ClaimsPrincipal UserClains, string id);
}