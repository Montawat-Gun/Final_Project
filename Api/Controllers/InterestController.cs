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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interest>>> GetInterest()
        {
            return await _context.Interests.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Interest>>> GetInterest(string id)
        {
            var interest = await _context.Interests.Where(u => u.UserId == id).ToListAsync();
            return interest;
        }

        [HttpGet("{userId}/{gameId}")]
        public async Task<ActionResult> GetIsInterest(string userId, int gameId)
        {
            var interest = await _context.Interests.Where(x => x.UserId == userId && x.GameId == gameId)
            .FirstOrDefaultAsync();

            return Ok(interest);
        }

        // [HttpPost]
        // public async Task<ActionResult<List<Interest>>> PostInterest(List<Interest> interests)
        // {
        //     var userId = interests.Select(u => u.UserId).FirstOrDefault();
        //     var Ids = new List<int>();
        //     foreach (Interest interest in interests)
        //     {
        //         Ids.Add(interest.GameId);
        //         interest.IsInterest = 1;
        //         _context.Interests.Add(interest);
        //     }
        //     var genresId = _context.Games.Select(g => g.GameId).ToList();
        //     foreach (int genreId in genresId)
        //     {
        //         if (!Ids.Contains(genreId))
        //         {
        //             Interest interest = new Interest
        //             {
        //                 GameId = genreId,
        //                 UserId = userId,
        //                 IsInterest = 0
        //             };
        //             _context.Interests.Add(interest);
        //             interests.Add(interest);
        //         }
        //     }
        //     await _context.SaveChangesAsync();
        //     return CreatedAtAction("GetInterest", interests);
        // }

        [HttpPost]
        public async Task<ActionResult<List<Interest>>> PostInterest(List<Interest> interests)
        {
            foreach (var interest in interests)
            {
                _context.Interests.Add(interest);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetInterest", interests);
        }

        [HttpDelete("{userId}/{gameId}")]
        public async Task<ActionResult<Interest>> DeleteInterest(string userId, int gameId)
        {
            var interest = await _context.Interests.Where(x => x.UserId == userId && x.GameId == gameId)
            .FirstOrDefaultAsync();
            if (interest == null)
            {
                return NotFound();
            }

            _context.Interests.Remove(interest);
            await _context.SaveChangesAsync();

            return interest;
        }

        private bool InterestExists(string id)
        {
            return _context.Interests.Any(e => e.UserId == id);
        }
    }
}
