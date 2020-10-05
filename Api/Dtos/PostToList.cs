using System;
using Api.Models;

namespace Api.Dtos
{
    public class PostToList
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime TimePost { get; set; }
        public UserToList User { get; set; }
        public GamesToList Game { get; set; }
        public string ImageUrl { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool isLike { get; set; }
    }
}