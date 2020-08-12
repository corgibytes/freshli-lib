using System;
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

    public static void SendNotificationEmail(string name, string email,
      string url)
    {
      Client.SendEmailAsync(CreateNotificationMessage(name, email, url));
    }

    public static void SendResultsEmail(string name, string email, string url,
      string guid)
    {
      Client.SendEmailAsync(CreateResultsMessage(name, email, url, guid));
    }

    private static SendGridMessage CreateNotificationMessage(string name,
      string email, string url)
    {
      var plainTextContent = $"Hi {name}, thanks for trying Freshli! " +
                             $"We're currently analyzing {url} and will send " +
                             "you a followup email as soon as your results " +
                             "are ready!";

      string htmlContent = null;

      return MailHelper.CreateSingleEmail(
        FromAddress,
        new EmailAddress(email, name),
        "Thanks for trying Freshli!",
        plainTextContent,
        htmlContent);
    }

    private static SendGridMessage CreateResultsMessage(string name,
      string email, string url, string guid)
    {
      var plainTextContent = $"Hi {name}, \n Good news! Your Freshli " +
                             $"analysis for {url} is ready! Check out the " +
                             $"results at " +
                             $"http://localhost:5000/AnalysisRequests/{guid}";

      string htmlContent = null;

      return MailHelper.CreateSingleEmail(
        FromAddress,
        new EmailAddress(email, name),
        "Your Freshli analysis is ready!",
        plainTextContent,
        htmlContent);
    }

  }
}
