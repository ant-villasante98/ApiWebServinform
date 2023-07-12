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
    public class Localidades : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly Mapper _mapper;

        public Localidades(ServinformContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Localidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalidadDTO>>> GetLocalidades()
        {
            if (_context.Localidades == null)
            {
                return NotFound();
            }
            List<Localidad> result = await _context.Localidades.ToListAsync();
            return _mapper.Map<List<Localidad>, List<LocalidadDTO>>(result);
        }

        // GET: api/Localidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocalidadDTO>> GetLocalidad(int id)
        {
            if (_context.Localidades == null)
            {
                return NotFound();
            }
            var localidad = await _context.Localidades.FindAsync(id);

            if (localidad == null)
            {
                return NotFound();
            }

            return _mapper.Map<Localidad, LocalidadDTO>(localidad);
        }

        // PUT: api/Localidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalidad(int id, LocalidadDTO localidad)
        {
            if (id != localidad.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<LocalidadDTO, Localidad>(localidad)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadExists(id))
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

        // POST: api/Localidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocalidadDTO>> PostLocalidad(LocalidadDTO localidad)
        {
            if (_context.Localidades == null)
            {
                return Problem("Entity set 'ServinformContext.Localidades'  is null.");
            }
            var model = _mapper.Map<LocalidadDTO, Localidad>(localidad);
            _context.Localidades.Add(model);
            await _context.SaveChangesAsync();
            localidad = _mapper.Map<Localidad, LocalidadDTO>(model);

            return CreatedAtAction("GetLocalidad", new { id = localidad.Id }, localidad);
        }

        // DELETE: api/Localidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalidad(int id)
        {
            if (_context.Localidades == null)
            {
                return NotFound();
            }
            var localidad = await _context.Localidades.FindAsync(id);
            if (localidad == null)
            {
                return NotFound();
            }

            _context.Localidades.Remove(localidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalidadExists(int id)
        {
            return (_context.Localidades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
