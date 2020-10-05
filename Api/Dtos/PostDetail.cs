using System;
using System.Collections.Generic;
using Api.Models;

namespace Api.Dtos
{
    public class PostDetail
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
        public ICollection<CommentDetail> Comments { get; set; }
        public ICollection<LikeDetail> Likes { get; set; }
    }
}