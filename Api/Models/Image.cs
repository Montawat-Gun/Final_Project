using System;

namespace Api.Models
{
    public class Image
    {
        public virtual int ImageId { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual DateTime TimeImage { get; set; }
        public virtual string PublicId { get; set; }
    }
}