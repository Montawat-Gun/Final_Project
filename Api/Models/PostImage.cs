namespace Api.Models
{
    public class PostImage : Image
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}