using RestSharp;
using System.Threading.Tasks;

namespace FB.QuickCommenter
{
    public class CommentsManager
    {
        private readonly RequestExecutor _re;

        public CommentsManager(RequestExecutor re)
        {
            _re = re;
        }

        public async Task AddCommentAsync(string comment, string postId)
        {
            var req = new RestRequest($"{postId}/comments", Method.POST);
            req.AddParameter("message", comment);
            var json = await _re.ExecuteFbRequestAsync(req);
        }
    }
}