using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform.Models.DataModels;
using Servirform.Models.DTO;
using Servirform.Models.JWT;
using Servirform.Services.Contrato;

namespace Servirform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Empresas : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;
        private readonly IEmpresaService _empresaService;

        public Empresas(ServinformContext context, IMapper mapper, IEmpresaService empresaService)
        {
            _context = context;
            _mapper = mapper;
            _empresaService = empresaService;
        }

        // GET: api/Empresas
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador")]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> GetEmpresas()
        {
            if (_context.Empresas == null)
            {
                return NotFound();
            }
            List<Empresa> result = await _context.Empresas.ToListAsync();
            return _mapper.Map<List<EmpresaDTO>>(result);
        }

        // GET: api/Empresas/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<EmpresaDTO>> GetEmpresa(int id)
        {


            if (_context.Empresas == null)
            {
                return NotFound();
            }
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }

            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == empresa.EmailUsuario;

            if (!Validacion) return NotFound();


            return _mapper.Map<EmpresaDTO>(empresa);
        }
        // PUT: api/Empresas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> PutEmpresa(int id, EmpresaDTO empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest();
            }

            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Empresas.AnyAsync(e => e.Id == empresa.Id && e.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            _context.Entry(_mapper.Map<Empresa>(empresa)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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

        // POST: api/Empresas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<EmpresaDTO>> PostEmpresa(EmpresaDTO empresa)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == empresa.EmailUsuario;

            if (!Validacion) return NotFound();

            if (_context.Empresas == null)
            {
                return Problem("Entity set 'ServinformContext.Empresas'  is null.");
            }
            Empresa modelo = _mapper.Map<Empresa>(empresa);
            _context.Empresas.Add(modelo);
            await _context.SaveChangesAsync();
            Console.WriteLine($"####El id de la nueva empresa es {modelo.Id}");

            return CreatedAtAction("GetEmpresa", new { id = modelo.Id }, _mapper.Map<EmpresaDTO>(modelo));
        }

        // DELETE: api/Empresas/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            if (_context.Empresas == null)
            {
                return NotFound();
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == empresa.EmailUsuario;

            if (!Validacion) return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExists(int id)
        {
            return (_context.Empresas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("PorUsuario/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<DataPaginatorDTO<EmpresaDTO>>> EmpresasPorUsuario(string id, [FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "limit")] int limit = 10)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == id;

            if (!Validacion) return NotFound();
            int totalEmpresas = await _context.Empresas.Where(e => e.EmailUsuario == id).CountAsync();

            int lastPage = totalEmpresas / limit;
            if (totalEmpresas % limit > 0)
            {
                lastPage++;
            }

            Console.WriteLine($"Sobrante de pagina: {totalEmpresas % limit}");
            Console.WriteLine($"Ultima pagina: {lastPage}");
            Console.WriteLine($"Total de Empresas: {totalEmpresas}");

            if (page > lastPage)
            {
                return NotFound();
            }
            Paginator paginator = new Paginator()
            {
                CurrentPage = page,
                LastPage = lastPage,
                Items = new PaginatorItems
                {
                    count = limit,
                    total = totalEmpresas
                }
            };

            List<Empresa> ListEmpresas = await _empresaService.EmpresasPorUsuario(id, limit, page);

            List<EmpresaDTO> result = _mapper.Map<List<EmpresaDTO>>(ListEmpresas);
            return new DataPaginatorDTO<EmpresaDTO>
            {
                Data = result,
                Paginator = paginator
            };

        }
    }
}
