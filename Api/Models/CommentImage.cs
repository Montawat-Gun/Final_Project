namespace Api.Models
{
    public class CommentImage : Image
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}