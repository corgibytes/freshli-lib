using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Freshli.Web.Util
{
  public static class RecaptchaHelper
  {
    private const string GoogleRecaptchaUrl =
      "https://www.google.com/recaptcha/api/siteverify";

    private static readonly string SecretKey =
      Environment.GetEnvironmentVariable("RECAPTCHA_SECRET_KEY");

    public static bool ValidateRecaptchaResponse(string response)
    {
      var isValid = false;

      var requestUri =
        $"{GoogleRecaptchaUrl}?secret={SecretKey}&response={response}";
      var request = WebRequest.Create(requestUri);
      var responseStream = request.GetResponse().GetResponseStream();
      if (responseStream != null)
      {
        var reader = new StreamReader(responseStream);
        var jResponse = JObject.Parse(reader.ReadToEnd());
        isValid = jResponse.Value<bool>("success");
      }

      return isValid;
    }
  }
}
