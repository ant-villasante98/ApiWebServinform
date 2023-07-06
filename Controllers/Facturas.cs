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
    public class Facturas : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;
        private readonly IFacturaService _facturaService;

        public Facturas(ServinformContext context, IMapper mapper, IFacturaService facturaService)
        {
            _context = context;
            _mapper = mapper;
            _facturaService = facturaService;
        }

        // GET: api/Facturas
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador")]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> GetFacturas()
        {

            if (_context.Facturas == null)
            {
                return NotFound();
            }
            List<Factura> result = await _context.Facturas.ToListAsync();
            return _mapper.Map<List<FacturaDTO>>(result);
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador , usuario")]
        public async Task<ActionResult<FacturaDTO>> GetFactura(int id)
        {

            if (_context.Facturas == null)
            {
                return NotFound();
            }
            var factura = await _context.Facturas.FindAsync(id);
            // .Include(listf => listf.LineasFacturas)
            // .ThenInclude(lf => lf.CodArticuloNavigation).FirstAsync();
            // var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Empresas.AnyAsync(e => e.Id == factura.IdEmpresa && e.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            factura.LineasFacturas = await _context.LineasFacturas.Where(lf => lf.NroFactura == id).Include(lf => lf.CodArticuloNavigation).ToListAsync();

            return _mapper.Map<FacturaDTO>(factura);
        }

        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> PutFactura(int id, FacturaDTO factura)
        {
            if (id != factura.NroFactura)
            {
                return BadRequest();
            }

            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Facturas.AnyAsync(f => f.NroFactura == id && f.IdEmpresaNavigation.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            _context.Entry(_mapper.Map<Factura>(factura)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            if (_context.Facturas == null)
            {
                return NotFound();
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Empresas.AnyAsync(e => factura.IdEmpresa == e.Id && e.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(int id)
        {
            return (_context.Facturas?.Any(e => e.NroFactura == id)).GetValueOrDefault();
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<FacturaDTO>> RegistrarFactura(FacturaDTO factura)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Empresas.AnyAsync(e => factura.IdEmpresa == e.Id && e.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            try
            {
                Factura facturaGenerada = await _facturaService.RegitrarFactura(_mapper.Map<Factura>(factura));
                return _mapper.Map<FacturaDTO>(facturaGenerada);


            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("PorEmpresas/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> FacturasPorEmpresas(int id)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : await _context.Empresas.AnyAsync(e => e.Id == id && e.EmailUsuario == EmailUser);

            if (!Validacion) return NotFound();

            List<Factura> facturas = await _facturaService.FacturasPorEmpresas(id);

            return _mapper.Map<List<FacturaDTO>>(facturas);
        }

        [HttpGet]
        [Route("PorUsuario/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrador,usuario")]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> FacturasPorUsuario(string id)
        {
            ClaimsPrincipal UserClaims = this.User;
            var RoleUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            var EmailUser = UserClaims.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            bool Validacion = RoleUser == Roles.administrador.ToString() ? true : EmailUser == id;

            if (!Validacion) return NotFound();

            List<Factura> ListFacturas = await _facturaService.FacturasPorUsuario(id);

            return _mapper.Map<List<FacturaDTO>>(ListFacturas);
        }
    }
}
