using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform.Models.JWT;
using Servirform.Services.Contrato;

namespace Servirform.Services;

public class UsuarioService : IUsuarioService
{
    private readonly ServinformContext _context;

    public UsuarioService(ServinformContext context)
    {
        _context = context;
    }

    public async Task<bool> ValidarUsuario(ClaimsPrincipal UserClaims, string id)
    {
        try
        {
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            Console.WriteLine(EmailUser);

            // Verificar si el rol de la autorizacion es administrador o usuario
            if (RoleUser == Roles.administrador.ToString())
            {
                // Si es "administrador" verificar, darle permiso de acceder de todo los usuarios
                return await _context.Usuarios.AnyAsync(u => u.Email == EmailUser && u.IdRol == (int)Roles.administrador);
            }
            else
            {
                // Si es "usuario" verificar con el id(email)
                return id == EmailUser;
            }
        }
        catch
        {
            throw new TaskCanceledException();
        }
    }
}