using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FB.QuickCommenter.Helpers
{
    public static class CommentsHelper
    {
        private const string BaseDirName = "comments";
        public static List<string> GetComments()
        {
            var f = SelectCommentsFolder();
            var filePath = Path.Combine(f, "comments.txt");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Не найден файл comments.txt!");
            return File.ReadAllLines(filePath).ToList();
        }

        private static string SelectCommentsFolder()
        {
            Console.WriteLine("Выберите папку с комментариями:");
            int i = 1;
            var cmntfs = Directory.EnumerateDirectories(BaseDirName).ToList();
            foreach (var f in cmntfs)
            {
                Console.WriteLine($"{i}.{Path.GetFileName(f)}");
                i++;
            }
            Console.Write("Ваш выбор:");
            var index = int.Parse(Console.ReadLine()) - 1;
            return cmntfs[index];
        }
    }
}
