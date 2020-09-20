using FB.QuickCommenter.Model;
using Newtonsoft.Json.Linq;
using System;

namespace FB.QuickCommenter.Helpers
{
    public static class ErrorChecker
    {
        public static bool HasErrorsInResponse(JObject json, bool throwException = false, bool printError = true)
        {
            var error = json["error"]?["message"].ToString();
            if (!string.IsNullOrEmpty(error))
            {
                if (json["error"]["error_subcode"]?.ToString() == "1487390"||
                    json["error"]["error_subcode"]?.ToString() == "1346003")
                {
                    var msg = "Скорее всего в вашем запросе находится что-то забаненое! Проверьте!!";
                    if (throwException)
                        throw new FbException(json);
                    if (printError)
                        Console.WriteLine(msg);
                    return true;
                }
                else
                {
                    var msg = $"Ошибка при попытке выполнить запрос:{json["error"]}!";
                    if (printError)
                        Console.WriteLine(msg);
                    if (throwException)
                        throw new FbException(json);
                    return true;
                }
            }
            return false;
        }

        public static string GetErrorSubCode(JObject json)
        {
            return json["error"]?["error_subcode"]?.ToString();
        }
    }
}