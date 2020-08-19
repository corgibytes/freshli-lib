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

    private static readonly string KickoffEmailTemplateId =
    Environment.GetEnvironmentVariable("KICKOFF_EMAIL_TEMPLATE_ID");

    private static readonly string ResultsReadyEmailTemplateId =
    Environment.GetEnvironmentVariable("RESULTS_READY_EMAIL_TEMPLATE_ID");

    private static readonly SendGridClient Client =
      new SendGridClient(SendGridApiKey);

    private static readonly EmailAddress FromAddress =
      new EmailAddress("info@freshli.io", "Freshli");

    public static void SendKickoffEmail(AnalysisRequest analysisRequest)
    {
      Client.SendEmailAsync(CreateKickoffMessage(analysisRequest));
    }

    public static void SendResultsReadyEmail(AnalysisRequest analysisRequest,
      string baseUrl)
    {
      Client.SendEmailAsync(
        CreateResultsReadyMessage(analysisRequest, baseUrl));
    }

    private static SendGridMessage CreateKickoffMessage(
      AnalysisRequest analysisRequest)
    {
      var data = new Dictionary<string, string>
      {
        {"Url", analysisRequest.Url}
      };

      return MailHelper.CreateSingleTemplateEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        KickoffEmailTemplateId,
        data);
    }

    private static SendGridMessage CreateResultsReadyMessage(
      AnalysisRequest analysisRequest, string baseUrl)
    {
      var data = new Dictionary<string, string>
      {
        {"Url", analysisRequest.Url},
        {"ResultsUrl", $"{baseUrl}/AnalysisRequests/{analysisRequest.Id}"}
      };

      return MailHelper.CreateSingleTemplateEmail(
        FromAddress,
        new EmailAddress(analysisRequest.Email, analysisRequest.Name),
        ResultsReadyEmailTemplateId,
        data);
    }

  }
}
