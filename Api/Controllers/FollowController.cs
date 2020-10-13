using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Api.Dtos;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FollowController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Follow>>> GetFollows()
        {
            return await _context.Follows.ToListAsync();
        }

        [HttpGet("following/{fromUserId}/{userId}")]
        public async Task<ActionResult> GetFollowing(string fromUserId, string userId)
        {
            var following = await _context.Follows.Where(u => u.FollowerId == userId).Include(i => i.Following.Image).Select(f => f.Following).ToListAsync();
            var fromUserfollowing = await _context.Follows.Where(u => u.FollowerId == fromUserId).Select(f => f.Following).ToListAsync();
            var followingToReturn = _mapper.Map<IEnumerable<UserToList>>(following);
            foreach (var follow in followingToReturn)
            {
                follow.IsFollowing = fromUserfollowing.Where(u => u.Id == follow.Id).Any();
            }
            return Ok(followingToReturn);
        }

        [HttpGet("follower/{fromUserId}/{userId}")]
        public async Task<ActionResult> GetFollower(string fromUserId, string userId)
        {
            var followers = await _context.Follows.Where(u => u.FollowingId == userId).Include(i => i.Follower.Image).Select(f => f.Follower).ToListAsync();
            var fromUserfollowing = await _context.Follows.Where(u => u.FollowerId == fromUserId).Select(f => f.Following).ToListAsync();
            var followersToReturn = _mapper.Map<IEnumerable<UserToList>>(followers);
            foreach (var follow in followersToReturn)
            {
                follow.IsFollowing = fromUserfollowing.Where(u => u.Id == follow.Id).Any();
            }
            return Ok(followersToReturn);
        }

        [HttpGet("isfollowing/{fromUserId}/{userId}")]
        public async Task<IActionResult> IsFollowing(string fromUserId, string userId)
        {
            var following = await _context.Follows
            .Where(x => x.FollowerId == fromUserId && x.FollowingId == userId).AnyAsync();
            return Ok(new { IsFollowing = following });
        }

        [HttpPost]
        public async Task<ActionResult<Follow>> PostFollow(Follow follow)
        {
            follow.TimeFollow = DateTime.UtcNow;
            _context.Follows.Add(follow);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FollowExists(follow.FollowerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFollowing", new { fromUserId = follow.FollowerId, userId = follow.FollowerId }, follow);
        }

        [HttpDelete("unfollow/{followerId}/{followingId}")]
        public async Task<ActionResult<Follow>> DeleteFollow(string followerId, string followingId)
        {
            var follow = await _context.Follows.Where(x => x.FollowerId == followerId && x.FollowingId == followingId).FirstOrDefaultAsync();
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
