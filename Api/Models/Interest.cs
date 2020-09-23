using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Interest
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        [Range(0, 1)]
        public int IsInterest { get; set; }
    }
}