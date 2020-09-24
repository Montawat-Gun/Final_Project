using System;
using Microsoft.AspNetCore.Http;

namespace Api.Dtos
{
    public class ImageRequest
    {
        public string ImageUrl { get; set; }
        public IFormFile File { get; set; }
        public DateTime TimeImage { get; set; }
        public string PublicId { get; set; }

        public ImageRequest()
        {
            TimeImage = DateTime.Now; 
        }
    }
}