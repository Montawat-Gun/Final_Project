using System.Collections.Generic;

namespace Api.Models
{
    public class Suggest
    {
        public string UserId { get; set; }
        public List<int> GamesId { get; set; }
    }
}