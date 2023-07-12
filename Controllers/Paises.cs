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
    public class Paises : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly Mapper _mapper;

        public Paises(ServinformContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Paises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaisDTO>>> GetPaises()
        {
            if (_context.Paises == null)
            {
                return NotFound();
            }
            List<Pais> result = await _context.Paises.ToListAsync();
            return _mapper.Map<List<Pais>, List<PaisDTO>>(result);
        }

        // GET: api/Paises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaisDTO>> GetPais(int id)
        {
            if (_context.Paises == null)
            {
                return NotFound();
            }
            var pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return _mapper.Map<Pais, PaisDTO>(pais);
        }

        // PUT: api/Paises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais(int id, PaisDTO pais)
        {
            if (id != pais.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<PaisDTO, Pais>(pais)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(id))
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

        // POST: api/Paises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaisDTO>> PostPais(PaisDTO pais)
        {
            if (_context.Paises == null)
            {
                return Problem("Entity set 'ServinformContext.Paises'  is null.");
            }
            var model = _mapper.Map<PaisDTO, Pais>(pais);
            _context.Paises.Add(model);
            await _context.SaveChangesAsync();
            pais = _mapper.Map<Pais, PaisDTO>(model);

            return CreatedAtAction("GetPais", new { id = pais.Id }, pais);
        }

        // DELETE: api/Paises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePais(int id)
        {
            if (_context.Paises == null)
            {
                return NotFound();
            }
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaisExists(int id)
        {
            return (_context.Paises?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
