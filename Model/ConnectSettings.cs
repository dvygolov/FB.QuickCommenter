using Newtonsoft.Json;

namespace FB.QuickCommenter.Model
{
    public class ConnectSettings
    {
        public string Token { get; set; }
        public string ProxyAddress { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }
        
        public ConnectSettings() { }
        public ConnectSettings(string line)
        {
            var s = line.Split(':');
            Token=s[0];
            if (s.Length > 1)
            {
                ProxyAddress = s[1];
                ProxyPort = s[2];
                ProxyLogin = s[3];
                ProxyPassword = s[4];
            }
        }
    }
}
