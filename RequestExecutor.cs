using FB.QuickCommenter.Helpers;
using FB.QuickCommenter.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FB.QuickCommenter
{
    public class RequestExecutor
    {
        public string Token { get => Cs.Token; }
        private readonly string _fbApiAddress;
        public ConnectSettings Cs;

        public RequestExecutor(string fbApiAddress, ConnectSettings cs)
        {
            _fbApiAddress = fbApiAddress;
            Cs = cs;
        }

        public async Task<JObject> ExecuteFbRequestAsync(RestRequest req, bool changeToken = true, bool checkError = true, string apiVersion = "")
        {
            var resp = await GetFbResponseAsync(req,changeToken,apiVersion);
            var respJson = (JObject)JsonConvert.DeserializeObject(resp.Content);
            if (checkError)
                ErrorChecker.HasErrorsInResponse(respJson, true);
            return respJson;
        }

        private async Task<IRestResponse> GetFbResponseAsync(RestRequest req, bool changeToken = true, string apiVersion = "")
        {
            var apiAddress = _fbApiAddress;
            if (!string.IsNullOrEmpty(apiVersion))
            {
                apiAddress = Regex.Replace(apiAddress, @"(\d\.\d)", apiVersion);
            }
            var rc = new RestClient(apiAddress);
            if (!string.IsNullOrEmpty(Cs.ProxyAddress))
            {
                rc.Proxy = new WebProxy()
                {
                    Address = new Uri($"http://{Cs.ProxyAddress}:{Cs.ProxyPort}"),
                    Credentials = new NetworkCredential()
                    {
                        UserName = Cs.ProxyLogin,
                        Password = Cs.ProxyPassword
                    }
                };
            }
            if (changeToken)
            {
                if (req.Method == Method.GET)
                {
                    req.AddQueryParameter("access_token", Cs.Token);
                    req.AddQueryParameter("locale", "ru_RU");
                }
                else
                {
                    req.AddParameter("access_token", Cs.Token);
                    req.AddParameter("locale", "ru_RU");
                }
            }
            var resp = await rc.ExecuteAsync(req);
            int tryCount = 0;
            while ((resp.StatusCode == 0
                || resp.StatusCode == HttpStatusCode.BadGateway
                || resp.StatusCode == HttpStatusCode.ProxyAuthenticationRequired) && tryCount < 3)
            {
                rc.Proxy = null;
                resp = await rc.ExecuteAsync(req);
                await Task.Delay(tryCount * 1000);
                tryCount++;
            }
            return resp;
        }
    }
}
