using FB.QuickCommenter.Model;
using System;

namespace FB.QuickCommenter.Helpers
{
    public class ProxyHelper
    {
        public static void FillProxy(ConnectSettings conns)
        {
            Console.Write("Введите ip-адрес прокси,либо полностью через : (Enter,если не нужен):");
            conns.ProxyAddress = Console.ReadLine();

            if (string.IsNullOrEmpty(conns.ProxyAddress))
            {
                Console.WriteLine("Не используем прокси!");
            }
            else
            {
                var split = conns.ProxyAddress.Split(':');
                if (split.Length == 4)
                {
                    conns.ProxyAddress = split[0];
                    conns.ProxyPort = split[1];
                    conns.ProxyLogin = split[2];
                    conns.ProxyPassword = split[3];
                }
                else
                {
                    Console.Write("Введите порт прокси:");
                    conns.ProxyPort = Console.ReadLine();
                    Console.Write("Введите логин прокси:");
                    conns.ProxyLogin = Console.ReadLine();
                    Console.Write("Введите пароль прокси:");
                    conns.ProxyPassword = Console.ReadLine();
                }
            }
        }
    }
}