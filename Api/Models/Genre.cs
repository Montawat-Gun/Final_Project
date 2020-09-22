using System.Collections.Generic;

namespace Api.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<GameGenre> Games { get; set; }
    }
}