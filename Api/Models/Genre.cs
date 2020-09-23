using System.Collections.Generic;

namespace Api.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<GameGenre> Games { get; set; }
    }
}