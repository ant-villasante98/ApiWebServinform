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

namespace Servirform.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public async Task<IActionResult> PostLogin(UserLogins userLogins)
        {
            var Token = new UserTokens();

            // Conprobamos que la tabla usuarios no este vacia
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            // Buscamos el usuario solicitado
            var searchUser = await _context.Usuarios.FirstOrDefaultAsync(user => user.Email == userLogins.UserEmail && user.Password == userLogins.Password);
            // En caso de no haber ninguna concidencia se informamos el resultado
            if (searchUser == null)
            {
                return BadRequest("Wrong password or not found user");
            }
            Console.WriteLine($"{searchUser.Nombre}, {searchUser.Apellido}, {searchUser.Email}, {searchUser.IdRol}");

            Token = JwtHelpers.GetTokenKey(new UserTokens()
            {
                UserEmail = searchUser.Email,
                Rol = (Roles)searchUser.IdRol
            }, _jwtSettings);

            return Ok(Token);

        }
    }
}
