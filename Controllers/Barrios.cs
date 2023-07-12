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
    public class Barrios : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly Mapper _mapper;

        public Barrios(ServinformContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Barrios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarrioDTO>>> GetBarrios()
        {
            if (_context.Barrios == null)
            {
                return NotFound();
            }
            List<Barrio> result = await _context.Barrios.ToListAsync();
            return _mapper.Map<List<Barrio>, List<BarrioDTO>>(result);
        }

        // GET: api/Barrios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarrioDTO>> GetBarrio(int id)
        {
            if (_context.Barrios == null)
            {
                return NotFound();
            }
            var barrio = await _context.Barrios.FindAsync(id);

            if (barrio == null)
            {
                return NotFound();
            }

            return _mapper.Map<Barrio, BarrioDTO>(barrio);
        }

        // PUT: api/Barrios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarrio(int id, BarrioDTO barrio)
        {
            if (id != barrio.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<BarrioDTO, Barrio>(barrio)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarrioExists(id))
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

        // POST: api/Barrios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BarrioDTO>> PostBarrio(BarrioDTO barrio)
        {
            if (_context.Barrios == null)
            {
                return Problem("Entity set 'ServinformContext.Barrios'  is null.");
            }
            var model = _mapper.Map<BarrioDTO, Barrio>(barrio);
            _context.Barrios.Add(model);
            await _context.SaveChangesAsync();

            barrio = _mapper.Map<Barrio, BarrioDTO>(model);

            return CreatedAtAction("GetBarrio", new { id = barrio.Id }, barrio);
        }

        // DELETE: api/Barrios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBarrio(int id)
        {
            if (_context.Barrios == null)
            {
                return NotFound();
            }
            var barrio = await _context.Barrios.FindAsync(id);
            if (barrio == null)
            {
                return NotFound();
            }

            _context.Barrios.Remove(barrio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BarrioExists(int id)
        {
            return (_context.Barrios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
