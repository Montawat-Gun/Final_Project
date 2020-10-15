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
using Api.Dtos;
using AutoMapper;
using Api.Services;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public GameController(DataContext context, IImageService imageService, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetGames()
        {
            var games = await _context.Games.Include(i => i.Image).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<GamesToList>>(games));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDetail>> GetGame(int id)
        {
            var game = await _context.Games.Where(g => g.GameId == id).Include(u => u.Interests)
            .Include(i => i.Image).Include(p => p.Posts).FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            return _mapper.Map<GameDetail>(game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game gameToEdit)
        {

            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return BadRequest();
            game.Name = gameToEdit.Name;
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _context.Games.Where(x => x.GameId == id).Include(p => p.Posts)
            .ThenInclude(i => i.Image).Include(i => i.Image).Include(i => i.Interests).FirstOrDefaultAsync();
            if (game == null)
            {
                return NotFound();
            }
            foreach (var interest in game.Interests)
            {
                _context.Interests.Remove(interest);
            }
            foreach (var post in game.Posts)
            {
                _context.Images.Remove(post.Image);
                _context.Posts.Remove(post);
            }
            if (game.Image != null)
            {
                await _imageService.DeleteGameImage(game.Image);
                _context.GameImages.Remove(game.Image);
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}/image")]
        public async Task<ActionResult> GetGameImage(int id)
        {
            var image = await _imageService.GetGameImage(id);
            return Ok(image);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("{id}/image")]
        public async Task<ActionResult> PostGameImage(int id, [FromForm] ImageGameRequest gameImage)
        {
            gameImage.GameId = id;
            var image = await _imageService.UpdateGameImage(gameImage);
            return CreatedAtAction("GetGameImage", new { id = gameImage.GameId }, image);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
