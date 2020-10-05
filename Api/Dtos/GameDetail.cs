namespace Api.Dtos
{
    public class GameDetail
    {
        public int GameId { get; set; }
        public string name { get; set; }
        public string ImageUrl { get; set; }
        public int UserInterestCount { get; set; }
        public int PostCount { get; set; }
    }
}