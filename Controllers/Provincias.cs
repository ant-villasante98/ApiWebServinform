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
    public class Provincias : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;

        public Provincias(ServinformContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Provincias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProvinciaDTO>>> GetProvincias()
        {
            if (_context.Provincias == null)
            {
                return NotFound();
            }
            List<Provincia> result = await _context.Provincias.ToListAsync();
            return _mapper.Map<List<Provincia>, List<ProvinciaDTO>>(result);
        }

        // GET: api/Provincias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProvinciaDTO>> GetProvincia(int id)
        {
            if (_context.Provincias == null)
            {
                return NotFound();
            }
            var provincia = await _context.Provincias.FindAsync(id);

            if (provincia == null)
            {
                return NotFound();
            }

            return _mapper.Map<Provincia, ProvinciaDTO>(provincia);
        }

        // PUT: api/Provincias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvincia(int id, ProvinciaDTO provincia)
        {
            if (id != provincia.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<ProvinciaDTO, Provincia>(provincia)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
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

        // POST: api/Provincias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProvinciaDTO>> PostProvincia(ProvinciaDTO provincia)
        {
            if (_context.Provincias == null)
            {
                return Problem("Entity set 'ServinformContext.Provincias'  is null.");
            }
            var model = _mapper.Map<ProvinciaDTO, Provincia>(provincia);
            _context.Provincias.Add(model);
            await _context.SaveChangesAsync();
            provincia = _mapper.Map<Provincia, ProvinciaDTO>(model);

            return CreatedAtAction("GetProvincia", new { id = provincia.Id }, provincia);
        }

        // DELETE: api/Provincias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvincia(int id)
        {
            if (_context.Provincias == null)
            {
                return NotFound();
            }
            var provincia = await _context.Provincias.FindAsync(id);
            if (provincia == null)
            {
                return NotFound();
            }

            _context.Provincias.Remove(provincia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProvinciaExists(int id)
        {
            return (_context.Provincias?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("PorPais/{id}")]
        public async Task<ActionResult<List<ProvinciaDTO>>> ProvinciasPorPais(int id)
        {
            if (_context.Provincias == null)
            {
                return NotFound();
            }
            var provincias = await _context.Provincias.Where(pr => pr.IdPais == id).ToListAsync();

            if (provincias == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<Provincia>, List<ProvinciaDTO>>(provincias);
        }
    }
}
