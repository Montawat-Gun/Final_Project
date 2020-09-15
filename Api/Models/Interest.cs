using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Interest
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        [Range(0, 1)]
        public int IsInterest { get; set; }
    }
}