using System.Collections.Generic;
using Api.Models;

namespace Api.Dtos
{
    public class GamesToList
    {
        public int GameId { get; set; }
        public string name { get; set; }
        public string ImageUrl { get; set; }
    }
}