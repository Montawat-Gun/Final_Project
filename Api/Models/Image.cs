using System;

namespace Api.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime TimeImage { get; set; }
        public string PublicId { get; set; }
    }
}