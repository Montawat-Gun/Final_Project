namespace Api.Dtos
{
    public class UserToContact
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public int MessageUnReadCount { get; set; }
    }
}