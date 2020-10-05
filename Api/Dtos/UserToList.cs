namespace Api.Dtos
{
    public class UserToList
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFollowing { get; set; }
    }
}