using System.Collections.Generic;

namespace Api.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GameTag> Games { get; set; }
    }
}