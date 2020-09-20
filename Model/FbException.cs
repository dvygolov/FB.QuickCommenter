using FB.QuickCommenter.Helpers;
using Newtonsoft.Json.Linq;
using System;

namespace FB.QuickCommenter.Model
{
    public class FbException : Exception
    {
        public FbErrorSubCodes Code { get; private set; }
        public JObject OriginalJson { get; private set; }

        public FbException(JObject json) : base()
        {
            OriginalJson = json;
            var subCode = ErrorChecker.GetErrorSubCode(json);
            var parsed = Enum.TryParse(subCode, out FbErrorSubCodes code);
            if (parsed)
                Code = code;
            else
                Code = FbErrorSubCodes.Other;
        }

        public override string ToString()
        {
            var error = OriginalJson["error"]?["message"]?.ToString()??"";
            var eum = OriginalJson["error"]?["error_user_msg"]?.ToString()??"";
            var eut = OriginalJson["error"]?["error_user_title"]?.ToString()??"";

            return $"Fb вернул ошибку: {error} {eut} {eum} {Code}";
        }
    }
}
