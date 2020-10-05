namespace Api.Models
{
    public class CommentImage : Image
    {
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}