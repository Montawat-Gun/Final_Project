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
using AutoMapper;
using Api.Dtos;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetComment")]
        public async Task<ActionResult<CommentDetail>> GetComment(int id)
        {
            var comment = await _context.Comments.Where(x => x.CommentId == id)
            .Include(u => u.User).ThenInclude(i => i.Image).FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return _mapper.Map<CommentDetail>(comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            comment.TimeComment = DateTime.UtcNow;
            _context.Comments.Add(comment);
            if (await _context.SaveChangesAsync() > 0)
            {
                var returnComment = await _context.Comments.Where(x => x.CommentId == comment.CommentId)
                .Include(u => u.User).ThenInclude(i => i.Image).FirstOrDefaultAsync();
                return CreatedAtRoute("GetComment", new { id = comment.CommentId }, _mapper.Map<CommentDetail>(returnComment));
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
