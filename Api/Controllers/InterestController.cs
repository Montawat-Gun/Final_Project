using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly DataContext _context;

        public InterestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Interest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interest>>> GetInterest()
        {
            return await _context.Interest.ToListAsync();
        }

        // GET: api/Interest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Interest>> GetInterest(string id)
        {
            var interest = await _context.Interest.FindAsync(id);

            if (interest == null)
            {
                return NotFound();
            }

            return interest;
        }

        // PUT: api/Interest/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterest(string UserId, Interest interest)
        {
            if (UserId != interest.UserId)
            {
                return BadRequest();
            }

            _context.Entry(interest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestExists(UserId))
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

        // POST: api/Interest
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<List<Interest>>> PostInterest(List<Interest> interests)
        {
            var userId = interests.Select(u => u.UserId).FirstOrDefault();
            var Ids = new List<int>();
            foreach (Interest interest in interests)
            {
                Ids.Add(interest.GenreId);
                interest.IsInterest = 1;
                _context.Interest.Add(interest);
            }
            var genresId = _context.Genres.Select(g => g.GenreId).ToList();
            foreach (int genreId in genresId)
            {
                if (!Ids.Contains(genreId))
                {
                    Interest interest = new Interest
                    {
                        GenreId = genreId,
                        UserId = userId,
                        IsInterest = 0
                    };
                    _context.Interest.Add(interest);
                    interests.Add(interest);
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetInterest", interests);
        }

        // DELETE: api/Interest/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Interest>> DeleteInterest(string id)
        {
            var interest = await _context.Interest.FindAsync(id);
            if (interest == null)
            {
                return NotFound();
            }

            _context.Interest.Remove(interest);
            await _context.SaveChangesAsync();

            return interest;
        }

        private bool InterestExists(string id)
        {
            return _context.Interest.Any(e => e.UserId == id);
        }
    }
}
