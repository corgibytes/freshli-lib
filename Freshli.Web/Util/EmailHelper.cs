using System;
using Freshli.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Freshli.Web.Util
{
  public static class EmailHelper
  {
    private static readonly string SendGridApiKey =
    Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

    private static readonly SendGridClient Client =
      new SendGridClient(SendGridApiKey);

    private static readonly EmailAddress FromAddress =
      new EmailAddress("info@freshli.io", "Freshli");

    private static readonly string BaseUrl = "http://localhost:5000";

    public static void SendKickoffEmail(AnalysisRequest analysisRequest)
    {
      Client.SendEmailAsync(CreateKickoffMessage(analysisRequest));
    }

    public static void SendResultsReadyEmail(AnalysisRequest analysisRequest)
    {
      Client.SendEmailAsync(CreateResultsReadyMessage(analysisRequest));
    }

    private static SendGridMessage CreateKickoffMessage(
      AnalysisRequest analysisRequest)
    {
      var plainTextContent = $"Hi {analysisRequest.Name}!\n" +
                             "Thanks for trying Freshli! We're currently " +
                             $"analyzing {analysisRequest.Url} and will send " +
                             "you a follow-up email as soon as your results " +
                             "are ready!";

      string htmlContent = null;

      return MailHelper.CreateSingleEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        "Thanks for trying Freshli!",
        plainTextContent,
        htmlContent);
    }

    private static SendGridMessage CreateResultsReadyMessage(
      AnalysisRequest analysisRequest)
    {
      var plainTextContent = $"Hi {analysisRequest.Name}, Good news! " +
                             "Your Freshli analysis for " +
                             $"{analysisRequest.Url} is ready! Check out the " +
                             $"results at {BaseUrl}" +
                             $"/AnalysisRequests/{analysisRequest.Id}";

      string htmlContent = null;

      return MailHelper.CreateSingleEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        "Your Freshli analysis is ready!",
        plainTextContent,
        htmlContent);
    }

  }
}
