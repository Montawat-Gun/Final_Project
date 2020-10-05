using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Api.Dtos;
using Api.Services;
using AutoMapper;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public PostController(DataContext context, IImageService imageService, IMapper mapper)
        {
            _mapper = mapper;
            _imageService = imageService;
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetPosts(string userId)
        {
            // var posts = await _context.Posts.Where(u => u.UserId == userId).Include(i => i.Image)
            // .Include(u => u.User).ThenInclude(u => u.Image)
            // .Include(c => c.Comments)
            // .Include(g => g.Game).Include(l => l.Likes).ToListAsync();


            var followsId = await _context.Follows.Where(u => u.FollowerId == userId).Include(u => u.Following).Select(i => i.FollowingId).ToListAsync();
            var gamesId = await _context.Interests.Where(u => u.UserId == userId).Select(g => g.Game.GameId).ToListAsync();
            if (followsId == null && gamesId == null)
                return NotFound();
            var posts = await _context.Posts.Where(u => followsId.Contains(u.UserId) || gamesId.Contains(u.GameId))
            .Include(u => u.User).ThenInclude(i => i.Image).Include(i => i.Game)
            .Include(i => i.Image).Include(c => c.Comments).Include(l => l.Likes).ToListAsync();
            var postsToReturn = _mapper.Map<IEnumerable<PostToList>>(posts).OrderByDescending(o => o.TimePost);
            foreach (var post in posts)
            {
                postsToReturn.Where(p => p.PostId == post.PostId).FirstOrDefault().isLike = 
                post.Likes.Where(x => x.UserId == userId && x.PostId == post.PostId).Any();
            }
            return Ok(postsToReturn);
        }

        [HttpGet("{userId}/{fromUserId}")]
        public async Task<ActionResult<IEnumerable<PostToList>>> GetPosts(string userId, string fromUserId)
        {
            var posts = await _context.Posts.Where(u => u.UserId == userId).Include(i => i.Image)
            .Include(u => u.User).ThenInclude(i => i.Image).Include(g => g.Game).Include(c => c.Comments).Include(l => l.Likes).ToListAsync();
            var postsToReturn = _mapper.Map<IEnumerable<PostToList>>(posts);
            foreach (var post in posts)
            {
                postsToReturn.Where(p => p.PostId == post.PostId).FirstOrDefault().isLike = post.Likes.
                Where(x => x.PostId == post.PostId && x.UserId == fromUserId).Any();
            }
            return Ok(postsToReturn);
        }

        [HttpGet("Detail/{postId}/{userId}")]
        public async Task<IActionResult> GetPost(int postId, string userId)
        {
            var post = await _context.Posts.Where(p => p.PostId == postId)
            .Include(c => c.Comments).ThenInclude(u => u.User).ThenInclude(i => i.Image)
            .Include(u => u.User).ThenInclude(i => i.Image)
            .Include(g => g.Game)
            .Include(l => l.Likes).ThenInclude(u => u.User).ThenInclude(i => i.Image)
            .Include(i => i.Image).FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }
            var postToReturn = _mapper.Map<PostDetail>(post);
            postToReturn.isLike = post.Likes.Where(x => x.PostId == post.PostId && x.UserId == userId).Any();
            return Ok(postToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            post.TimePost = DateTime.Now;
            _context.Posts.Add(post);
            var result = await _context.SaveChangesAsync();
            return CreatedAtAction("GetPost", new { postId = post.PostId, userId = post.UserId }, post);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult<Post>> DeletePost(int postId)
        {
            var post = await _context.Posts.Where(p => p.PostId == postId).Include(c => c.Comments)
                            .Include(l => l.Likes).FirstOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }
            await _imageService.DeletePostImage(postId);
            foreach (var comment in post.Comments)
            {
                _context.Comments.Remove(comment);
            }
            foreach (var like in post.Likes)
            {
                _context.Likes.Remove(like);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        [HttpPost("Like")]
        public async Task<IActionResult> LikePost(Like like)
        {
            var isLike = await _context.Likes.Where(x => x.PostId == like.PostId && x.UserId == like.UserId).AnyAsync();
            if (isLike)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                return Ok(like);
            }
            else
            {
                await _context.Likes.AddAsync(like);
                await _context.SaveChangesAsync();
                return Ok(like);
            }
        }

        [HttpGet("{postId}/image", Name = "GetPostImage")]
        public async Task<IActionResult> GetPostImage(int postId)
        {
            var image = await _imageService.GetPostImage(postId);
            return Ok(image);
        }

        [HttpPost("{postId}/image")]
        public async Task<IActionResult> PostPostImage(int postId, [FromForm] ImagePostRequest imageRequest)
        {
            imageRequest.PostId = postId;
            var image = await _imageService.AddPostImage(imageRequest);
            return CreatedAtRoute("GetPostImage", new { postId = postId, id = image.ImageId }, image);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
