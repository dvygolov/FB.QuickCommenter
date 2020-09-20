using FB.QuickCommenter.Helpers;
using FB.QuickCommenter.Model;
using System;
using System.Threading.Tasks;

namespace FB.QuickCommenter
{
    class Program
    {
        static async Task Main()
        {
            var fbApiAddress = "https://graph.facebook.com/v8.0/";
            Console.Write("Введите access token:");
            var token = Console.ReadLine();
            var cs = new ConnectSettings() { Token = token };
            ProxyHelper.FillProxy(cs);
            var re = new RequestExecutor(fbApiAddress, cs);
            var fpm = new FanPageManager(re);
            var fp = await fpm.SelectFanPageAsync();
            var posts = await fpm.GetPostIdsAsync(fp.Id, fp.Token);
            for (var i = 0; i < posts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {posts[i].Item2}");
            }
            Console.Write("Выберите пост:");
            var index = int.Parse(Console.ReadLine()) - 1;
            var postId = posts[index].Item1;
            var comments = CommentsHelper.GetComments();
            Console.WriteLine($"Найдено {comments.Count} комментариев!");
            await BulkHelper.BulkProcessAsync(fbApiAddress, async (re, cs) =>
            {
                if (comments.Count == 0)
                {
                    Console.WriteLine("Комментарии кончились!");
                    return;
                }
                var c = comments[0];
                comments.RemoveAt(0);
                Console.WriteLine($"Оставляем коммент:{c}");
                var cm = new CommentsManager(re);
                await cm.AddCommentAsync(c, postId);
            });
            Console.ReadKey();
        }
    }
}