using System;
using System.Collections.Generic;
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

    //TODO: Set baseUrl dynamically
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
      var data = new Dictionary<string, string>
      {
        {"Name", analysisRequest.Name},
        {"Url", analysisRequest.Url}
      };

      return MailHelper.CreateSingleTemplateEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        "d-0c57ea9815824b0f8c99483a3c355a58",
        data);
    }

    private static SendGridMessage CreateResultsReadyMessage(
      AnalysisRequest analysisRequest)
    {
      var data = new Dictionary<string, string>
      {
        {"Url", analysisRequest.Url},
        {"ResultsUrl", $"{BaseUrl}/AnalysisRequests/{analysisRequest.Id}"}
      };

      return MailHelper.CreateSingleTemplateEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        "d-9dbbeb69748c4bb9854595a6744a6ddb",
        data);
    }

  }
}
