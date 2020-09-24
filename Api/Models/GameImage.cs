using System;

namespace Api.Models
{
    public class GameImage : Image
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}