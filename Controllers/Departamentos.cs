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
    public class Departamentos : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;

        public Departamentos(ServinformContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Departamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> GetDepartamentos()
        {
            if (_context.Departamentos == null)
            {
                return NotFound();
            }
            List<Departamento> result = await _context.Departamentos.ToListAsync();
            return _mapper.Map<List<Departamento>, List<DepartamentoDTO>>(result);
        }

        // GET: api/Departamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDTO>> GetDepartamento(int id)
        {
            if (_context.Departamentos == null)
            {
                return NotFound();
            }
            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();
            }

            return _mapper.Map<Departamento, DepartamentoDTO>(departamento);
        }

        // PUT: api/Departamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, DepartamentoDTO departamento)
        {
            if (id != departamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<DepartamentoDTO, Departamento>(departamento)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(id))
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

        // POST: api/Departamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DepartamentoDTO>> PostDepartamento(DepartamentoDTO departamento)
        {
            if (_context.Departamentos == null)
            {
                return Problem("Entity set 'ServinformContext.Departamentos'  is null.");
            }
            var model = _mapper.Map<DepartamentoDTO, Departamento>(departamento);
            _context.Departamentos.Add(model);
            await _context.SaveChangesAsync();
            departamento = _mapper.Map<Departamento, DepartamentoDTO>(model);

            return CreatedAtAction("GetDepartamento", new { id = departamento.Id }, departamento);
        }

        // DELETE: api/Departamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            if (_context.Departamentos == null)
            {
                return NotFound();
            }
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }

            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartamentoExists(int id)
        {
            return (_context.Departamentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet]
        [Route("PorProvincia/{id}")]
        public async Task<ActionResult<List<DepartamentoDTO>>> DepartamentosPorProvincia(int id)
        {
            if (_context.Departamentos == null)
            {
                return NotFound();
            }
            var departamentos = await _context.Departamentos.Where(d => d.IdProvincia == id).ToListAsync();

            if (departamentos == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<Departamento>, List<DepartamentoDTO>>(departamentos);
        }
    }
}
