namespace FB.QuickCommenter.Model
{
    public class FanPage
    {
        public FanPage(string id, string name, string token)
        {
            Id = id;
            Name = name;
            Token = token;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
