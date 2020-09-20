using FB.QuickCommenter.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FB.QuickCommenter.Helpers
{
    public class BulkHelper
    {
        public static async Task BulkProcessAsync(
            string fbApiAddress, Func<RequestExecutor, ConnectSettings, Task> process)
        {
            var lines = File.ReadAllLines("accounts.txt");
            var connections = lines.Select(l => new ConnectSettings(l)).ToList();

            int successCount = 0;
            Console.WriteLine($"Всего подключений:{connections.Count}");
            for (var i = 0; i < connections.Count; i++)
            {
                var c = connections[i];
                try
                {
                    Console.WriteLine($"Обработка подключения #{i+1}...");
                    var re = new RequestExecutor(fbApiAddress, c);
                    await process(re, c);
                    successCount++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            Console.WriteLine($"Успешно обработано:{successCount}");
        }
    }
}
