using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Interest
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}