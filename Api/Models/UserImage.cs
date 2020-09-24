namespace Api.Models
{
    public class UserImage : Image
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}