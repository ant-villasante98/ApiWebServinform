using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servirform.Helpers;
using Servirform.Models.DataModels;
using Servirform.DataAcces;
using Servirform.Models.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Servirform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly JwtSettings _jwtSettings;

        public AccountController(ServinformContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> PostLogin(UserLogins userLogins)
        {
            var Token = new UserTokens();

            // Conprobamos que la tabla usuarios no este vacia
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            // Buscamos el usuario solicitado
            var searchUser = await _context.Usuarios.Where(user => user.Email == userLogins.UserEmail && user.Password == userLogins.Password).Include(u => u.IdRolNavigation).FirstOrDefaultAsync();
            // En caso de no haber ninguna concidencia se informamos el resultado
            if (searchUser == null)
            {
                return BadRequest("Wrong password or not found user");
            }
            Console.WriteLine($"{searchUser.Nombre}, {searchUser.Apellido}, {searchUser.Email}, {searchUser.IdRolNavigation.Nombre}");

            if (searchUser.IdRolNavigation.Nombre == null)
            {
                return BadRequest("Wrong password or not found user");
            }
            string RolUser = searchUser.IdRolNavigation.Nombre;

            Token = JwtHelpers.GetTokenKey(new UserTokens()
            {
                UserEmail = searchUser.Email,
                Rol = RolUser
            }, _jwtSettings);

            return Ok(Token);
        }
        [HttpGet]
        [Route("VerifyToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult> VerificarSesion()
        {
            ClaimsPrincipal UserClaims = this.User;
            var dateExpiredClaim = UserClaims.FindFirstValue(ClaimTypes.Expiration);
            var emailUser = UserClaims.FindFirstValue(ClaimTypes.Email);
            var rol = UserClaims.FindFirstValue(ClaimTypes.Role);
            if (emailUser == null || rol == null || dateExpiredClaim == null)
            {
                return BadRequest(new
                {
                    sucess = false,
                    message = "El token no es correcto"
                });
            }
            DateTime dateExpired = DateTime.Parse(dateExpiredClaim);

            DateTime dateNow = DateTime.Now.ToUniversalTime();

            Console.WriteLine($"fecha actual: {dateNow} , fecha token: {dateExpired}");

            if (dateExpired < dateNow)
            {
                return BadRequest(new
                {
                    sucess = false,
                    message = "El token ha expirado"
                });
            }

            var Token = new UserTokens();


            bool result = await _context.Usuarios.AnyAsync(u => u.Email == emailUser && u.IdRolNavigation.Nombre == rol);
            if (!result)
            {
                return BadRequest(new
                {
                    sucess = false,
                    message = "Token no valido"
                });
            }

            Token = JwtHelpers.GetTokenKey(new UserTokens()
            {
                UserEmail = emailUser,
                Rol = rol
            }, _jwtSettings);

            return Ok(new
            {
                sucess = true,
                message = "Token valido",
                token = Token
            });

        }


    }
}
