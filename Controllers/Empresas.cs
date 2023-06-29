using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform.Models.DataModels;
using Servirform.Models.DTO;

namespace Servirform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Empresas : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;

        public Empresas(ServinformContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Empresas
        [HttpGet]
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

            return _mapper.Map<EmpresaDTO>(empresa);
        }
        // PUT: api/Empresas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, EmpresaDTO empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<EmpresaDTO>> PostEmpresa(EmpresaDTO empresa)
        {
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

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExists(int id)
        {
            return (_context.Empresas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
