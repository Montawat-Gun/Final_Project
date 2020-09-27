using System.Collections.Generic;
using System.Linq;
using Api.Data;
using Api.Models;
using Newtonsoft.Json;

namespace Api.Helpers
{
    public static class Seed
    {
        public static void SeedData(DataContext context)
        {
            SeedTags(context);
            SeedGames(context);
        }

        static void SeedTags(DataContext _context)
        {
            if (!_context.Tags.Any())
            {
                var data = System.IO.File.ReadAllText("Datas/TagsData.json");
                var tags = JsonConvert.DeserializeObject<List<Tag>>(data);
                foreach (Tag tag in tags)
                {
                    _context.Tags.Add(tag);
                }
                _context.SaveChanges();
            }
        }

        static void SeedGames(DataContext _context)
        {
            if (!_context.Games.Any())
            {
                var data = System.IO.File.ReadAllText("Datas/GamesData.json");
                var games = JsonConvert.DeserializeObject<List<Game>>(data);
                foreach (Game game in games)
                {
                    _context.Games.Add(game);
                }
                _context.SaveChanges();
            }
        }
    }
}