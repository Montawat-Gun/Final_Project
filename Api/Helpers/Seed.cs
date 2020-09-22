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
            SeedGames(context);
        }

        static void SeedGames(DataContext _context)
        {
            if (!_context.Genres.Any())
            {
                var data = System.IO.File.ReadAllText("Datas/GenreData.json");
                var genres = JsonConvert.DeserializeObject<List<Genre>>(data);
                foreach (Genre genre in genres)
                {
                    _context.Genres.Add(genre);
                }
                _context.SaveChanges();
            }
        }
    }
}