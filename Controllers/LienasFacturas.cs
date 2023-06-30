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
    public class LienasFacturas : ControllerBase
    {
        private readonly ServinformContext _context;
        private readonly IMapper _mapper;

        public LienasFacturas(ServinformContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LienasFacturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineasFacturaDTO>>> GetLineasFacturas()
        {
            if (_context.LineasFacturas == null)
            {
                return NotFound();
            }
            List<LineasFactura> result = await _context.LineasFacturas.ToListAsync();
            return _mapper.Map<List<LineasFacturaDTO>>(result);
        }

        // GET: api/LienasFacturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineasFacturaDTO>> GetLineasFactura(int id)
        {
            if (_context.LineasFacturas == null)
            {
                return NotFound();
            }
            var lineasFactura = await _context.LineasFacturas.FindAsync(id);

            if (lineasFactura == null)
            {
                return NotFound();
            }

            return _mapper.Map<LineasFacturaDTO>(lineasFactura);
        }

        // PUT: api/LienasFacturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineasFactura(int id, LineasFacturaDTO lineasFactura)
        {
            if (id != lineasFactura.NroFactura)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<LineasFactura>(lineasFactura)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineasFacturaExists(id))
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

        // POST: api/LienasFacturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineasFacturaDTO>> PostLineasFactura(LineasFacturaDTO lineasFactura)
        {
            if (_context.LineasFacturas == null)
            {
                return Problem("Entity set 'ServinformContext.LineasFacturas'  is null.");
            }
            LineasFactura model = _mapper.Map<LineasFactura>(lineasFactura);
            _context.LineasFacturas.Add(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LineasFacturaExists(lineasFactura.NroFactura))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLineasFactura", new { id = model.NroFactura }, _mapper.Map<LineasFacturaDTO>(model));
        }

        // DELETE: api/LienasFacturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineasFactura(int id)
        {
            if (_context.LineasFacturas == null)
            {
                return NotFound();
            }
            var lineasFactura = await _context.LineasFacturas.FindAsync(id);
            if (lineasFactura == null)
            {
                return NotFound();
            }

            _context.LineasFacturas.Remove(lineasFactura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LineasFacturaExists(int id)
        {
            return (_context.LineasFacturas?.Any(e => e.NroFactura == id)).GetValueOrDefault();
        }
    }
}
