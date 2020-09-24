using System;
using System.Collections.Generic;
namespace Api.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual GameImage Image { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
    }
}