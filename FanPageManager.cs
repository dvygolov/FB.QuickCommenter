using FB.QuickCommenter.Helpers;
using FB.QuickCommenter.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FB.QuickCommenter
{
    public class FanPageManager
    {
        private readonly RequestExecutor _re;

        public FanPageManager(RequestExecutor re)
        {
            _re = re;
        }

        public async Task<List<JToken>> ListFanPagesAsync()
        {
            var request = new RestRequest($"me/accounts", Method.GET);
            request.AddQueryParameter("fields", "name,is_published,access_token");
            request.AddQueryParameter("type", "page");
            var json = await _re.ExecuteFbRequestAsync(request);

            var pages = json["data"].OrderBy(p => p["name"].ToString()).ToList();
            for (int i = 0; i < pages.Count; i++)
            {
                var p = pages[i];
                Console.WriteLine($"{i + 1}. {p["name"]} ({p["id"]}) - {p["is_published"]}");
            }
            return pages;
        }

        public async Task<FanPage> SelectFanPageAsync()
        {
            var pages = await ListFanPagesAsync();
            if (pages.Count == 0) return null;
            PageStart:
            int index;
            bool goodRes;
            do
            {
                Console.Write("Выберите страницу, введя её номер, и нажмите Enter:");
                var readIndex = Console.ReadLine();
                goodRes = int.TryParse(readIndex, out index);
                index--;
                if (index < 0 || index > pages.Count - 1) goodRes = false;
            }
            while (!goodRes);

            var selectedPage = pages[index];

            if (!bool.Parse(selectedPage["is_published"].ToString()))
            {
                Console.Write($"Страница {selectedPage["name"]} не опубликована! Опубликовать?");
                if (YesNoSelector.ReadAnswerEqualsYes())
                {
                    //Страница не опубликована! Пытаемся опубликовать
                    var request = new RestRequest(selectedPage["id"].ToString(), Method.POST);
                    request.AddParameter("access_token", selectedPage["access_token"].ToString());
                    request.AddParameter("is_published", "true");
                    var publishJson = await _re.ExecuteFbRequestAsync(request, false);
                    if (publishJson["error"] != null)
                    {
                        //невозможно опубликовать страницу, вероятно, она забанена!
                        Console.WriteLine($"Страница {selectedPage["name"]} не опубликована и, вероятно, забанена!");
                        goto PageStart;
                    }
                    else
                    {
                        //уведомим пользователя, что мы опубликовали страницу после снятия с публикации
                        Console.WriteLine($"Страница {selectedPage["name"]} была заново опубликована после снятия с публикации!");
                        return new FanPage(
                            selectedPage["id"].ToString(),
                            selectedPage["name"].ToString(),
                            selectedPage["access_token"].ToString());
                    }
                }
                else
                    goto PageStart;
            }
            return new FanPage(
                selectedPage["id"].ToString(),
                selectedPage["name"].ToString(),
                selectedPage["access_token"].ToString());
        }

        public async Task<List<(string, string)>> GetPostIdsAsync(string fpId, string accessToken)
        {
            var req = new RestRequest($"{fpId}/ads_posts", Method.GET);
            req.AddQueryParameter("fields", "message");
            req.AddQueryParameter("access_token", accessToken);
            var json = await _re.ExecuteFbRequestAsync(req, changeToken: false);
            return json["data"]
                .Select(p => (p["id"].ToString(), p["message"]?.ToString()??"Без текста!"))
                .ToList();
        }
    }
}
