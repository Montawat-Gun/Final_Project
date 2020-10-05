using System;

namespace Api.Models
{
    public class GameImage : Image
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}