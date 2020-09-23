using System.Collections.Generic;
namespace Api.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<GameGenre> Genres { get; set; }
    }
}