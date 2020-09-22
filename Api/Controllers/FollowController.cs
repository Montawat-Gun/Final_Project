using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class FollowController : ControllerBase
    {
        private readonly DataContext _context;

        public FollowController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Follow>>> GetFollows()
        {
            return await _context.Follows.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Follow>> GetFollow(string id)
        {
            var follow = await _context.Follows.FindAsync(id);

            if (follow == null)
            {
                return NotFound();
            }

            return follow;
        }

        [HttpPost]
        public async Task<ActionResult<Follow>> PostFollow(Follow follow)
        {
            follow.TimeFollow = DateTime.Now;
            _context.Follows.Add(follow);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FollowExists(follow.FollowingId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFollow", new { id = follow.FollowingId }, follow);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Follow>> DeleteFollow(string id)
        {
            var follow = await _context.Follows.FindAsync(id);
            if (follow == null)
            {
                return NotFound();
            }

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();

            return follow;
        }

        private bool FollowExists(string id)
        {
            return _context.Follows.Any(e => e.FollowingId == id);
        }
    }
}
