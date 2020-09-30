using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eLibrairie.Core.Data.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailCommandesController : ControllerBase
    {
        private readonly DefaultContext _context;

        public DetailCommandesController(DefaultContext context)
        {
            _context = context;
        }

        // GET: api/DetailCommandes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailCommande>>> GetDetailCommandes()
        {
            return await _context.DetailCommandes.ToListAsync();
        }

        // GET: api/DetailCommandes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailCommande>> GetDetailCommande(int id)
        {
            var detailCommande = await _context.DetailCommandes.FindAsync(id);

            if (detailCommande == null)
            {
                return NotFound();
            }

            return detailCommande;
        }

        // PUT: api/DetailCommandes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetailCommande(int id, DetailCommande detailCommande)
        {
            if (id != detailCommande.Id)
            {
                return BadRequest();
            }

            _context.Entry(detailCommande).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailCommandeExists(id))
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

        // POST: api/DetailCommandes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DetailCommande>> PostDetailCommande(DetailCommande detailCommande)
        {
            _context.DetailCommandes.Add(detailCommande);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetailCommande", new { id = detailCommande.Id }, detailCommande);
        }

        // DELETE: api/DetailCommandes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DetailCommande>> DeleteDetailCommande(int id)
        {
            var detailCommande = await _context.DetailCommandes.FindAsync(id);
            if (detailCommande == null)
            {
                return NotFound();
            }

            _context.DetailCommandes.Remove(detailCommande);
            await _context.SaveChangesAsync();

            return detailCommande;
        }

        private bool DetailCommandeExists(int id)
        {
            return _context.DetailCommandes.Any(e => e.Id == id);
        }
    }
}
