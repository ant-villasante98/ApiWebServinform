using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servirform.Models.DataModels;
using Servirform.DataAcces;
using AutoMapper;
using Servirform.Models.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Servirform.Models.JWT;
using Servirform.Services.Contrato;

namespace Servirform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usuarios : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public Usuarios(ServinformContext context, IMapper mapper, IUsuarioService usuarioService)
        {
            _context = context;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        // GET: api/Usuarios
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            List<Usuario> result = await _context.Usuarios.ToListAsync();
            return _mapper.Map<List<UsuarioDTO>>(result);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(string id)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == id;
            // Console.WriteLine($"Resultado de la Validacion :{Validacion} en {nameof(Usuarios)}");
            // bool Validacion = await _usuarioService.ValidarUsuario(UserClaims, id);
            // Si la validacion no cumple 
            if (!Validacion) return Unauthorized();

            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> PutUsuario(string id, UsuarioDTO usuario)
        {
            if (id != usuario.Email)
            {
                return BadRequest();
            }

            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == id;
            // Si la validacion no cumple 
            if (!Validacion) return Unauthorized();

            _context.Entry(_mapper.Map<Usuario>(usuario)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ServinformContext.Usuarios'  is null.");
            }
            _context.Usuarios.Add(_mapper.Map<Usuario>(usuario));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetUsuario", new { id = usuario.Email }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == id;
            // Si la validacion no cumple 
            if (!Validacion) return Unauthorized();

            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(string id)
        {
            return (_context.Usuarios?.Any(e => e.Email == id)).GetValueOrDefault();
        }
    }
}
